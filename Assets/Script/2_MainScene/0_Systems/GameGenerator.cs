using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// 플레이어 움직임에 따른 차량 생성과 움직임 조작
/// 플레이어 이동, 시간 경과 후 출현하는 오브젝트

public class GameGenerator : MonoBehaviour
{
    
    
}

/*
컨트롤러 스크립트 -> 제너레이터 스크립트 -> 감독 스크립트

게임매니저 : 게임 규칙 및 정보 담기(ex :싱글톤?)
플레이어 컨트롤러 : 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본
게임 제너레이터 : 플레이어 움직임에 따른 구름 생성과 움직임 조작 / 플레이어 이동,시간 경과 후 출현하는 오브젝트
게임 디렉터(감독) : 점수 계산 및 게임 진행 재시작 및 오버 여부 / ui 조작하거나 진행 상황을 판단

*/