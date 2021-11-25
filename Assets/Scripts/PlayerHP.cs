using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private float maxHp = 10;
    private float currentHp;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    public float MaxHP => maxHp;
    public float CurrentHP
    {
        set => currentHp = Mathf.Clamp(value, 0, maxHp);
        get => currentHp;
    }


    private void Awake()
    {
        currentHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        StopCoroutine("HitColorAnimation");
        StartCoroutine("HitColorAnimation");

        if(currentHp <= 0)
        {
            playerController.OnDie();
        }
    }

    private IEnumerator HitColorAnimation()
    {
        // 플레이어 색상을 빨간색으로 변경
        spriteRenderer.color = Color.red;
        // 0.1 초 대기
        yield return new WaitForSeconds(0.1f);
        // 플레이어 색상을 원래 색으로 다시 변경
        spriteRenderer.color = Color.white;
    }
}
