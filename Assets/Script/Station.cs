using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using static Item;
using UnityEngine.UI;

// Station���� ����Ǿ� �÷��̾ �迭�� ������
// ���� ��ġ�� station ���� ���� ground���� number�� number�� �����̼ǿ��� �ڽ�����ã�´� (���߿� ��ġ�� ���� ��ġ�� ����� ���� ���)
public class Grounds
{
    // �� ������ ȹ�� �ߴ°�?
    bool m_IsOn = false;                           
    public bool IsOn { get { return m_IsOn; } }
    //���� ��ġ
    Vector3 m_Position;
    public Vector3 Position { get { return m_Position; } }
    // ���� ����
    int m_Value = 0;                                
    public int Value { get { return m_Value; } }
    // ��ġ�� ������
    Item.Lubricator m_Installed_Lub = null;                
    public Item.Lubricator Installed_Lub { get { return m_Installed_Lub; } }
    // ������  ��ġ - station���� ó��
    internal void Install_Lub(Item.Lubricator _Lubricator) 
    {
        m_Installed_Lub = _Lubricator;
    }
    // ���� ȹ��� ����
    internal void On_Grounds() 
    {
        m_IsOn = true;
    }
    //���� ��ġ ����
    internal void Insert_Ground_Position( Vector3 _Position)
    {
        m_Position= _Position;
    }
    
    public Grounds(int _Value)
    {
        m_Value = _Value;
    }
}


public enum GROUNDS_MAIN_POP_UP_SELLECT
{
    BUY_GROUNDS,
    LUB_SETTING,
    LUB_MOVE,
    ON,
}
#region Grounds Menu Canvas

public enum ON_OFF
{
    ON,
    OFF,
}
# region Lub Inventory

public class Lub_Inventory_Install_Button : MonoBehaviour, IPointerClickHandler
{
    Grounds Sellected_Grounds;
    int Lub_Index;
    void Calculate()
    {
        if (Sellected_Grounds.Installed_Lub != null)
        {
            Lub_Index = Array.FindIndex(Lub_Inventory.Arr_Cur_Lub, x => x == Sellected_Grounds.Installed_Lub);
            Lub_Inventory.Arr_Cur_Lub[Lub_Index].Add_Installed_Amount(-1);
        } // �̹� ��ġ�Ǿ��ִ� ������ ġ��� - Change
        Lub_Index = Array.FindIndex(Lub_Inventory.Arr_Cur_Lub, x => x == Lub_Inventory.Cur_Lub);
        Lub_Inventory.Arr_Cur_Lub[Lub_Index].Add_Installed_Amount(1);
        Sellected_Grounds.Install_Lub(Lub_Inventory.Cur_Lub);
    } // ��ġ�� �� �ϳ� �ø���
    public void OnPointerClick(PointerEventData eventData)
    {
        Sellected_Grounds = PlayerScript.Instance.m_Grounds[Grounds_Menu_Canvas.Sellected_Grounds_Index];
        Calculate();
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.OFF;
    }
}
public class Lub_Inventory_Install_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Lub_Inventory.Set_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.OFF;
    }
}
public enum LUB_INVENTORY_INSTALL_POP_UP
{
    ON,
    OFF,
}
public class Lub_Inventory_Install_Pop_Up : MonoBehaviour
{
    GameObject Cancle_Panel;
    TMP_Text Text; // (������)�� ��ġ�Ͻðڽ��ϱ�?
    GameObject Install_Button;

