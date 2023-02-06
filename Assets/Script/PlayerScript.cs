using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerScript : MonoBehaviour
{
    private static PlayerScript _instance;  //�̱���
    public static PlayerScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;

                if (_instance == null)
                {
                    Debug.Log("�̱��� ������Ʈ ����");
                }
            }

            return _instance;
        }
    }  //�̱���

    // ���� ��
    public double m_Money = 1000000;
    [HideInInspector]
    public Grounds[] m_Grounds = null;

    
    // Station ���� ���� ���� ������
    void Get_Grounds_Info_From_Station()
    {
        m_Grounds = new Grounds[Station.Grounds.Length];
        m_Grounds = Station.Grounds;
    }
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
    private void Awake()
    {
        SingleTone();
        Get_Grounds_Info_From_Station();
    }
}
