using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private KeyCode keyCodeAttack = KeyCode.Space;
    [SerializeField]
    private KeyCode keyCodeBoom = KeyCode.Z;
    private bool isDie = false;
    private Movement2D movement2D;
    private Weapon weapon;
    private Animator animator;

    private int score;
    public int Score
    {
        // score 값이 음수가 되지않도록
        set => score = Mathf.Max(0, value);
        get => score;
    }
    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDie == true) return;
        //  방향 키를 눌러 이동방향 설정
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement2D.MoveTo(new Vector3(x, y, 0));

        // 공격 키를 Down/up으로 공격 시작/종료
        if (Input.GetKeyDown(keyCodeAttack))
        {
            weapon.StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack))
        {
            weapon.StopFiring();
        }

        // 폭탄 키를 눌러 폭탄 생성
        if(Input.GetKeyDown(keyCodeBoom))
        {
            weapon.StartBoom();
        }
    }
    private void LateUpdate()
    {
        // 플레이어 캐릭터가 화면범위 밖으로 나가지 못하도록 한다.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
                                                                Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }

    public void OnDie()
    {
        // 이동방향 초기화
        movement2D.MoveTo(Vector3.zero);
        // 사망 애니메이션 재생
        animator.SetTrigger("onDie");
        // 충돌 박스 삭제
        Destroy(GetComponent<CircleCollider2D>());
        // 사망시 플레이어 조작을 하지 못하게 한다.
        isDie = true;

    }

    public void OnDieEvent()
    {
        // 디바이스에 획득한 점수 저장
        PlayerPrefs.SetInt("Score", score);
        // 플레이어 사망 시 nextSceneName으로 이동
        SceneManager.LoadScene(nextSceneName);
    }
}

/*
 * Desc : 플레이어 캐릭터에 부착해서 사용
 */ 