    void Set_Child()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Inventory_Install_Cancle_Panel>();
        Text = transform.Find("Text").GetComponent<TMP_Text>();
        Install_Button = transform.Find("Install Button").gameObject;
        Install_Button.AddComponent<Lub_Inventory_Install_Button>();
    }
    void Exit_Me()
    {
        if(Lub_Inventory.Lub_Inventory_Pop_Sellect == LUB_INVENTORY_INSTALL_POP_UP.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
    private void OnEnable()
    {
        if (Lub_Inventory.Cur_Lub.Name != null)
        {
            Text.text = Lub_Inventory.Cur_Lub.Name + "��" + "\n" + "��ġ�Ͻðڽ��ϱ�?";
        }
    }
    private void Awake()
    {
        Set_Child();
    }
    
}
public class Lub_Inventory_Button_In_Scroll : MonoBehaviour, IPointerClickHandler
{
    public Lubricator My_Lub;
    public int Number;
    TMP_Text Name;
    TMP_Text Has_Amount;
    TMP_Text Install_Available_Amount;
    
    void Button_Disable()
    {
        if( My_Lub.HasAmount - My_Lub.Installed_Amount <= 0)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
        }
    }
    void Update()
    {
        if(Lub_Inventory.Is_Change == true)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Name.text = My_Lub.Name;
        Has_Amount.text = "���� : " + My_Lub.HasAmount;
        Install_Available_Amount.text = "��ġ���� : " + (My_Lub.HasAmount - My_Lub.Installed_Amount);
        Button_Disable(); // ��ġ���ɼ� 0 �̸� ����Ʈ
    }
    void Awake()
    {
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Has_Amount = transform.Find("Amount").GetComponent<TMP_Text>();
        Install_Available_Amount = transform.Find("Install Available").GetComponent<TMP_Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(My_Lub.HasAmount - My_Lub.Installed_Amount > 0)
        {
            Lub_Inventory.Set_Cur_Lub = My_Lub;
            Lub_Inventory.Set_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.ON;
        }
    }
}
public class Lub_Inventory_Button_In_Main : MonoBehaviour, IPointerClickHandler
{
    public LUBRICATOR_TYPE Lub_Type; //�κ��丮���� add �� �߰�����
    TMP_Text Type;
    void Set_Text()
    {
        switch (Lub_Type)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                Type.text = "�ֹ���";
                break;
            case LUBRICATOR_TYPE.DIESEL:
                Type.text = "����";
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                Type.text = "����";
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                Type.text = "����";
                break;
            case LUBRICATOR_TYPE.BIO:
                Type.text = "���̿�";
                break;
            default:
                break;
        }
    }
    void Awake()
    {
        Type=transform.GetChild(0).GetComponent<TMP_Text>();
    }
    void Start()
    {
        Set_Text(); // Awake ���� ó���ϸ� Lub_Type�� ���־��ֱ����� ������ ����
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Lub_Inventory.Set_Lub_Type = Lub_Type;
        Lub_Inventory.Is_Change = true;
    }
}

public class Lub_Inventory_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.OFF;
    }
}
public class Lub_Inventory : MonoBehaviour
{
    // �� ������ Ÿ��
    static LUBRICATOR_TYPE m_Lub_Type; 
    public static LUBRICATOR_TYPE Lub_Type { get { return m_Lub_Type; } }
    public static LUBRICATOR_TYPE Set_Lub_Type { set { m_Lub_Type = value; } }

    public static bool Is_Change = false; //�ֹ���, ���� .. �������� �޴� ���ϴ� Ʈ����

    //�� ������
    static Lubricator m_Cur_Lub;
    public static Lubricator Cur_Lub { get { return m_Cur_Lub; } }
    public static Lubricator Set_Cur_Lub { set { m_Cur_Lub = value; } }

    //Lub_Inventory_Install_Pop_Up
    static LUB_INVENTORY_INSTALL_POP_UP m_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.OFF;
    public static LUB_INVENTORY_INSTALL_POP_UP Lub_Inventory_Pop_Sellect { get { return m_Lub_Inventory_Pop_Sellect; } }
    public static LUB_INVENTORY_INSTALL_POP_UP Set_Lub_Inventory_Pop_Sellect { set { m_Lub_Inventory_Pop_Sellect = value; } }

    GameObject Main_Button;
    Transform Button_In_Main; // �ֹ��� , ���� ...

    GameObject Content; // ��ũ�Ѻ� ����
    RectTransform Content_Size;
    Transform Button_In_Scroll; // ��� ������, �߱� ������ ...
    static Lubricator[] m_Arr_Cur_Lub;
    public static Lubricator[] Arr_Cur_Lub { get { return m_Arr_Cur_Lub; } }

    GameObject Cancle_Panel;
    GameObject m_Lub_Install_Pop_Up;

    Vector3 Inst_Pos;
    Vector3 Inst_Size;
    float Inst_Distance;
    Transform Inst_Object;

