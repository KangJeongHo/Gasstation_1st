using UnityEngine;
using System.Collections;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using GoogleSheetsToUnity.ThirdPary;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class TestData : ScriptableObject
{
    public string associatedSheet = "";
    public string associatedWorksheet = "";

    public List<string> items = new List<string>();
    
    public List<string> Names = new List<string>();
    internal void UpdateStats(List<GSTU_Cell> list, string name)
    {
        items.Clear();
        
/*
 CarName : char
 CarGasAmount : int
 CarGasType : enum
 CarLimitTIme : int
 CarSatisfaction : int
 CarZenProbability : int
 CarOpenLevel : int 
 CarImage : string(?) / 바로 불러올 수 있는지 알아보는중. 

*/
        int math=0, korean=0, english=0;
        string CarName, int CarGasAmount = 0;, enum CarGasType, int CarLimitTime, int CarSttisfaction, int CarZenProbability, int CarOpenLevel, string
            CarImage;
        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "CarName":
                {
                    CarName = null;//int.Parse(list[i].value);
                    break;
                }
                case "CarGasAmount":
                {
                    CarGasAmount = int.Parse(list[i].value);
                    break;
                }
                case "English":
                {
                    english = int.Parse(list[i].value);
                    break;
                }
            }
        }
        Debug.Log($"{name}의 점수 수학:{CarName} 국어:{CarGasAmount} 영어:{english}");
    }

}

[CustomEditor(typeof(TestData))]
public class DataEditor : Editor
{
    TestData data;

    void OnEnable()
    {
        data = (TestData)target;
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