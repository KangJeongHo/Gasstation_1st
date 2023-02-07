using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GoogleSheetsToUnity.ThirdPary;
using Random = System.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataFromSheetsTools : ScriptableObject
{
    public string associatedSheet = "";
    public string associatedWorksheet = "";

    public List<string> items = new List<string>();
    
    public List<string> Names = new List<string>();
    internal void UpdateStats(List<GSTU_Cell> list, string name)
    {
        items.Clear();

        string CarName = null, CarGasType = null, CarImage = null;
        int CarGasAmountMin = 0, CarGasAmountMax = 0, CarGasAmountRand = 0,
            CarLimitTimeMin = 0, CarLimitTimeMax = 0, CarLimitTimeRand = 0,
            CarSatisfactionMin = 0, CarSatisfactionMax = 0, CarSatisfactionRand = 0,
            CarZenProbability = 0, CarOpenLevel = 0;
        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "CarName":
                {
                    CarName = list[i].value;
                    break;
                }
                case "CarGasType":
                {
                    CarGasType = list[i].value;
                    break;
                }
                case "CarGasAmountMin":
                {
                    CarGasAmountMin = int.Parse(list[i].value);
                    CarGasAmountMax = int.Parse(list[i].value);
                    CarGasAmountRand = UnityEngine.Random.Range(CarGasAmountMin, CarGasAmountMax);
                    break;
                }
                case "CarLimitTimeMin":
                {
                    CarLimitTimeMin = int.Parse(list[i].value);
                    CarLimitTimeMax = int.Parse(list[i].value);
                    CarLimitTimeRand = UnityEngine.Random.Range(CarLimitTimeMin, CarLimitTimeMax);
                    break;
                }
                case "CarSatisfactionMin":
                {
                    CarSatisfactionMin = int.Parse(list[i].value);
                    CarSatisfactionMax = int.Parse(list[i].value);
                    CarSatisfactionRand = UnityEngine.Random.Range(CarSatisfactionMin, CarSatisfactionMax);
                    break;
                }
                case "CarZenProbability":
                {
                    CarZenProbability = int.Parse(list[i].value);
                    break;
                }
                case "CarOpenLevel":
                {
                    CarOpenLevel = int.Parse(list[i].value);
                    break;
                }
                case "CarImage": //얘는 바로 불러오던가 아니면 글자를 찾아온 다음 게임 안에서 불러와야 할 수도 있음.
                {
                    CarImage = list[i].value;
                    break;
                }
            }
        }
        /*
        Debug.Log($"{name}의 정보 차량 이름 :{CarName} 주유 희망 량 :{CarGasAmountRand} 차량 인내심 시간 :{CarLimitTimeRand}" +
                  $" 차량 만족도 : {CarSatisfactionRand} 차량 등장 확률 : {CarZenProbability} 차량 등장 레벨 : {CarOpenLevel}" +
                  $" 차량 유종 : {CarGasType} 차량 사진 : {CarImage}");
        */
        
        
        Debug.Log(name);
    }

}
public class DataFromSheets : MonoBehaviour
{
    private void Start()
    {

        //DataFromSheetsTools;
    }
}

[CustomEditor(typeof(DataFromSheetsTools))]
public class DataFromSheetsEditor : Editor
{
    DataFromSheetsTools data;

    void OnEnable()
    {
        data = (DataFromSheetsTools)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Read Data Examples");

        if (GUILayout.Button("Pull Data Method One"))
        {
            UpdateStats(UpdateMethodOne);
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        //data.UpdateStats(ss.rows["Jim"]);
        foreach (string dataName in data.Names)
            data.UpdateStats(ss.rows[dataName], dataName);
        EditorUtility.SetDirty(target);
    }
    
}