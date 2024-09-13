using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // 파일 쓰기 작업에 필요
using System.Text;
using System.Linq; // 텍스트 인코딩에 필요


public class InventoryDataReader : MonoBehaviour
{
    public Dictionary<int, InvenItemData> InvenItemDic; //코드를 통해 검색용

    private void Awake()
    {
        LoadData();
    }

    public void LoadData()
    {
        InvenItemDic = DataManager.SetDictionary<InvenItemData, InvenItemDatas>("JSON/Writable/InventoryData",
            data => data.storageItems,
            item => item.storageId
            );

    }

    // 데이터를 수정하고 JSON 파일을 저장하는 메서드
    public void SaveData()
    {
        // InvenItemDatas 객체 생성
        InvenItemDatas dataInstance = new InvenItemDatas
        {
            storageItems = InvenItemDic.Values.ToArray()
        };

        // 데이터를 JSON 문자열로 변환
        string jsonData = JsonUtility.ToJson(dataInstance, true);

        // JSON 데이터를 저장할 파일 경로 설정 (쓰기 가능한 경로)
        string path = Path.Combine(Application.persistentDataPath, "InventoryData.json");

        // JSON 파일로 저장
        File.WriteAllText(path, jsonData, Encoding.UTF8);

        Debug.Log($"InvenData : JSON 파일이 {path}에 저장됨");
    }

    /// <summary>
    /// 해당 아이템이 인벤토리에 존재하면 개수 업데이트, 없으면 인벤 데이터 추가
    /// </summary>
    public void AddNewItem(int itemType, int masterId, string name, int amount)
    {
        InvenItemData? findItem = FindByMasterId(masterId);
        if (findItem != null)  //더하려는 아이템이 이미 존재한다면 
        {
            ModifyItem(findItem.Value.storageId, findItem.Value.amount + amount);
            return;
        }

        //확인결과 인벤토리에 존재하지 않는 아이템 -> 새로운 인벤토리 데이터 추가
        int newStorageId = 0; // 기본값
        if (InvenItemDic.Count > 0)
        {
            newStorageId = InvenItemDic[InvenItemDic.Count - 1].storageId + 1;
        }


        InvenItemData newItem = new InvenItemData();
        newItem.storageId = newStorageId;
        newItem.itemType = itemType;
        newItem.masterId = masterId;
        newItem.name = name;
        newItem.amount = amount;
        

        InvenItemDic.Add(newStorageId, newItem);
    }

    // 기존의 아이템 데이터를 수정
    public void ModifyItem(int storageId, int newAmount)
    {
        if (InvenItemDic.ContainsKey(storageId))
        {
            InvenItemData item = InvenItemDic[storageId];
            item.amount = newAmount; // 아이템의 수량을 변경
            InvenItemDic[storageId] = item;
            Debug.Log($"InvenData : 아이템 {item.name}의 수량이 {newAmount}로 수정됨");
        }
        else
        {
            Debug.Log($"InvenData : ID {storageId}를 가진 아이템이 존재하지 않음");
        }
    }

    // todo -> 아이템을 삭제하는 코드 추가할것. storageId에 빈공간이 생길시 그 뒤에 요소들을 땡겨서 빈공간 제거

    public InvenItemData? FindByMasterId(int masterId)
    {
        foreach (KeyValuePair<int, InvenItemData> pair in InvenItemDic)
        {
            if (pair.Value.masterId == masterId)
            {
                return pair.Value; // 해당 masterId를 가진 아이템을 찾음
            }
        }
        Debug.Log("InvenData : 해당 masterId를 가진 아이템이 없습니다.");
        return null; // 해당 아이템이 없을 경우 null 반환
    }
}
