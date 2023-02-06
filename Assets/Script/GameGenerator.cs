using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Events;


/// 플레이어 움직임에 따른 구름 생성과 움직임 조작
/// 플레이어 이동, 시간 경과 후 출현하는 오브젝트

public class GameGenerator : MonoBehaviour
{
    //해당 내용은 시트 호출을 위해 사용하는 내용들
    private string id = "1RgsaoZoHw5pxwQVwIlxV_H8knM3nIw63g98mXzcY9Cc";
    private string CarSheetName = "CarDatas";

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(id, CarSheetName), callback, mergedCells);
    }
    

}

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