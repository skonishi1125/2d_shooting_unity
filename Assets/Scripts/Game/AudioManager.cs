using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioSource bgmSource;

    private void Awake()
    {
        CheckAudioManager();
    }

    private void CheckAudioManager()
    {
        // シングルトンパターンで設計
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);// Stage2, 3とSceneをまたいで残したい
    }

    public void PlayBgm()
    {
        // wip
    }

    public void PlaySeOneShot(AudioClip clip, float volume = 1f )
    {
        seSource.PlayOneShot(clip, volume);
    }

    public void PlaySeAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
    {
        // 削除されたオブジェクト(clip)を呼んで作るので、seSourceを使わなくてよい
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }


}
