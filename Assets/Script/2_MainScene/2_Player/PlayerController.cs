using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    #region 싱글톤

    private static PlayerController _instance;

    public static PlayerController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PlayerController)) as PlayerController;

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