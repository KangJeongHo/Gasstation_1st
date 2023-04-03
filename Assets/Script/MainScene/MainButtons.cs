using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtons : MonoBehaviour
{

    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _shopCanvas;
    [SerializeField] private GameObject _mainCamera;

    private float _smoothTime = 0.3f;
    public Vector2 targetPosition;
    private Vector2 _currentVelocity;
    private Vector2 _currentPosition;
    private float maxSpeed;
    
    public void ShopMenuOpen()
    {
        if (_mainCanvas.activeSelf == true)
        { 
            _mainCanvas.SetActive(false);
            Debug.Log("if구문 작동함. 첫번째꺼");
        }

        _currentVelocity = 
        _currentPosition = new Vector2(0f, 8f);
        _mainCamera.transform.position = Vector2.SmoothDamp(_currentPosition, targetPosition, ref _currentVelocity, _smoothTime, maxSpeed = Mathf.Infinity);
        
        if (_shopCanvas.activeSelf == false)
        {
            _shopCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
            Debug.Log("if구문 작동함. 두번째꺼");
        }
        
        Debug.Log("상점 캔버스 켜짐 : "+_shopCanvas.activeSelf);
        Debug.Log("메인캔버스 꺼짐 : "+_mainCanvas.activeSelf);
        //1. 메인 캔버스 꺼진다.
        //2. 화면이 하늘로 날아간다
        //3. 상점 캔버스가 켜진다.
        //4. x표시를 누르면 상점 캔버스가 꺼진다.
        //5. 화면이 아래로 내려온다.
        //6. 동시에 메인 캔버스가 켜진다.
    }

    public void ShopMenuClose()
    {
        _mainCanvas.SetActive(true);
        _mainCamera.transform.position = new Vector2(0, 0);
        _shopCanvas.SetActive(false);
    }
}