    void Install_Pop_Up()
    {
        if (m_Lub_Inventory_Pop_Sellect == LUB_INVENTORY_INSTALL_POP_UP.ON
            && m_Lub_Install_Pop_Up.activeSelf == false)
        {
            m_Lub_Install_Pop_Up.SetActive(true);
        }
    }
    void Set_Content()
    {
        if (Is_Change == true)
        {
            Is_Change = false;
            Get_Cur_Lub();
            Content.transform.localPosition = Vector3.zero; // ��ũ�� ��ġ �ʱ�ȭ

            Inst_Pos = Button_In_Scroll.localPosition;
            Inst_Size = Button_In_Scroll.localScale;
            Inst_Distance = Button_In_Scroll.GetComponent<RectTransform>().sizeDelta.y;
            for (int i = 0; i < m_Arr_Cur_Lub.Length; i++)
            {
                Inst_Object = GameObject.Instantiate(Button_In_Scroll);
                Inst_Object.SetParent(Content.transform);
                Inst_Object.AddComponent<Lub_Inventory_Button_In_Scroll>().My_Lub = m_Arr_Cur_Lub[i];
                Inst_Object.localPosition = Inst_Pos;
                Inst_Object.localScale = Inst_Size;
                Inst_Pos += Vector3.down * Inst_Distance;
            }
            Content_Size.sizeDelta = new Vector2(0, Inst_Distance * m_Arr_Cur_Lub.Length);
        }
    }
    void Get_Cur_Lub()
    {
        switch (m_Lub_Type)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                m_Arr_Cur_Lub = Item.ArrGasoline;
                break;
            case LUBRICATOR_TYPE.DIESEL:
                m_Arr_Cur_Lub = Item.ArrDissel;
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                m_Arr_Cur_Lub = Item.ArrElectric;
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                m_Arr_Cur_Lub = Item.ArrHydrogen;
                break;
            case LUBRICATOR_TYPE.BIO:
                m_Arr_Cur_Lub = Item.ArrBio;
                break;
            case LUBRICATOR_TYPE.NONE:
                m_Arr_Cur_Lub = null;
                break;
            default:
                break;
        }
    } // ���� ���� ��ư�� ���� ������ ���� �޾ƿ� , �����ִ� Set_Content() ���� ���
    void Set_Main_Button()
    {
        Button_In_Main.AddComponent<Lub_Inventory_Button_In_Main>().Lub_Type = LUBRICATOR_TYPE.GASOLINE;

        Inst_Pos = Button_In_Main.localPosition;
        Inst_Size = Button_In_Main.localScale;
        Inst_Distance = Button_In_Main.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 1; i < (int)LUBRICATOR_TYPE.NONE; i++)
        {
            Inst_Pos += Vector3.right * Inst_Distance;
            Inst_Object = GameObject.Instantiate(Button_In_Main);
            Inst_Object.SetParent(Main_Button.transform);
            Inst_Object.AddComponent<Lub_Inventory_Button_In_Main>().Lub_Type = (LUBRICATOR_TYPE)i;
            Inst_Object.localPosition = Inst_Pos;
            Inst_Object.localScale = Inst_Size;
        }
    }
    void Set_Child()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Inventory_Cancle_Panel>();
        m_Lub_Install_Pop_Up = transform.Find("Lub Install Pop").gameObject;
        m_Lub_Install_Pop_Up.AddComponent<Lub_Inventory_Install_Pop_Up>();
        Main_Button = transform.Find("Main Button").gameObject;
        Button_In_Main = Main_Button.transform.GetChild(0); // ������ �������� ù��° ��ư ������ , �̰ŷ� �����ؼ� ������ ��ư �����
        Content = transform.Find("Scroll View").GetChild(0).GetChild(0).gameObject;
        Content_Size = Content.GetComponent<RectTransform>();
        Button_In_Scroll = Content.transform.GetChild(0); // ������ ����� ���� ù���� ��ũ�� ��ư
    }
    void Exit_Me()
    {
        if(Grounds_Menu_Canvas.Lub_Inventory_Sellect == ON_OFF.OFF)
        {
            m_Lub_Type = LUBRICATOR_TYPE.GASOLINE;
            m_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.OFF;
            Pop_Up_Reset();
            gameObject.SetActive(false);
        }        
    }
    void Pop_Up_Reset()
    {
        m_Lub_Install_Pop_Up.SetActive(false);
    } // ����Ǹ� �˾� ����
    private void Awake()
    {
        Set_Child();
        Set_Main_Button();
    }
    private void LateUpdate()
    {
        Exit_Me();
        Set_Content();
        Install_Pop_Up();
    }
}

#endregion

#region Lub Change Pop Up
public class Lub_Change_Store_Button : MonoBehaviour, IPointerClickHandler
{
    Grounds Sellected_Grounds;
    public void OnPointerClick(PointerEventData eventData)
    {
        Sellected_Grounds = PlayerScript.Instance.m_Grounds[Grounds_Menu_Canvas.Sellected_Grounds_Index]; // ���õ� ���� �ҷ�����
        Sellected_Grounds.Installed_Lub.Add_Installed_Amount(-1); // �� �������� ��ġ�Ǿ��ִ� ���� �ϳ� ����
        Sellected_Grounds.Install_Lub(null); //������ ��ġ�� ������ �������� �ٲٰ�
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF; //�˾��ݰ�
    }
}

public class Lub_Change_Move_button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE;
        Grounds_Menu_Canvas.Set_Move_Grounds_Index = Grounds_Menu_Canvas.Sellected_Grounds_Index;
        Buy_Station.Reset_Grounds(); // Buy_Station �����̴°� �ʱ�ȭ
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF;
    }
}

public class Lub_Change_Change_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF;
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.ON;
        Lub_Inventory.Is_Change = true;
    }
}

public class Lub_Change_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF;
        
    }
}

