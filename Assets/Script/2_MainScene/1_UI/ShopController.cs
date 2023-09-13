using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;

/// 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본
public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject itemScrollInfo;


    private void Start()
    {
        Transform childTransform = itemScrollInfo.transform.Find("");
    }


    void FindHavingImageObject()
    {
        for (int i = 0; i == 2; i++)
        {
            Transform childTransform = itemScrollInfo.transform.GetChild(i);

            if (childTransform != null)
            {
                Image imageComponent = childTransform.GetComponent<Image>();

                if (imageComponent != null)
                {
                    imageComponent.sprite = null; // 사진의 이미지를 상점의 내용에 표시될 상점 매니저 또는 게임 매니저를 통해서 값들을 저장해주고 해당 값에 맞는 정보를 불러오게 한다.
                }
            }
            
            
        }
    }
}