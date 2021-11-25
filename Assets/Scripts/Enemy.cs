using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 1; // 적 공격력
    [SerializeField]
    private int scorePoint = 100; // 적 처치시 획득 점수
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject[] itemPrefabs;
    private PlayerController playerController; // 플레이어의 점수 정보에 접근

    private void Awake()
    {
        // 오브젝트 풀링을 이용해 오브젝트를 재사용할 경우에는
        // 최초 1번만 find를 이용해
        // PlayerController의 정보를 저장해두고 사용하는 것이 연산에 효율적이다.

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 공격력만큼 플레이어 체력감소
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            OnDie();
        }
    }

    public void OnDie()
    {
        playerController.Score += scorePoint;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 일정 확률로 아이템 생성
        SpawnItem();

        Destroy(gameObject);
    }

    private void SpawnItem()
    {
        // 파워업 (10%), 폭탄+1(5%), 체력회복(15%)
        int spawnItem = Random.Range(0, 100);
        if( spawnItem  <10)
        {
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
        }
        else if(spawnItem < 15)
        {
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        else if( spawnItem < 30)
        {
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);

        }
    }
}
