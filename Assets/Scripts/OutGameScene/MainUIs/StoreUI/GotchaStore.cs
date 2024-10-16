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
        UIManager.gotchaInterface.SetGotchaInterface(tier);
        ActiveGotchaInterface();
    }

    private void ActiveGotchaInterface()
    {
        StartCoroutine(GetGotchaInterface());
    }
    
    //��í �������̽��� ����� ����ϴٰ� �ش� ����� ���� ��Ʈ ����
    private IEnumerator GetGotchaInterface()
    {
        GotchaInterface gotcha = UIManager.gotchaInterface;
        // TF �������̽����� ����� ��ٸ�
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

    //���� �������� ����üũ
    private void DoubleCheck(int tier, int costId, int costAmount)
    {
        UIManager.tfInterface.SetTFContent("������ ��í�� �����Ͻðڽ��ϱ�?");
        StartCoroutine(TFCheck(tier, costId, costAmount));
    }

    //����üũ�� ��� ��� �� ������Ʈ ����
    private IEnumerator TFCheck(int tier, int costId, int costAmount)
    {
        TFInterface tFInterface = UIManager.tfInterface;

        yield return StartCoroutine(tFInterface.GetValue());

        if ((bool)tFInterface.result)
        {
            //����üũ �Ϸ�� ��í ����
            Gotcha(tier, costId,costAmount);
        }
        else
        {
            UIManager.alertInterface.SetAlert($"��í�� ��ҵǾ����ϴ�");
        }
    }

    //��í�� ����
    public void Gotcha(int tier, int costMasterId, int costAmount)
    {
        //todo -> ��í �ý��� �߰��Ұ�
        //�ش� ��ȭ�� ������ �ִ��� Ȯ��

        GotchaData curGotchaData = DataManager.gotcha.GetData(tier);

        //�밡 ������(�̳׶� Ȥ�� ����)�� ������� üũ�ϰ� ������ �������̽��� ���� ��í ���
        if(!DataManager.inven.IsEnoughItem(DataManager.inven.GetDataWithMasterId(costMasterId).id, costAmount))
        {
            UIManager.alertInterface.SetAlert($"{DataManager.master.GetData(costMasterId).name}�� �����մϴ�");
            return;
        }
        else
        {
            //����ϸ� �ش� ������ ����
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

        //(�ʿ��) ������ ������ ������

    }

    //�ش� ��í�� �����ͷ� ��� �����͵��� �ϳ��� ��������
    public string GetRandomItem(GotchaData gotcha)
    {
        float totalRate = 0;

        // �� Ȯ���� ���
        foreach (var item in gotcha.items)
        {
            totalRate += item.rate;
        }

        // 0���� �� Ȯ�� ������ ������ ���ڸ� ����
        float randomNumber = Random.Range(0, totalRate);
        float cumulativeRate = 0;

        // ���� Ȯ���� ����ϸ� ������ ����
        foreach (var item in gotcha.items)
        {
            cumulativeRate += item.rate;
            if (randomNumber < cumulativeRate)
            {
                return item.type;
            }
        }

        return null; // �⺻������ ������� ����
    }

    //������ ������ ��� ��ũ�� ���� ������ �κ��丮�� �������ݿ� �߰�
    private void AddParts(int partsRank)
    {
        List<MasterData> partsList = DataManager.master.GetItemsByType(MasterType.Parts);
        int random = Random.Range(0, partsList.Count);
        MasterData ChosenPartsData = partsList[random];

        //�κ��丮�� �ش� ���� �߰�
        InvenData newPartsInven = new InvenData()
        {
            id = DataManager.inven.GetLastKey() + 1,
            masterId = ChosenPartsData.id,
            name = DataManager.master.GetData(ChosenPartsData.id).name,
            quantity = 1
        };
        

        //���� ���ݿ� ���ݵ� ����
        PartsAbilityData newParts = new PartsAbilityData()
        {
            invenId = newPartsInven.id,
            isActive = false,
            level = 1,
            rank = partsRank
        };

        newParts.mainAbility = SetMainAbility(ChosenPartsData.id);
        newParts.subAbilities = new List<Ability>();

        //������ ��ũ = ��������Ƽ�� ��
        List<AbilityData> available = DataManager.ability.GetDictionary().Values.Where(ability => ability.minRank <= partsRank).ToList();
        for (int i =0; i< partsRank; i++)
        {
            int subRandom = Random.Range(0, available.Count); //�����ϸ� ����� �����ϼ��� Ȯ���� ���� �ϱ�
            int value = GetValue(available[subRandom].id, partsRank);
            Ability newSubAbility = new Ability(available[subRandom].id, value);
            newParts.subAbilities.Add(newSubAbility);
            
        }

        //������ �߰� �� ���̺�
        DataManager.inven.AddData(newPartsInven);
        DataManager.inven.SaveData();
        DataManager.parts.AddData(newParts);
        DataManager.parts.SaveData();
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

    //���� �����Ƽ�� �߰���
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

    private void AddRandomItem()
    {
        List<MasterData> consumeList = DataManager.master.GetItemsByType(MasterType.Consume);

        int random = Random.Range(0, consumeList.Count);
        DataManager.inven.DataAddOrUpdate(consumeList[random].id, 1);
        DataManager.inven.SaveData();
    }

}