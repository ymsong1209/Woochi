using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundBGM : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioClip[]    bgmClips;   // ¾À¸¶´Ù ¹è°æÀ½¾Ç
    [SerializeField] AudioClip      battleBGM;  // ÀüÅõ¶û Áöµµ¶û ¾ÀÀÌ °°¾Æ¼­ ±¸ºÐ

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        PlayBGM(scene.buildIndex);
    }

    private void PlayBGM(int index)
    {
        if(index < 0 || index >= bgmClips.Length)
        {
            return;
        }

        if(audioSource.isPlaying && audioSource.clip == bgmClips[index])
        {
            return;
        }

        audioSource.clip = bgmClips[index];
        audioSource.Play();
    }

    public void ToggleBattleMap(bool isBattle)
    {
        if(isBattle)
        {
            audioSource.clip = battleBGM;
        }
        else
        {
            audioSource.clip = bgmClips[1];
        }

        audioSource.Play();
    }
}