public class Lub_Change_Pop_Up : MonoBehaviour
{
    GameObject Cancle_Panel;
    GameObject Change_Button;
    GameObject Move_button;
    GameObject Store_Button;
    void Exit_Me()
    {
        if (Grounds_Menu_Canvas.Lub_Change_Sellect == ON_OFF.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    void Child_Setting()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Change_Cancle_Panel>();
        Change_Button = transform.Find("Change Button").gameObject;
        Change_Button.AddComponent<Lub_Change_Change_Button>();
        Move_button = transform.Find("Move Button").gameObject;
        Move_button.AddComponent<Lub_Change_Move_button>();
        Store_Button = transform.Find("Store Button").gameObject;
        Store_Button.AddComponent<Lub_Change_Store_Button>();
    }

    private void Awake()
    {
        Exit_Me();
        Child_Setting();
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
}

#endregion

#region Lub Install Pop Up
public class Lub_Install_Install_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Install_Sellect = ON_OFF.OFF;
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.ON;
        Lub_Inventory.Is_Change = true;
    }
}
public class Lub_Install_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Install_Sellect = ON_OFF.OFF;
    }
}
public class Lub_Install_Pop_Up : MonoBehaviour
{
    GameObject Cancle_Panel;
    GameObject Install_Button;

    void Exit_Me()
    {
        if(Grounds_Menu_Canvas.Lub_Install_Sellect == ON_OFF.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    void Child_Setting()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Install_Cancle_Panel>();
        Install_Button= transform.Find("Install Button").gameObject;
        Install_Button.AddComponent<Lub_Install_Install_Button>();
    }

    private void Awake()
    {
        Exit_Me();
        Child_Setting();
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
}
#endregion

#region Buy Grounds Pop Up
public class Buy_Grounds_Buy_Button : MonoBehaviour, IPointerClickHandler
{
    TMP_Text Value;
    Grounds Sellected_Grounds; 
    
    void Calculate()
    {
        Sellected_Grounds.On_Grounds();
        PlayerScript.Instance.m_Money -= Sellected_Grounds.Value;
        Debug.Log(PlayerScript.Instance.m_Money);
    }
    void OnEnable()
    {
        Value = transform.GetChild(0).GetComponent<TMP_Text>();
        if (PlayerScript.Instance.m_Grounds == null)
        {
            Debug.Log("���� ��������");
        }
        Sellected_Grounds = PlayerScript.Instance.m_Grounds[Grounds_Menu_Canvas.Sellected_Grounds_Index];
        Value.text = Sellected_Grounds.Value.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Calculate();
        Grounds_Menu_Canvas.Set_Buy_Grounds_Sellect = ON_OFF.OFF;
    }
}
public class Buy_Grounds_Cancle_Panel : MonoBehaviour ,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Buy_Grounds_Sellect = ON_OFF.OFF;
    }

    
}
public class Buy_Grounds_Pop_Up : MonoBehaviour
{
    GameObject m_Cancle_Panel;
    GameObject m_Buy_Button;
    TMP_Text m_Gold_Text;
    public static GameObject Complete_Pop_Up;

    void Exit_Me()
    {
        if (Grounds_Menu_Canvas.Buy_Grounds_Sellect == ON_OFF.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    void Insert_Component_In_Child()
    {
        m_Cancle_Panel.AddComponent<Buy_Grounds_Cancle_Panel>();
        m_Buy_Button.AddComponent<Buy_Grounds_Buy_Button>();
    }
    void Get_Child()
    {
        m_Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        m_Buy_Button = transform.Find("Buy Button").gameObject;
        m_Gold_Text = transform.Find("Gold Text").gameObject.GetComponent<TMP_Text>();
    }
    private void Awake()
    {
        Get_Child();
        Insert_Component_In_Child();
        
    }
    private void OnEnable()
    {
        m_Gold_Text.text = "�����ݾ� : " + PlayerScript.Instance.m_Money;
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
}

#endregion //Buy Grounds Pop Up ������ �����Ͻðڽ��ϱ�?

//�������� ������ ���� ���� ���� �ϴ� ��ư
public class Grounds_Main_Change_Button : MonoBehaviour, IPointerClickHandler
{
    TMP_Text Text;

    void Set_Text()
    {
        switch (Grounds_Menu_Canvas.Ground_Menu_Sellect)
        {
            case GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS:
                Text.text = "������ ����";
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING:
                Text.text = "���� ����";
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE:
                Text.text = "�̵� ���";
                break;
            default:
                break;
        }
    } // ���������� ��� ������ �������� 

    void Disable_Me()
    {
        if(Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.ON)
        {
            gameObject.SetActive(false);
        }
    }
    void LateUpdate()
    {
        Set_Text();
        Disable_Me();
    }
    void OnEnable()
    {
        Set_Text();
    }
    void Awake()
    {
        Text = transform.GetChild(0).GetComponent<TMP_Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Buy_Station.Reset_Grounds();
        switch (Grounds_Menu_Canvas.Ground_Menu_Sellect)
        {
            case GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS:
                Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING;
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING:
                Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS;
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE:
                Grounds_Menu_Canvas.Set_Move_Grounds_Index = -1;
                Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING;
                break;
            default:
                break;
        }
    }
}

//���� ���� , ������ ����
#region Grounds Main Pop Up

public class Lub_Setting_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING;
    }
}
public class Buy_Grounds_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS;
    }
}
public class Grounds_Menu_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.IsReset= true;
    }
}
public class Grounds_Main_Pop_Up : MonoBehaviour
{
    GameObject m_Lub_Setting; // ������ ���� ��ư
    GameObject m_Buy_Grounds; // ���� ���� ��ư
    GameObject m_Cancle_Panel; // ����� ������ ���

