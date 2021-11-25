using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 35.0f;
    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // slider UI가 쫓아다닐 타겟 설정
        targetTransform = target;
        // RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 적이 파괴되어 사라지면 따라다니던 slider UI도 삭제
        if(targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // 오브젝트 월드 좌표 기준으로 화면에서의 좌표 값을 군한다.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);

        // 화면 내에서의 좌표 + distance만큼 떨어진 위치를 Slider UI 의 위치로 설정
        rectTransform.position = screenPosition + distance;
    }
}
