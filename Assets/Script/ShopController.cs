using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
// 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본
public class ShopController : MonoBehaviour
{
    public GameObject buy_stations; // 주유기 설치 칸 보기(부모 오브젝트)
    public GameObject main_stations; // 주유기 설치 되고 나면 보이는 칸 (부모 오브젝트)
    public GameObject normal_lub; // 주유기 프리팹 소환하기 위한 오브젝트 선언
    public Transform lub_cub_0_transform; // 주유기 설치를 위한 부모 찾기? 왜 main_stations와 중첩되는 것 같음.
    public GameObject lub_cub_0;
    public Text moneytext;  //Money 오브젝트에 들어있는 텍스트값을 불러오기 위해 쓴 함수.
    public string moneystring; // 머니 오브젝트에 들은 값을 스트링 함수로 쓸수 있게 담을 그릇.
    private string money; // moneystring에서 가져온 값을 글자 다 빼버리기 위한 그릇.
    private double doublemoney; // 다 뺴버린 값인 money 함수의 내용을 담기 위한 그릇.
    void Start()
    {
        doublemoney = 0f;
        money = String.Empty;
        moneystring = moneytext.text.ToString();
        money = Regex.Replace(moneystring, @"\D","");
        doublemoney = double.Parse(money);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 화면 터치하는 위치 저장.
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                /* 1. 주유소 칸 상자를 클릭한 값을 저장하게 함.
                 * 1-1. 주유소 칸 상자를 나중에 어디 값에다가 카운드해서 저장해야겠음.
                 *      ex: lub.buy.1 ++; , lubname.text = "기본주유기"
                 * 2. 주유소 상자들을 눈앞에서 사라지게 해야함. 이게 사라지는 순서가 언제인지가 중요한듯. 사실상 빈공간 클릭했는데
                 *      사라지면 안되니깐.
                 * 3. 주유소 상자들이 사라진 후 그 자리에 주유기가 추가해야함 
                    4. case로 항목을 선택지로 추가하되 case전에 공통된 항목으로 2번은 가능할듯.
                 */
                if (hit.transform.gameObject.name == "buy area (0)") // 주유기 첫번째 칸을 클릭했을때
                {
                    // GameObject myInstance = Instantiate(prefab); 부모 지정 없이 인스턴스화 하는 방법.
                    main_stations.SetActive(true);
                    lub_cub_0.SetActive(true);
                    GameObject lubInstance = Instantiate(normal_lub, lub_cub_0_transform); // 부모 밑에 인스턴스하기.
                    money =  doublemoney - (double)1000 + " 원";
                    //나중에 가격을 컨트롤러같은곳에 정해두고 정보를 땡겨와서 빼볼것.
                    
                    moneytext.text = money;
                    //lubInstance.transform.position = main_parent + new Vector3(0f,0f,0); 
                    // 위 선언은 부모의 좌표 + 그 옆에 어디 위치로 소환하는 방법.
                    
                    buy_stations.SetActive(false);
                }

            }
        }
    }
}
