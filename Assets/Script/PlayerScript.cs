using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerScript : MonoBehaviour
{
    private static PlayerScript _instance;  //ΩÃ±€≈Ê
    public static PlayerScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;

                if (_instance == null)
                {
                    Debug.Log("ΩÃ±€≈Ê ø¿∫Í¡ß∆Æ æ¯¿Ω");
                }
            }

            return _instance;
        }
    }  //ΩÃ±€≈Ê

    // ∞°¡¯ µ∑
    public double m_Money = 1000000;
    [HideInInspector]
    public Grounds[] m_Grounds = null;

    
    // Station ø°º≠ ∫Œ¡ˆ ¡§∫∏ ∞°¡Æø»
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
    } // ΩÃ±€≈Ê
    private void Awake()
    {
        SingleTone();
        Get_Grounds_Info_From_Station();
    }
}
