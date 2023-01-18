using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string lubName = string.Empty;
    public float lubChagerSpeed;
    public string lubGas = string.Empty;

    // 게임 규칙 및 정보 담김
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private static GameManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            {
                Destroy(this.gameObject);
            }
        }
    }
        /*
         *주유기 세부사항
         * 주유기 이름
         * 주유기 충전 속도
         * 기름 종류
         * 특수능력(?)
         *
         * 
         */
        //namespace = 여러가지 클래스들을 하나로 묶어주는 역할.
        
        //public Dictionary<string, List<string>> lubDatas;
        void Dictionary()
        {
            var lubDatas = new Dictionary<string, Dictionary<string, string>>();
            
            var lub1 = new Dictionary<string, string>();
            lub1.Add("주유소 이름","주유소1");
            lub1.Add("기름 종류","경유");
            lub1.Add("주유 충전 속도","1");
            lub1.Add("주유소 그림","lub_image1");
            
            var lub2 = new Dictionary<string, string>();
            lub2.Add("주유소 이름","주유소2");
            lub2.Add("기름 종류","경유");
            lub2.Add("주유 충전 속도","2");
            lub1.Add("주유소 그림","lub_image2");
            
            var lub3 = new Dictionary<string, string>();
            lub3.Add("주유소 이름","주유소3");
            lub3.Add("기름 종류","경유");
            lub3.Add("주유 충전 속도","3");
            lub1.Add("주유소 그림","lub_image3");
            
            lubDatas.Add("주유기", lub1);
            lubDatas.Add("주유기", lub2);
            lubDatas.Add("주유기", lub3);

        }
   
        
}
// 딕셔너리 사용법1 : https://engineer-mole.tistory.com/174
// 네임스페이스 설명 : https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-C-%EA%B0%95%EC%A2%8C-12-%EB%84%A4%EC%9E%84%EC%8A%A4%ED%8E%98%EC%9D%B4%EC%8A%A4Namespaces-using