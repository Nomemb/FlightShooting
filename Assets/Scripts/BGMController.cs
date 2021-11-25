using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType {  Stage = 0, Boss}
public class BGMController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] bgmClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeBGM(BGMType index)
    {
        // 현재 재생 중인 배경음악 정지
        audioSource.Stop();

        // 다른 클래스에서 BGM을 설정할때 정수를 사용하면
        // bgmClips를 확인해야 알 수 있기 때문에 열거형을 사용해 가독성을 높여준다

        // index 번째 배경음악으로 파일 교체
        audioSource.clip = bgmClips[(int)index];
        // 바뀐 배경음악 재생
        audioSource.Play();
    }
}
