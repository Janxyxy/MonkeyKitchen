using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    private float volume = 0.3f;
    private AudioSource audioSource;

    private const string VOLUME_KEY = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        volume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.3f);
        audioSource.volume = volume;
    }

    internal void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        audioSource.volume = volume;

        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    internal float GetVolume()
    {
        return volume;
    }
}
