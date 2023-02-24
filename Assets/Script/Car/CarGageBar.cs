using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGageBar : MonoBehaviour
{ 
    private Camera _gageCamera;
    private Canvas _gageCanvas;
    private RectTransform _rectParent; //트랜스폼 값을 저장할 변수를 생성
    private RectTransform _rectGage; //자기자신 트랜스폼

    [HideInInspector] public Vector3 offset = Vector3.zero; 
    //캐릭터에서 얼마만큼 떨어뜨려서 게이지를 둘건지를 정하는값.
    [HideInInspector] public Transform targetTransform;
    
    
    void Start()
    {
        _gageCanvas = GetComponentInParent<Canvas>();
        _gageCamera = _gageCanvas.worldCamera;
        _rectParent = _gageCanvas.GetComponent<RectTransform>();
        _rectGage = this.gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        // 자동차가 다 움직인 다음에 체력게이지가 따라가야함.

        var screenPos = Camera.main.WorldToScreenPoint(targetTransform.position + offset);
        //월드 좌표를 스크린 좌표로 바꾼거임.
        
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f; //스크린 좌표로 변환을 했는데 x,y밖에 필요가없음.
            // z는 메인카메라에서 대상까지의 거리라서 큰 의미가없음.
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, screenPos, _gageCamera, out localPos);
        //ui캔버스에서 사용할 수 있는  좌표로 바꿔주는 함수를 사용한 것임.

        _rectGage.localPosition = localPos;
        //체력 게이지에 직접 표시를 한거임.
    }
}