    void Exit_Me()
    {
        if (Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS
            || Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING)
        {
            gameObject.SetActive(false);
        }
    }
    void AddComponent_In_Child()
    {
        m_Cancle_Panel.AddComponent<Grounds_Menu_Cancle_Panel>();
        m_Buy_Grounds.AddComponent<Buy_Grounds_Button>();
        m_Lub_Setting.AddComponent<Lub_Setting_Button>();
    }
    void Find_Child()
    {
        m_Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        m_Buy_Grounds = transform.Find("Buy Grounds").gameObject;
        m_Lub_Setting = transform.Find("Lub Setting").gameObject;
    }
    private void Awake()
    {
        Exit_Me();
        Find_Child();
        AddComponent_In_Child();
    }

    private void LateUpdate()
    {
        Exit_Me(); // ���õǸ� �˾� ����
    }
}

#endregion  //Grounds Main Pop Up

public class Grounds_Menu_Exit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.IsReset = true;
    }
}

// ���� �޴�
public class Grounds_Menu_Canvas : MonoBehaviour
{
    //Lub_Inventory
    static ON_OFF m_Lub_Inventory_Sellect = ON_OFF.OFF;
    public static ON_OFF Lub_Inventory_Sellect { get { return m_Lub_Inventory_Sellect; } }
    public static ON_OFF Set_Lub_Inventory_Sellect { set { m_Lub_Inventory_Sellect = value; } }
    
    //Lub_Change_Pop_Up
    static ON_OFF m_Lub_Change_Sellect = ON_OFF.OFF;
    public static ON_OFF Lub_Change_Sellect { get { return m_Lub_Change_Sellect; } }
    public static ON_OFF Set_Lub_Change_Sellect { set { m_Lub_Change_Sellect= value; } }

    static int m_Move_Grounds_Index = -1; //Move_Grounds ���� ���
    public static int Move_Grounds_Index { get { return m_Move_Grounds_Index; } }
    public static int Set_Move_Grounds_Index { set { m_Move_Grounds_Index= value; } }

    //Lub_Install_Pop_UP
    static ON_OFF m_Lub_Install_Sellect = ON_OFF.OFF;
    public static ON_OFF Lub_Install_Sellect { get { return m_Lub_Install_Sellect; } }
    public static ON_OFF Set_Lub_Install_Sellect { set { m_Lub_Install_Sellect = value; } }

    //Buy_Grounds_Pop_Up
    static ON_OFF m_Buy_Grounds_Sellect = ON_OFF.OFF;
    public static ON_OFF Buy_Grounds_Sellect { get { return m_Buy_Grounds_Sellect; } }
    public static ON_OFF Set_Buy_Grounds_Sellect { set { m_Buy_Grounds_Sellect = value; } }

    //���� ���� - Buy_Station Ŭ����
    static bool m_Is_Input = false; //�Է��� �־��°�?
    public static bool Is_Input { get { return m_Is_Input; } }
    static int m_Sellected_Grounds_Index = -1; // ���õ� ������ �ε���
    public static int Sellected_Grounds_Index { get { return m_Sellected_Grounds_Index; } }

    // ��������, �����⼳�� Change ��ư - Grounds_Main_Change_Button

    // ��������, �����⼳�� - Main_Pop_Up
    static GROUNDS_MAIN_POP_UP_SELLECT m_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.ON;
    public static GROUNDS_MAIN_POP_UP_SELLECT Ground_Menu_Sellect { get { return m_Ground_Menu_Sellect; } }
    public static GROUNDS_MAIN_POP_UP_SELLECT Set_Ground_Menu_Sellect { set { m_Ground_Menu_Sellect = value; } }
    

    public static bool IsReset; // �������� ������ ���� enum static �ʱ�ȭ


