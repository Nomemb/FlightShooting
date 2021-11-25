using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float attackRate = 0.1f;
    [SerializeField]
    private int maxAttackLevel = 3;
    private int attackLevel = 1;
    private AudioSource audioSource;

    [SerializeField]
    private GameObject boomPrefab;
    private int boomCount = 3;

    public int AttactLevel
    {
        set => attackLevel = Mathf.Clamp(value, 1, maxAttackLevel);
        get => attackLevel;
    }
    public int BoomCount
    {
        set => boomCount = Mathf.Max(0, value);
        get => boomCount;

    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void StartFiring()
    {
        StartCoroutine("TryAttack");
    }
    public void StopFiring()
    {
        StopCoroutine("TryAttack");
    }

    public void StartBoom()
    {
        if(boomCount > 0)
        {
            boomCount--;
            Instantiate(boomPrefab, transform.position, Quaternion.identity);
        }
    }
    private IEnumerator TryAttack()
    {
        while (true)
        {
            // 발사체 오브젝트 생성
            //Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 공격 레벨에 따라 발사체 생성
            AttackByLevel();
            audioSource.Play();
            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }

    private void AttackByLevel()
    {
        GameObject cloneProjectile = null;

        switch (attackLevel)
        {
            case 1:  // Lv 01 : 발사체 1개
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                break;

            case 2: // Lv 02 :  간격을 두고 전방으로 발사체 2개 생성
                Instantiate(projectilePrefab, transform.position + Vector3.left * 0.2f, Quaternion.identity);
                Instantiate(projectilePrefab, transform.position + Vector3.right * 0.2f , Quaternion.identity);
                break;

            case 3: // Lv 03 : 전방으로 발사체 1개, 좌우 대각 방향으로 발사체 각 1개씩
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // 왼쪽 대각선 발사체                
                cloneProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile.GetComponent<Movement2D>().MoveTo(new Vector3(-0.2f, 1, 0));
                // 오른쪽 대각선 발사체
                cloneProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile.GetComponent<Movement2D>().MoveTo(new Vector3(0.2f, 1, 0));
                break;
        }
    }
}
