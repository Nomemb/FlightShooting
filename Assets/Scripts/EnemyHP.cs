using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 4;
    private float currentHP;
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        StopCoroutine("HitColorAnimation");
        StartCoroutine("HitColorAnimation");

        if (currentHP <= 0)
        {
            enemy.OnDie();
        }
    }

    private IEnumerator HitColorAnimation()
    {
        // 플레이어 색상을 빨간색으로 변경
        spriteRenderer.color = Color.red;
        // 0.1 초 대기
        yield return new WaitForSeconds(0.05f);
        // 플레이어 색상을 원래 색으로 다시 변경
        spriteRenderer.color = Color.white;
    }
}
