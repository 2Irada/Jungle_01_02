using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound  // MonoBehaviour 상속을 받지 않은 클래스는 컴포넌트로 추가 불가능
{
    public string name;     // 사운드 이름
    public AudioClip clip;  // 사운드
}

public class SoundManager : MonoBehaviour
{
    // 사운드 매니저 싱글톤 사용
    public static SoundManager instance;

    void Awake()
    {
        instance = this;
    }

    public Sound[] effectSounds;    // 효과음 오디오 클립들
    public Sound[] bgmSounds;       // BGM 오디오 클립들

    public AudioSource audioSourceBGM;  // BGM 재생기, BGM 재생은 한 군데서만 이뤄지므로 배열 X
    public AudioSource[] audioSourceEffects;    // 효과음들은 동시에 여러개가 재생될 수 있으므로 배열 O

    public string[] playSoundName;  // 재생 중인 효과음 사운드 이름 배열

    void Start()
    {
        // 효과음 배열의 크기만큼 초기화
        playSoundName = new string[audioSourceEffects.Length];

        // 배경음 재생
        // PlayBGM("BGM_1");
        // audioSourceBGM.loop = true;
    }

    // 효과음 재생 함수
    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            // 매개변수로 받은 이름이 오디오클립의 이름과 같으면
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    // 효과음이 재생중이지 않다면
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        // 현재 놀고 있는 재생기(오디오클립)에 매개변수로 받은 효과음을 할당, 재생
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        // 재생 중인 효과음은 매개변수로 받은 효과음의 이름이 됨
                        playSoundName[j] = effectSounds[i].name;
                        // 종료
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용 중입니다.");
                return;
            }
        }
        Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다.");
    }

    // BGM 재생 함수
    public void PlayBGM(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            // 매개변수로 받은 이름이 bgm의 이름과 같으면
            if (_name == bgmSounds[i].name)
            {
                // bgm재생기에 입력받은 bgm을 할당, 재생
                audioSourceBGM.clip = bgmSounds[i].clip;
                audioSourceBGM.Play();
                return;
            }
        }
        Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다.");
    }

    // 모든 효과음 재생을 멈추는 함수
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    // 특정 효과음 재생을 멈추는 함수
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            // 재생중인 효과음의 이름이 매개변수로 받은 이름과 같다면
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중인 " + _name + " 사운드가 없습니다.");
    }
}