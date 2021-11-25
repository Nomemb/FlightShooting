using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject : 해당 클래스를 에셋 파일 형태로 저장 가능
// CreateAssetMenu : 프로젝트 뷰의 create 메뉴에 메뉴로 등록된다.
[CreateAssetMenu]
public class StageData : ScriptableObject
{
    [SerializeField]
    private Vector2 limitMin;
    [SerializeField]
    private Vector2 limitMax;

    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;

}

/*
 * Desc
 * : 현재 스테이지의 화면 내 범위
 * : 에셋 데이터로 저장해두고 정보를 불러와서 사용
 */ 
