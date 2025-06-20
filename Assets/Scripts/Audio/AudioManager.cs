using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX Clips")]
    public AudioClip sunpickup;


    [Header("BGM Clips")]
    public AudioClip backgroundMusic;
    private AudioSource bgmSource; // Nhạc nền

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitBGM();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Phát âm thanh tại vị trí chỉ định, tự hủy sau khi phát xong.
    /// </summary>
    public void PlaySoundAt(Vector3 position, AudioClip clip)
    {
        if (clip == null) return;

        GameObject temp = new GameObject("TempAudio");
        temp.transform.position = position;

        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        Destroy(temp, clip.length); // Tự hủy sau khi phát xong
    }

    private void InitBGM()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f); // nếu bạn lưu volume
        bgmSource.Play();
    }

    /// <summary>
    /// Gọi hàm này để phát âm khi nhặt Sun
    /// </summary>
    public void PlayCollectSunSound(Vector3 position)
    {
        PlaySoundAt(position, sunpickup);
    }
}