    GameObject m_Main_Pop_Up;
    GameObject m_Main_Change_Button;
    GameObject m_Buy_Grounds_Pop_Up;
    GameObject m_Lub_Install_Pop_Up;
    GameObject m_Lub_Change_Pop_Up;
    GameObject m_Exit; // Main_Pop_Up ������ �ȶ߰� �����ĺ��� �� (��, �޴���ư �Ⱥ��̰� �� ��������)
    GameObject m_Lub_Inventory;

    void Lub_Inventory()
    {
        if (m_Lub_Inventory_Sellect == ON_OFF.ON
            && m_Lub_Inventory.activeSelf == false)
        {
            m_Lub_Inventory.SetActive(true);
        }
    }

    void Lub_Move()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE
            && m_Is_Input == true)
        {
            m_Is_Input = false;
            if (Move_Grounds_Index != -1)
            {
                PlayerScript.Instance.m_Grounds[Sellected_Grounds_Index].Install_Lub(
                    PlayerScript.Instance.m_Grounds[Move_Grounds_Index].Installed_Lub); //������ ���� ������ �߰��ϰ�
                PlayerScript.Instance.m_Grounds[Move_Grounds_Index].Install_Lub(null); //���� ���� ������ ���ְ�
                Buy_Station.Reset_Grounds();//Buy_Station ȭ��ǥ�� ���� (�����̴°� ���ϼ��� ����)
                m_Move_Grounds_Index = -1; // ���� �ʱ�ȭ
                m_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING; // �̵� �Ϸ������� ������ �������� �ǵ�����
            } // Move�� ��쿡�� ���ý� �����⸦ �̵���Ű�� �˾��� ��������
        }
    }
    void Lub_Install_Or_Change()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING
            && m_Is_Input == true)
        {
            m_Is_Input = false;
            if (PlayerScript.Instance.m_Grounds[Sellected_Grounds_Index].Installed_Lub == null)
            {
                m_Lub_Install_Sellect = ON_OFF.ON;
                m_Lub_Install_Pop_Up.SetActive(true);
            }
            else
            {
                m_Lub_Change_Sellect = ON_OFF.ON;
                m_Lub_Change_Pop_Up.SetActive(true);  
            }
            
            
        }
    }
    void Grounds_Buy_Pop_Up()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS
            && m_Is_Input == true)
        {
            m_Is_Input = false;
            m_Buy_Grounds_Sellect = ON_OFF.ON;
            m_Buy_Grounds_Pop_Up.SetActive(true);
        }
    }//�������� On - ���ο��� �������� ������ , ����  Ŭ���ǰ� ,  BUY_GROUNDS_POP_UP.ON �̸� (��ũ��Ʈ ������ ����)

    // ���� �����̴°� ����
    #region Sellect Actived Grounds
    
    string Number_Extract;
    Ray ray;
    RaycastHit hit;
    void Grounds_Sellect()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ȭ�� ��ġ�ϴ� ��ġ ����.

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                /* 1. ������ ĭ ���ڸ� Ŭ���� ���� �����ϰ� ��.
                 * 2. ������ ���ڵ��� ���տ��� ������� �ؾ���. �̰� ������� ������ ���������� �߿��ѵ�. ��ǻ� ����� Ŭ���ߴµ�
                 *      ������� �ȵǴϱ�.
                 * 3. ������ ���ڵ��� ����� �� �� �ڸ��� �����Ⱑ �߰��ؾ��� 
                    4. case�� �׸��� �������� �߰��ϵ� case���� ����� �׸����� 2���� �����ҵ�.
                 */
                if (hit.transform.gameObject.GetComponent<Animator>() != null)
                {
                    if(EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        Number_Extract = Regex.Replace(hit.transform.gameObject.name, @"\D", ""); // ���ӿ�����Ʈ���� ���ڸ� ����
                        m_Sellected_Grounds_Index = int.Parse(Number_Extract);
                        m_Is_Input = true;
                    }
                    
                }

            }
        }
    }
    #endregion  // ���� �����̴°� ����
    
    void Main_Change_Button()
    {
        if(m_Ground_Menu_Sellect != GROUNDS_MAIN_POP_UP_SELLECT.ON
            && m_Main_Change_Button.activeSelf == false)
        {
            m_Main_Change_Button.SetActive(true);
        }
    }
    void Main_Pop_Up()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.ON
            && m_Main_Pop_Up.activeSelf == false)
        {
            m_Main_Pop_Up.SetActive(true);
        }
    } //�����˾� On - GROUNDS_MAIN_POP_UP_SELLECT.ON �̸� (MenuScipt - Menu_Grounds ���� ����)  

    void Main_Canvas_And_Exit_OnOff()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.ON 
            && Station.Instance.Main_Canvas.activeSelf == false)
        {
            Station.Instance.Main_Canvas.SetActive(true);
            m_Exit.SetActive(false);
        }
        
        else if(m_Ground_Menu_Sellect != GROUNDS_MAIN_POP_UP_SELLECT.ON
            && Station.Instance.Main_Canvas.activeSelf == true)
        {
            Station.Instance.Main_Canvas.SetActive(false);
            m_Exit.SetActive(true);
        }
    } // �������ų� ������ ��ġ�Ҷ� ���� ĵ���� ��� ���� ������ ��ư ��

    void Insert_Component_In_Child()
    {
        m_Main_Pop_Up = transform.Find("Main Pop Up").gameObject;
        m_Main_Pop_Up.AddComponent<Grounds_Main_Pop_Up>();
        m_Main_Change_Button = transform.Find("Change Button").gameObject;
        m_Main_Change_Button.AddComponent<Grounds_Main_Change_Button>();
        m_Buy_Grounds_Pop_Up = transform.Find("Buy Grounds Pop Up").gameObject;
        m_Buy_Grounds_Pop_Up.AddComponent<Buy_Grounds_Pop_Up>();
        m_Lub_Install_Pop_Up = transform.Find("Lub Install Pop Up").gameObject;
        m_Lub_Install_Pop_Up.AddComponent<Lub_Install_Pop_Up>();
        m_Lub_Change_Pop_Up = transform.Find("Lub Change Pop Up").gameObject;
        m_Lub_Change_Pop_Up.AddComponent<Lub_Change_Pop_Up>();
        m_Exit = transform.Find("Exit").gameObject;
        m_Exit.AddComponent<Grounds_Menu_Exit>();
        m_Lub_Inventory = transform.Find("Lub Inventory").gameObject;
        m_Lub_Inventory.AddComponent<Lub_Inventory>();
    } //�ڽĵ� ������Ʈ �ֱ�
    

    void Reset_All_Pop_Up()
    {
        m_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.ON;
        m_Buy_Grounds_Sellect = ON_OFF.OFF;
        m_Lub_Install_Sellect = ON_OFF.OFF;
        m_Lub_Change_Sellect = ON_OFF.OFF; 
        m_Sellected_Grounds_Index = -1;
        m_Move_Grounds_Index = -1;
        Main_Canvas_And_Exit_OnOff(); // ����ĵ����(��, �޴���ư) �Ѱ� �������ư ���ְ�
    } // �������� ������ ���� enum static �ʱ�ȭ
    void Reset_And_Exit()
    {
        if (IsReset == true)
        {
            IsReset = false;
            Reset_All_Pop_Up();
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        Insert_Component_In_Child();
        Reset_All_Pop_Up();
        m_Exit.SetActive(false); // Reset ������ ���� ĵ������ TRUE�� ���¶� ������ �ȵ�
    }
    private void Update()
    {
        Main_Pop_Up();
    }
    private void LateUpdate()
    {
        Main_Change_Button();
        Main_Canvas_And_Exit_OnOff(); // ���������Ҷ� ����ĵ����(�� �޴���ư ��) ���صǼ� ���� ������ ��ư �Ѵ� ���
        Grounds_Sellect(); // ��������
        Grounds_Buy_Pop_Up(); //������ �����Ͻðٽ��ϱ�?
        Lub_Install_Or_Change(); // ������ ������ ���� ��ġ or ���� �˾�
            Lub_Move();
        Lub_Inventory(); // ������ �κ��丮 ����
        Reset_And_Exit();
    }
}

