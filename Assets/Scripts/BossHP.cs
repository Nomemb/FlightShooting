using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 1000; // 최대 체력
    private float currentHP; // 현재 체력
    private SpriteRenderer spriteRenderer;
    private Boss boss;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss = GetComponent<Boss>();
    }

    public void TakeDamage(float damage)
    {
        // 현재 체력을 damage만큼 감소
        currentHP -= damage;

        StopCoroutine("HitColorAnimation");
        StartCoroutine("HitColorAnimation");

        // 체력이 0 이하이면
        if(currentHP <= 0)
        {
            boss.OnDie();
        }
    }

    private IEnumerator HitColorAnimation()
    {
        {
            // 플레이어 색상을 빨간색으로 변경
            spriteRenderer.color = Color.red;
            // 0.1 초 대기
            yield return new WaitForSeconds(0.05f);
            // 플레이어 색상을 원래 색으로 다시 변경
            spriteRenderer.color = Color.white;
        }
    }
}
