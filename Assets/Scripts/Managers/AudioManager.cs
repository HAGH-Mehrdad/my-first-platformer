using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // singleton reference

    [Header("Audio Source")]
    [SerializeField] private AudioSource[] sfx;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject); // avoid duplicates when load a new scene.
    }

    public void PlaySfx(int sfxToPlay)
    {
        if (sfxToPlay >= sfx.Length)
            return;

        sfx[sfxToPlay].Play();
    }

    public void StopSfx(int sfxToStop) => sfx[sfxToStop].Stop();
}