#endregion //Grounds_Menu_Canvas ����

// ������ ���⼭ ������ ����� ���� �ִ� Grounds_Menu_Canvas ���� �Ѵ�
public class Buy_Station : MonoBehaviour
{
    static GameObject[] Arr_Buy_Area;

    public static void Reset_Grounds()
    {
        for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
        {
            Arr_Buy_Area[i].SetActive(false);
        }
    }
    void Just_Available_Grounds()
    {
        if (Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS)
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (PlayerScript.Instance.m_Grounds[i].IsOn == false
                    && Arr_Buy_Area[i].activeSelf == false)
                {
                    Arr_Buy_Area[i].SetActive(true);
                }
                else if(PlayerScript.Instance.m_Grounds[i].IsOn == true
                    && Arr_Buy_Area[i].activeSelf == true)
                {
                    Arr_Buy_Area[i].SetActive(false);
                }
            }
        }
        else if(Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING)
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (PlayerScript.Instance.m_Grounds[i].IsOn == true
                    && Arr_Buy_Area[i].activeSelf == false)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(true);
                }
                else if (PlayerScript.Instance.m_Grounds[i].IsOn == false
                    && Arr_Buy_Area[i].activeSelf == true)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(false);
                }
            }
        }
        else if(Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE)
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (PlayerScript.Instance.m_Grounds[i].IsOn ==true
                    && PlayerScript.Instance.m_Grounds[i].Installed_Lub == null
                    && Arr_Buy_Area[i].activeSelf == false)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (Arr_Buy_Area[i].gameObject.activeSelf == true)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(false);
                }
            }
        }

    } // Grounds_Main_Pop_Up ���ÿ� ���� Ŭ�� ������ ������ Ȱ��ȭ
    void Get_Child()
    {
        Arr_Buy_Area = new GameObject[transform.childCount];
        for (int i = 0; i < Arr_Buy_Area.Length; i++)
        {
            Arr_Buy_Area[i] = transform.GetChild(i).gameObject;
        }
        if (Arr_Buy_Area.Length != PlayerScript.Instance.m_Grounds.Length)
        {
            Debug.LogError("if (Arr_Buy_Area.Length != PlayerScript.Instance.m_Grounds.Length) ���� ���� ������ �÷��̾ �Էµ� ���� ������ �ٸ��� (�÷��̾ ������ �����ƴ� false�� ����)");
        }
    } // Ȱ��ȭ �Ǿ��ִ� �ڽĵ鸸 ������ (���� ������Ʈ Ȱ��ȭ �ʼ�)

    private void Start()
    {
        Get_Child(); // PlayerScript�� Station ���Ŀ� Awake �Ǳ� ������
                     // �� �Լ��� Awake ���� ó���ϸ� null ���� �߻�
    }

    private void Update()
    {
        Just_Available_Grounds();
        
    }
}

