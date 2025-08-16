using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // singleton reference

    [Header("Audio Source")]
    [SerializeField] private AudioSource[] sfx; // sound effects
    [SerializeField] private AudioSource[] bgm; // background music


    private int bgmIndex;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject); // avoid duplicates when load a new scene.

        if (bgm.Length <= 0)
            return;

        if (SceneManager.GetActiveScene().buildIndex == 0)// I want the main menu music to play when the game starts
            PlayMainMenuMusic();

        InvokeRepeating(nameof(PlayBGMIfNeeded), 0f, 2f); // check every two seconds if the BGM is playing, if not, play a random one
    }


    private void Update()
    {
        if (bgm[0].isPlaying && SceneManager.GetActiveScene().buildIndex != 0)// change the music when the scene changes, except for the main menu
            ChangeMusic(); 
    }

    private void ChangeMusic()// play a random BGM when the scene changes, except for the main menu
    {
        PlayRandomBGM(); 
    }

    public void PlayMainMenuMusic()
    {
        PlayBgm(0); // usually the first one is the main menu music
    }

    public void PlayBGMIfNeeded() // the index needs to be different to play a different music if stops and the plays
    {
        if (bgm[bgmIndex].isPlaying == false)
            PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(1, bgm.Length); // except the first one, which is usually the main menu
        PlayBgm(bgmIndex);
    }


    public void PlayBgm(int bgmToPlay)
    {
        if (bgm.Length <= 0)
        {
            Debug.LogWarning("No BGM audio sources assigned in AudioManager.");
            return;
        }

        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }

        bgmIndex = bgmToPlay;// store the index of the currently playing BGM (we do not interrupt the current one, we just stop it and play a new one)
        bgm[bgmToPlay].Play();
    }
    public void PlaySfx(int sfxToPlay, bool randomPitch = true)
    {
        if (sfxToPlay >= sfx.Length)
            return;

        if(randomPitch)
            sfx[sfxToPlay].pitch = Random.Range(0.9f, 1.1f); // Random pitch to make the sound more dynamic

        sfx[sfxToPlay].Play();
    }

    public void StopSfx(int sfxToStop) => sfx[sfxToStop].Stop();
}
