using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    private float _roadSpeed = 0.1f;
    private float _skySpeed = -0.01f;
    [SerializeField] private Material sky;
    [SerializeField] private Material road;
    [SerializeField] private GameObject startText;
    private bool _touchCheck = false;
    //private bool _speedCheck = false;


    void Start()
    {
        StartCoroutine(StartText());
    }

    void Update()
    {
        var moveoffsetSky = sky.mainTextureOffset.x + _skySpeed * Time.deltaTime;
        Vector2 movedoffsetsky = new Vector2(moveoffsetSky, 0);
        var moveoffsetroad = road.mainTextureOffset.x + _roadSpeed * Time.deltaTime;
        Vector2 movedoffsetroad = new Vector2(moveoffsetroad, 0);

        sky.mainTextureOffset = movedoffsetsky;
        road.mainTextureOffset = movedoffsetroad;
        if (Input.GetMouseButton(0))
        {
            _touchCheck = true;
        }
    }

    IEnumerator StartText()
    {
        while (_touchCheck == false)
        {
            startText.SetActive(true);
            yield return new WaitForSeconds(1f);
            startText.SetActive(false);
            yield return new WaitForSeconds(1f);
        }

        if (_touchCheck == true)
        {
            Debug.Log("화면 클릭 됨.");
            startText.SetActive(true);

            StartCoroutine(ReadyToSlow());
        }
    }

    IEnumerator ReadyToSlow()
    {
        Debug.Log("레디투 슬로우 코루틴 입장.");
        Debug.Log("for 문 전에 하늘의 속도 : " + _skySpeed + "  /  현재 땅 속도 : " + _roadSpeed);
        
        var lubAnima = GameObject.Find("lub").GetComponent<Animator>();
        lubAnima.SetTrigger("TouchCheck");
        
        for (int a = 0; a < 5; a++)
        {
            _skySpeed += 0.002f;
            _roadSpeed -= 0.02f;
            Debug.Log("헌재 하늘의 속도 : " + _skySpeed + "  /  현재 땅 속도 : " + _roadSpeed);
            yield return new WaitForSeconds(0.7f);
        
        }
        _skySpeed = 0;
        _roadSpeed = 0;
        yield return new WaitForSeconds(0.1f);

        var FadeOutImage = GameObject.Find("FadeOut").GetComponent<Image>();
        var changeimage = FadeOutImage.color;
        while (changeimage.a <= 1)
        {
            changeimage.a += Time.deltaTime;
            FadeOutImage.color = changeimage;
            Debug.Log(FadeOutImage.color.a);
            yield return new WaitForSeconds(0.001f);
        }

        LoadSceneController.LoadScene("Ingame");
    }
}