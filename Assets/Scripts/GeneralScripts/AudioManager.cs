using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixers")]
    public AudioMixer masterMixer;

    [Header("Volume Parameters")]
    public string masterVolumeParameter = "MasterVolume";
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    [Header("Audio Sources")]
    public AudioSource musicSource; // Reference to the AudioSource for music

    private bool isMusicPlaying = true; // Track if music is currently playing

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
        SceneManager.sceneLoaded += OnSceneLoaded; // Register to the sceneLoaded event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the scene name and control music accordingly
        if (scene.name == "AudioGame")
        {
            StopMusic();
        }
        else
        {
            ResumeMusic();
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat(masterVolumeParameter, LinearToDecibel(volume));
        PlayerPrefs.SetFloat(masterVolumeParameter, volume);
    }

    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat(musicVolumeParameter, LinearToDecibel(volume));
        PlayerPrefs.SetFloat(musicVolumeParameter, volume);
        musicSource.volume = volume; // Update the music source volume
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat(sfxVolumeParameter, LinearToDecibel(volume));
        PlayerPrefs.SetFloat(sfxVolumeParameter, volume);
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(masterVolumeParameter))
        {
            float masterVolume = PlayerPrefs.GetFloat(masterVolumeParameter);
            masterMixer.SetFloat(masterVolumeParameter, LinearToDecibel(masterVolume));
        }
        if (PlayerPrefs.HasKey(musicVolumeParameter))
        {
            float musicVolume = PlayerPrefs.GetFloat(musicVolumeParameter);
            masterMixer.SetFloat(musicVolumeParameter, LinearToDecibel(musicVolume));
            musicSource.volume = musicVolume; // Load the saved music volume
        }
        if (PlayerPrefs.HasKey(sfxVolumeParameter))
        {
            float sfxVolume = PlayerPrefs.GetFloat(sfxVolumeParameter);
            masterMixer.SetFloat(sfxVolumeParameter, LinearToDecibel(sfxVolume));
        }
    }

    private float LinearToDecibel(float linear)
    {
        return linear <= 0 ? -80f : 20f * Mathf.Log10(linear);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (isMusicPlaying)
        {
            musicSource.Stop();
            isMusicPlaying = false;
        }
    }

    public void ResumeMusic()
    {
        if (!isMusicPlaying)
        {
            musicSource.UnPause();
            isMusicPlaying = true;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister from the sceneLoaded event
    }
}
