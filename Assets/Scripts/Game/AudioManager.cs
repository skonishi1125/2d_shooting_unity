using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource seSource;

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

    public void PlaySeOneShot(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        seSource.pitch = pitch;
        seSource.PlayOneShot(clip, volume);
        seSource.pitch = 1f; // 戻す必要がある
    }

    public void PlaySeAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
    {
        // 削除されたオブジェクト(clip)を呼んで作るので、seSourceを使わなくてよい
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }


}
