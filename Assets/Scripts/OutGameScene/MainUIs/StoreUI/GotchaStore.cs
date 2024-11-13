using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;

public class GotchaStore : MonoBehaviour
{
    public Transform gotchas;
    public Button gotcha1;
    public Button gotcha2;
    public Button gotcha3;

    public Button[] gotchaBtns;

    private void Awake()
    {
        gotchas = transform.GetChild(0);
        gotchaBtns = new Button[3];
    }

    private void OnEnable()
    {
        for (int i = 0; i < gotchas.childCount; i++)
        {
            gotchaBtns[i] = gotchas.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < gotchaBtns.Length; i++)
        {
            int code = i+1; 
            gotchaBtns[i].onClick.RemoveAllListeners();
            gotchaBtns[i].onClick.AddListener(() => OpenGotchaUI(code));
        }
    }

    private void OpenGotchaUI(int tier)
    {
        OG_UIManager.gotchaInterface.SetGotchaInterface(tier);
        ActiveGotchaInterface();
    }

    private void ActiveGotchaInterface()
    {
        StartCoroutine(GetGotchaInterface());
    }
    
    //가챠 인터페이스의 결과를 대기하다가 해당 결과로 다음 루트 실행
    private IEnumerator GetGotchaInterface()
    {
        GotchaInterface gotcha = OG_UIManager.gotchaInterface;
        // TF 인터페이스에서 결과를 기다림
        yield return StartCoroutine(gotcha.GetValue());

        if ((bool)gotcha.result)
        {
            DoubleCheck(gotcha.gotchaTier, gotcha.acceptCostId, gotcha.acceptCostAmount);
        }
        else
        {
            gotcha.CloseInterface();
        }
    }

    //정말 진행할지 이중체크
    private void DoubleCheck(int tier, int costId, int costAmount)
    {
        OG_UIManager.tfInterface.SetTFContent("정말로 가챠를 진행하시겠습니까?");
        StartCoroutine(TFCheck(tier, costId, costAmount));
    }

