using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
                Debug.LogWarning("if (GameDirector.MenuValue.Menu_Cancle_Panel == null)");
            }
            GameDirector.MenuValue.Menu_Cancle_Panel.AddComponent<Menu_Cancle_Panel>();

        }
    }
}

#region Menu_Sub

enum MENU_LIST
{
    SETTING,
    SHOP,
}


public class Menu_Shop : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("ShopScene");
    }
}
public class Menu_Setting : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

    }
}

#endregion

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
    MENU_LIST m_Menu_List = MENU_LIST.SHOP;

    private GameObject InstObject;

    private Vector3 Reset_Pre_InstObject_Pos = Vector3.zero;
    private Vector3 Pre_InstObject_Pos = Vector3.zero;
    
    private int Menu_Count = 0;
    private List<GameObject> Exist_Menu = null;
    bool IsMenu_ClickAvailable = true;

    void class_In_InstObject(MENU_LIST _List)
    {
        if(_List == MENU_LIST.SETTING)
        {
            InstObject.AddComponent<Menu_Setting>();
        }
        else if (_List == MENU_LIST.SHOP)
        {
            InstObject.AddComponent<Menu_Shop>();
        }
    }

    void Menu_Detroy()
    {
        if (true == GameDirector.MenuLogicValue.Is_Menu_Cancle)
        {
            for (int i = 0; i < Exist_Menu.Count; i++)
            {
                Destroy(Exist_Menu[i]);
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
        Exist_Menu.Add(InstObject);
        InstObject.transform.SetParent(this.transform);
        InstObject.transform.localScale = Vector3.one;
        InstObject.transform.localPosition = Pre_InstObject_Pos;
        
    }
    
    IEnumerator Open_Menu() // 메뉴 누르면 나오게 , 아래로 천천히 
    {
        Open_Menu_Create();
        
        Vector3 Origin = InstObject.transform.localPosition;

        while (Origin.y - GameDirector.MenuValue.Menu_Distance < InstObject.transform.localPosition.y)
        {
            InstObject.transform.localPosition += Vector3.down *Time.deltaTime * GameDirector.MenuValue.Menu_Down_Speed;
            if (Origin.y - GameDirector.MenuValue.Menu_Distance > InstObject.transform.localPosition.y)
            {
                InstObject.transform.localPosition = Origin + Vector3.down * GameDirector.MenuValue.Menu_Distance;
            }
            
            yield return null;
        }

        class_In_InstObject(m_Menu_List);
        m_Menu_List += 1;                // 다음 이넘으로 넘어가게 (Menu_Setting -> Menu_Shop -> ...)
        Menu_Count += 1;
        Pre_InstObject_Pos = InstObject.transform.localPosition;

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
        Exist_Menu = new List<GameObject>();
        Reset_Pre_InstObject_Pos = Pre_InstObject_Pos;
    }

    private void Update()
    {
        Menu_Detroy();
    }
}
#endregion