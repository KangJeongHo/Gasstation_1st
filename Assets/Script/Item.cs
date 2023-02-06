using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using static Item;


public class Item : MonoBehaviour
{
    

    // ��ӽ� �����ڿ� base �־��־�� ��
    public class Item_Info
    {
        //�������̸�
        string m_Name;
        public string Name { get { return m_Name; } }
        //�ǸŰ���
        int m_Value;
        public int Value { get { return m_Value; } }
        //�⺻ �Ǹ� ����
        int m_SellAmount;
        public int SellAmount { get { return m_SellAmount; } }
        //������ �ִ� ����
        int m_HasAmount;
        public int HasAmount { get { return m_HasAmount; } }

        public void AddAmount(int _Amount_Number)
        {
            m_HasAmount += _Amount_Number;
        }

        public Item_Info(string _Name, int _Value, int _Amount) 
        {
            m_Name = _Name;
            m_Value= _Value;
            m_SellAmount = _Amount;
        }
    }

    #region Lubricator
    public class Lubricator : Item_Info
    {
        int m_Installed_Amount;
        public int Installed_Amount { get { return m_Installed_Amount; } }

        public void Add_Installed_Amount(int _Amount_Number)
        {
            m_Installed_Amount += _Amount_Number;
        }
        public Lubricator(string __Name, int __Value, int __SellAmount) : base(__Name, __Value, __SellAmount)
        {
        }
    }

    public static Lubricator[] ArrGasoline = null;
    public static Lubricator[] ArrDissel = null;
    public static Lubricator[] ArrElectric = null;
    public static Lubricator[] ArrHydrogen = null;
    public static Lubricator[] ArrBio = null;
    void Item_Gasoline()
    {
        ArrGasoline = new Lubricator[]
        {
                new Lubricator("���ָ�", 500, 50),
                new Lubricator("���ָ�2", 1500, 50),
                new Lubricator("���ָ�3", 2500, 50),
                new Lubricator("���ָ�4", 3500, 50),
                new Lubricator("���ָ�5", 4500, 50),
                new Lubricator( "���ָ�6", 5500, 50),
                new Lubricator( "���ָ�7", 6500, 50),
                new Lubricator( "���ָ�8", 7500, 50),
                new Lubricator( "���ָ�9", 8500, 50),
                new Lubricator( "���ָ�10", 9500, 50),
        };
    }

    void Item_ArrDissel()
    {
        ArrDissel = new Lubricator[]
        {
                new Lubricator("����", 500, 20),
                new Lubricator("����2", 1500, 50),
                new Lubricator("����3", 2500, 40),
                new Lubricator("����4", 3500, 50),
                new Lubricator("����5", 4500, 10),
        };
    }
    void Item_ArrElectric()
    {
        ArrElectric = new Lubricator[]
        {
                new Lubricator("�����", 500, 20),
        };
    }
    void Item_ArrHydrogen()
    {
        ArrHydrogen = new Lubricator[]
        {
                new Lubricator("���Ҵ� ���Ҽ���", 500, 20),
        };
    }
    void Item_ArrBio()
    {
        ArrBio = new Lubricator[] 
        { 
            new Lubricator("���̿���", 500, 20) 
        };
    }


    #endregion
    private void Awake()
    {
        Item_Gasoline();
        Item_ArrDissel();
        Item_ArrElectric();
        Item_ArrHydrogen();
        Item_ArrBio();
    }
    
}
