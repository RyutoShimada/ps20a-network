using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    /// <summary>スピードアップ時の効果音 </summary>
    [SerializeField] AudioClip m_speedUp;
    /// <summary>プレイヤーとの接触時の効果音 </summary>
    [SerializeField] AudioClip m_touchPlayer;
    /// <summary>ボタン効果音 </summary>
    [SerializeField] AudioClip m_buttonDown;
    AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlaySpeedUpSE()
    {
        audioSource.PlayOneShot(m_speedUp);
    }
    public void PlayTouchSE()
    {
        audioSource.PlayOneShot(m_touchPlayer);
    }
    public void PlayButtonSE()
    {
        audioSource.PlayOneShot(m_buttonDown);
    }
}
