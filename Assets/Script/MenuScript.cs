using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace Script
{
    public class MenuScript : MonoBehaviour
    {
        public GameObject Menu_Button;

        private void Start()
        {
            Menu_Button.AddComponent<Menu>();
            if (GameDirector.MenuValue.Menu_Cancle_Panel == null)
            {
                Debug.LogWarning("sdasda");
            }
            GameDirector.MenuValue.Menu_Cancle_Panel.AddComponent<Menu_Cancle_Panel>();

        }
    }
}



#region Menu

public class Menu_Cancle_Panel : MonoBehaviour ,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameDirector.MenuLogicValue.Is_Menu_Cancle = true;
        gameObject.SetActive(false);
    }
}
public class Menu : MonoBehaviour,IPointerClickHandler
{
    private GameObject InstObject;

    private Vector3 Reset_Pre_InstObject_Pos = Vector3.zero;
    private Vector3 Pre_InstObject_Pos = Vector3.zero;
    
    private int Menu_Count = 0;
    private List<GameObject> Menu_List = null;
    bool IsMenu_ClickAvailable = true;


    void Menu_Detroy()
    {
        if (true == GameDirector.MenuLogicValue.Is_Menu_Cancle)
        {
            for (int i = 0; i < Menu_List.Count; i++)
            {
                Destroy(Menu_List[i]);
            }

            Menu_Count = 0;
            Pre_InstObject_Pos = Reset_Pre_InstObject_Pos;

            GameDirector.MenuLogicValue.Is_Menu_Cancle = false;
            IsMenu_ClickAvailable = true;
        }
    }
        void Menu_Cancle_Panel_Active()
    {
        GameDirector.MenuValue.Menu_Cancle_Panel.SetActive(true);
    }
    void Open_Menu_Create() //메뉴칸 만들고 마지막 메뉴칸에 곂치게
    {
        InstObject = GameObject.Instantiate(GameDirector.MenuValue.Button);
        Menu_List.Add(InstObject);
        InstObject.transform.SetParent(this.transform);
        InstObject.transform.localScale = Vector3.one;
        InstObject.transform.localPosition = Pre_InstObject_Pos;
        
    }
    
    IEnumerator Open_Menu() // 메뉴 누르면 나오게 , 아래로 천천히 
    {
        Open_Menu_Create();

        Vector3 Origin = InstObject.transform.position;
        while (Origin.y - GameDirector.MenuValue.Menu_Distance <= InstObject.transform.position.y)
        {
            InstObject.transform.position += Vector3.down *Time.deltaTime * GameDirector.MenuValue.Menu_Down_Speed;
            if (Origin.y - GameDirector.MenuValue.Menu_Distance > InstObject.transform.position.y)
            {
                InstObject.transform.position = Origin + Vector3.down * GameDirector.MenuValue.Menu_Distance;
            }
            
            yield return null;
        }

        Menu_Count += 1;
        Pre_InstObject_Pos = InstObject.transform.localPosition;
        Debug.Log(Menu_Count);
        if (Menu_Count < GameDirector.MenuValue.Menu_Count) // 메뉴 갯수만큼 반복
        {
            StartCoroutine(Open_Menu());
        }
    }
            
    public void OnPointerClick(PointerEventData eventData)
    {
        if (true == IsMenu_ClickAvailable)
        {
            IsMenu_ClickAvailable = false;
            
            Menu_Cancle_Panel_Active();
            StartCoroutine(Open_Menu());
        }
    }

    private void Awake()
    {
        Menu_List = new List<GameObject>();
        Reset_Pre_InstObject_Pos = Pre_InstObject_Pos;
    }

    private void Update()
    {
        Menu_Detroy();
    }
}
#endregion
