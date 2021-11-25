﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState {  MoveToAppearPoint = 0, Phase01, Phase02, Phase03,}
public class Boss : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private float bossAppearPoint = 2.5f;
    private BossState bossState = BossState.MoveToAppearPoint;
    private Movement2D movement2D;
    private BossWeapon bossWeapon;
    private BossHP bossHP;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        bossWeapon = GetComponent<BossWeapon>();
        bossHP = GetComponent<BossHP>();
    }

    public void ChangeState(BossState newState)
    {
        // 열거형 변수.ToString()을 하게 되면 열거형에 정의한 변수 이름을
        // string 으로 받아오게 된다.
        // 이를 이용해 열거형의 이름과 코루틴 이름을 일치시켜
        // 열거형 변수에 따라 코루틴 함수 재생을 제어할 수 있다.

        // 이전에 재생중이던 상태 종료
        StopCoroutine(bossState.ToString());
        // 상태 변경
        bossState = newState;
        // 새로운 상태 재생
        StartCoroutine(bossState.ToString());
    }

    private IEnumerator MoveToAppearPoint()
    {
        // 이동방향 설정( 코루틴 실행 시 1회 호출 )
        movement2D.MoveTo(Vector3.down);

        while (true)
        {
            if(transform.position.y <= bossAppearPoint)
            {
                // 이동방향을 (0,0,0)으로 설정해 멈추도록 한다.
                movement2D.MoveTo(Vector3.zero);
                // Phase01 상태로 변경
                ChangeState(BossState.Phase01);
            }
            yield return null;
        }
    }

    private IEnumerator Phase01()
    {
        // 원 형태의 발사 공격 시작
        bossWeapon.StartFiring(AttackType.CircleFire);

        while (true)
        {
            // 현재 체력이 70% 이하가 되면
            if(bossHP.CurrentHP <= bossHP.MaxHP * 0.7f)
            {
                // 원 방사형태의 공격 중지
                bossWeapon.StopFiring(AttackType.CircleFire);
                // Phase2로 변경
                ChangeState(BossState.Phase02);
            }
            yield return null;
        }
    }
    private IEnumerator Phase02()
    {
        // 플레이어 위치를 기준으로 단일 발사체 공격 시작
        bossWeapon.StartFiring(AttackType.SingleFireToCenterPosition);

        // 처음 이동 방향을 오른쪽으로 설정
        Vector3 direction = Vector3.right;
        movement2D.MoveTo(direction);

        while (true)
        {
            // 좌~ 우 이동중 양 끝에 도달하게 되면 방향을 반대로 설정
            if( transform.position.x <= stageData.LimitMin.x ||
                transform.position.x >= stageData.LimitMax.x)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }

            // 보스의 현재 체력이 30% 이하가 되면
            if ( bossHP.CurrentHP <= bossHP.MaxHP * 0.3f)
            {
                // 플레이어 위치를 기준으로 단일 발사체 공격 시작
                bossWeapon.StopFiring(AttackType.SingleFireToCenterPosition);
                // Phase03으로 변경
                ChangeState(BossState.Phase03);
            }

            yield return null;
        }
    }
    private IEnumerator Phase03()
    {
        // 원 방사형태의 공격 시작
        bossWeapon.StartFiring(AttackType.CircleFire);
        bossWeapon.StartFiring(AttackType.SingleFireToCenterPosition);

        // 처음 이동 방향을 오른쪽으로 설정
        Vector3 direction = Vector3.right;
        // 빠져나가는거 방지?
        // movement2D.MoveTo(direction);

        while (true)
        {
            if (transform.position.x <= stageData.LimitMin.x ||
    transform.position.x >= stageData.LimitMax.x)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }
            yield return null;
        }
    }

    public void OnDie()
    {
        // 보스 파괴 파티클 생성
        GameObject clone = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 파티클 재생 완료 후 씬 전환을 위한 설정
        clone.GetComponent<BossExplosion>().Setup(playerController, nextSceneName);

        Destroy(gameObject);
    }
}