public class Main_Station : MonoBehaviour
{
    Transform[] Children;

    void Station_Activate()
    {
        for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
        {
            if (PlayerScript.Instance.m_Grounds[i].IsOn == true
                && Children[i].gameObject.activeSelf == false)
            {
                Children[i].gameObject.SetActive(true);
            }
        }
       
    }
    void Insert_Ground_Position()
    {
        if (Children.Length != Station.Grounds.Length)
        {
            Debug.LogError("        if(Children.Length != Station.Grounds.Length)\r\n");
        }
        for (int i = 0; i < Station.Grounds.Length; i++)
        {
            Station.Grounds[i].Insert_Ground_Position(Children[i].position);
        }
    } // ������ ������Ʈ ��ġ�� m_Groonds �� ������ Player�� Awake �� ������

    void Get_Station()
    {
        Children = new Transform[transform.childCount];
        for (int i = 0; i < Children.Length; i++)
        {
            Children[i] = transform.GetChild(i);
        }
    } // Station(=�ڽ�) ������

    private void Update()
    {
        Station_Activate(); // ���� ���۶� Ȱ��ȭ �ϰ�, ���� �߿��� ������ �����ϸ� ���� Ȱ��ȭ
    }
    private void Awake()
    {
        Get_Station();
        Insert_Ground_Position();
    }
}
public class Station : MonoBehaviour
{
    private static Station _instance;  //�̱���
    public static Station Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(Station)) as Station;

                if (_instance == null)
                {
                    Debug.Log("�̱��� ������Ʈ ����");
                }
            }

            return _instance;
        }
    }  //�̱���
    void SingleTone()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    } // �̱���

    
    public GameObject m_Grounds_Menu_Canvas; // ������Ʈ �������� ���� 
    public GameObject Main_Canvas; //���� ����ȭ�鿡�� �޴���ư ������ ���� 

    public GameObject m_Buy_Station; // ������ ��ġ ĭ ����(�θ� ������Ʈ)
    public GameObject m_Main_Station; // ������ ��ġ �ǰ� ���� ���̴� ĭ (�θ� ������Ʈ)
    static Grounds[] m_Grounds; // ���� ����
    public static Grounds[] Grounds { get { return m_Grounds; } } // �÷��̾�, Buy_Station���� ���� ���� ����

    

    void Setting_Grounds()
    {
        m_Grounds = new Grounds[]
        {
            new Grounds(1000), //0�� ����
            new Grounds(2000), //1
            new Grounds(3000), //2
            new Grounds(4000), //3
            new Grounds(5000), //4
            new Grounds(6000), //5
            new Grounds(7000), //6
            new Grounds(8000), //7
            new Grounds(9000), //8
        };
    }

    void Child_Setting()
    {
        m_Buy_Station.AddComponent<Buy_Station>();
        m_Main_Station.AddComponent<Main_Station>();
        m_Grounds_Menu_Canvas.AddComponent<Grounds_Menu_Canvas>();
    } //�������� �޾ƿͼ� �־���

    private void Awake()
    {
        SingleTone();
        Setting_Grounds(); //���� ����
        Child_Setting();
    }
}
