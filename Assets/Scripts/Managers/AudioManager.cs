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
