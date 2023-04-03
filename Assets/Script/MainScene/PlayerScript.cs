using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlayerScript : MonoBehaviour
{
    #region 싱글톤

    private static PlayerScript _instance;

    public static PlayerScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 오브젝트 없음");
                }
            }

            return _instance;
        }
    }

    private void Awake()
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
    }
    #endregion
    
    
    
    
}