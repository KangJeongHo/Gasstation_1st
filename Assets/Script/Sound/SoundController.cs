using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    
    #region SingleTon_In_SoundController

    private static SoundController _instance;

    public static SoundController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SoundController)) as SoundController;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 오브젝트 없음");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    public AudioSource audioSource;
    public AudioClip[] carCome;
    public AudioClip[] chargeStart;
    public AudioClip[] charging;
    public AudioClip chargeFinish;
    public AudioClip[] getCoins;
    public AudioClip[] satisUp;
    public AudioClip[] satisDown;
    public AudioClip carGone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void 소리재생(string action)
    {
        audioSource.pitch = 1f;
        switch (action)
        {
            
            case "차량등장":
                {
                    audioSource.clip = carCome[Random.Range(0,2)];
                    break;
                }
            case "주유시작":
                {
                    audioSource.clip = chargeStart[Random.Range(0,2)];
                    break;
                }
            case "주유중":
                {
                    audioSource.clip = charging[Random.Range(0,11)];
                    break;
                }
            case "주유완료":
                {
                    audioSource.clip = chargeFinish;
                    break;
                }
            case "돈받음":
                {
                    audioSource.clip = getCoins[Random.Range(0,4)];
                    break;
                }
            case "만족도상승":
                {
                    audioSource.clip = satisUp[Random.Range(0,1)];
                    break;
                }
            case "만족도하락":
                {
                    audioSource.clip = satisDown[Random.Range(0,1)];
                    break;
                }
            case "차량떠남":
                {
                    audioSource.clip = carGone;
                    audioSource.pitch = 2f;
                    break;
                }
        }
        audioSource.Play();
    }
}
