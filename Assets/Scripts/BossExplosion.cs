using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossExplosion : MonoBehaviour
{
    private PlayerController playerController;
    private string sceneName;

    public void Setup(PlayerController playerController, string sceneName)
    {
        this.playerController = playerController;
        this.sceneName = sceneName;
    }
    /// <summary>
    /// ParticleAutoDestory 컴포넌트에서 파티클 재생이 완료되면
    /// 파티클을 삭제하기 떄문에
    /// 오브젝트가 삭제될 떄 호출되는 OnDie()함수를 이용해 파티클 재생이
    /// 완료 되었을 때 필요한 처리를 설정한다.
    /// </summary>
    private void OnDestroy()
    {
        // 보스 처치 + 10000
        playerController.Score += 10000;
        // 플레이어 획득 점수를 "Score" 키에 저장
        PlayerPrefs.SetInt("Score", playerController.Score);
        // sceneName으로 씬 변경
        SceneManager.LoadScene(sceneName);
    }

}
