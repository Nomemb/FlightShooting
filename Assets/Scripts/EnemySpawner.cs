using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private StageData stageData; // 적 생성을 위한 스테이지 크기 정보
    [SerializeField]
    private GameObject enemyPrefab; 
    [SerializeField]
    private GameObject enemyHPSliderPrefab;
    [SerializeField]
    private Transform canvasTransform; // UI 표현하는 Canvas 오브젝트의 Transform
    [SerializeField]
    private BGMController bgmController; // 배경음악 설정( 보스 등장 시 변경 )
    [SerializeField]
    private GameObject textBossWarning; // 보스 등장 텍스트 오브젝트
    [SerializeField]
    private GameObject panelBossHP; // 보스 체력 패널 오브젝트
    [SerializeField]
    private GameObject boss; // 보스 오브젝트
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private int maxEnemyCount = 100; // 현재 스테이지의 최대 적 생성 숫자

    private void Awake()
    {
        // 보스 등장 텍스트 비활성화
        textBossWarning.SetActive(false);
        // 보스 체력 패널 비활성화
        panelBossHP.SetActive(false);
        // 보스 오브젝트 비활성화
        boss.SetActive(false);
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        int currentEnemyCount = 0;
        while (true)
        {
            // x 위치는 스테이지의 크기 범위 내에서 임의의 값을 선택
            float positionX = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);

            // 적 캐릭터 생성

            Vector3 position = new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f);
            GameObject enemyClone = Instantiate(enemyPrefab, position, Quaternion.identity);

            SpawnEnemyHPSlider(enemyClone);

            currentEnemyCount++;

            // 적을 최대 숫자까지 생성하면 적 생성 코루틴 중지, 보스 생성 코루틴 실행
            if(currentEnemyCount == maxEnemyCount)
            {
                StartCoroutine("SpawnBoss");
                break;
            }
            // spawnTime만큼 대기
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        // slider UI 오브젝트를 Canvas 오브젝트의 자식으로 설정
        // UI는 캔버스의 자식으로 생성되어 있어야 화면에 보인다.
        sliderClone.transform.SetParent(canvasTransform);

        sliderClone.transform.localScale = Vector3.one;
        // Slider UI가 쫓아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
    private IEnumerator SpawnBoss()
    {
        // 보스 등장 BGM
        bgmController.ChangeBGM(BGMType.Boss);

        // 보스 등장 텍스트 활성화
        textBossWarning.SetActive(true);

        // 1초 대기
        yield return new WaitForSeconds(1.0f);

        // 보스 등장 텍스트 비활성화
        textBossWarning.SetActive(false);
        // 보스 체력 패널 활성화
        panelBossHP.SetActive(true);
        // 보스 오브젝트 활성화
        boss.SetActive(true);
        // 보스의 첫 번째 상태인 지정된 위치로 이동 실행
        boss.GetComponent<Boss>().ChangeState(BossState.MoveToAppearPoint);
    }
}