    //이중체크의 결과 대기 및 다음루트 실행
    private IEnumerator TFCheck(int tier, int costId, int costAmount)
    {
        TFInterface tFInterface = OG_UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //더블체크 완료시 가챠 실행
            Gotcha(tier, costId,costAmount);
        }
        else
        {
            OG_UIManager.alertInterface.SetAlert($"가챠가 취소되었습니다");
        }
    }

    //가챠의 시작
    public void Gotcha(int tier, int costMasterId, int costAmount)
    {
        //todo -> 가챠 시스템 추가할것
        //해당 재화를 가지고 있는지 확인

        GotchaData curGotchaData = DataManager.gotcha.GetData(tier);

        //대가 아이템(미네랄 혹은 쿠폰)이 충분한지 체크하고 없으면 인터페이스를 띄우며 가챠 취소
        if(!DataManager.inven.IsEnoughItem(DataManager.inven.GetDataWithMasterId(costMasterId).id, costAmount))
        {
            OG_UIManager.alertInterface.SetAlert($"{DataManager.master.GetData(costMasterId).name}이 부족합니다");
            return;
        }
        else
        {
            //충분하면 해당 데이터 감소
            DataManager.inven.DataUpdateOrDelete(DataManager.inven.GetDataWithMasterId(costMasterId).id, costAmount);
        }

        string chosenType = GetRandomItem(curGotchaData);
        switch (chosenType)
        {
            case "S": AddParts(5);  break;
            case "A": AddParts(4); break;
            case "B": AddParts(3); break;
            case "C": AddParts(2); break;
            case "D": AddParts(1); break;
            case "consume": AddRandomItem(); break;
        }

        //(필요시) 생성된 파츠를 보여줌

    }

    //해당 가챠의 데이터로 대상 데이터들중 하나를 랜덤선택
    public string GetRandomItem(GotchaData gotcha)
    {
        float totalRate = 0;

        // 총 확률을 계산
        foreach (var item in gotcha.items)
        {
            totalRate += item.rate;
        }

        // 0부터 총 확률 사이의 무작위 숫자를 생성
        float randomNumber = Random.Range(0, totalRate);
        float cumulativeRate = 0;

        // 누적 확률을 계산하며 아이템 선택
        foreach (var item in gotcha.items)
        {
            cumulativeRate += item.rate;
            if (randomNumber < cumulativeRate)
            {
                return item.type;
            }
        }

        return null; // 기본적으로 실행되지 않음
    }

    //파츠가 나왔을 경우 랭크에 따라 파츠를 인벤토리와 파츠스텟에 추가
    private async void AddParts(int partsRank)
    {
        List<MasterData> partsList = DataManager.master.GetItemsByType(MasterType.Parts);
        int random = Random.Range(0, partsList.Count);
        MasterData ChosenPartsData = partsList[random];

        //인벤토리에 해당 파츠 추가
        InvenData newPartsInven = new InvenData()
        {
            id = DataManager.inven.GetLastKey() + 1,
            masterId = ChosenPartsData.id,
            name = DataManager.master.GetData(ChosenPartsData.id).name,
            quantity = 1
        };
        

        //파츠 스텟에 스텟들 설정
        PartsData newParts = new PartsData()
        {
            invenId = newPartsInven.id,
            isActive = false,
            level = 1,
            rank = partsRank
        };

        newParts.mainAbility = SetMainAbility(ChosenPartsData.id);
        newParts.subAbilities = new List<Ability>();

        //파츠의 랭크 = 서브어빌리티의 수
        List<AbilityData> available = DataManager.ability.GetDictionary().Values.Where(ability => ability.minRank <= partsRank).ToList();
        for (int i =0; i< partsRank; i++)
        {
            int subRandom = Random.Range(0, available.Count); //가능하면 희귀한 스텟일수록 확률이 낮게 하기
            int value = GetValue(available[subRandom].id, partsRank);
            Ability newSubAbility = new Ability(available[subRandom].id, value);
            newParts.subAbilities.Add(newSubAbility);
            
        }

        //데이터 추가 및 세이브
        DataManager.inven.AddData(newPartsInven);
        await DataManager.inven.SaveData();
        DataManager.parts.AddData(newParts);
        await DataManager.parts.SaveData();
    }

    private int GetValue(int key ,int rank)
    {
        AbilityData targetAbilityData= DataManager.ability.GetData(key);
        AbilityRate targetAbilityRate = new AbilityRate();
        switch (rank)
        {
            case 1: targetAbilityRate = targetAbilityData.DRange; break;
            case 2: targetAbilityRate = targetAbilityData.CRange; break;
            case 3: targetAbilityRate = targetAbilityData.BRange; break;
            case 4: targetAbilityRate = targetAbilityData.ARange; break;
            case 5: targetAbilityRate = targetAbilityData.SRange; break;
        }

        int value = Random.Range(targetAbilityRate.min, targetAbilityRate.max+1);
        return value;
    }

    //메인 어빌리티를 추가함
    private Ability SetMainAbility(int id)
    {
        int targetAbilityCode = 0;
        switch (id)
        {
            case 201: targetAbilityCode = 1; break;
            case 202: targetAbilityCode = 3; break;
            case 203: targetAbilityCode = 4; break;
            case 204: targetAbilityCode = 1; break;
            case 205: targetAbilityCode = 5; break;
            case 206: targetAbilityCode = 2; break;
        }

        Ability newAbility = new Ability(targetAbilityCode, DataManager.ability.GetData(targetAbilityCode).basicValue);
        return newAbility;
    }

    private async void AddRandomItem()
    {
        List<MasterData> consumeList = DataManager.master.GetItemsByType(MasterType.Consume);

        int random = Random.Range(0, consumeList.Count);
        DataManager.inven.DataAddOrUpdate(consumeList[random].id, 1);
        await DataManager.inven.SaveData();
    }

}