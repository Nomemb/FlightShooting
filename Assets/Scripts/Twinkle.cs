using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour
{
    private float fadeTime = 0.1f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine("TwinkleLoop");
    }

    private IEnumerator TwinkleLoop()
    {
        while (true)
        {
            // Alpha 값을 1에서 0으로 : Fade Out
            yield return StartCoroutine(FadeEffect(1, 0));
            // Alpha 값을 0에서 1로 : Fade In
            yield return StartCoroutine(FadeEffect(1, 0));
        }
    }

    private IEnumerator FadeEffect(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            // fadeTime 시간동안 while()반복문 실행
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // spriteRenderer.color, transform.position은 프로퍼티로 초기화가 불가능하다/
            Color color = spriteRenderer.color;
            // start와 end사이 값 중 percent 위치에 있는 값을 반환
            // start = 0, end = 100일 때 percent가 0.3이면 30을 반환
            color.a = Mathf.Lerp(start, end, percent);
            spriteRenderer.color = color;

            yield return null;
        }
    }
}
