using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityCore.Audio;

public class HeightDetector : MonoBehaviour
{
    public AudioController audioController;
    public AudioMixer _mixer;
    public string _windVolume = "WindVolume";

    public GameObject player;

    public float height;
    private bool isHigh;
    private bool playbackWind;

    // Start is called before the first frame update


    void Start()
    {
        isHigh = false;
        playbackWind = false;
        _mixer.SetFloat(_windVolume, -80f);
    }

    // Update is called once per frame
    void Update()
    {
        MeasureHeight();

       if (isHigh && !playbackWind)
        {
            _mixer.SetFloat(_windVolume, -80f);
            WindLoopPlay(); 
        }

        else if (!isHigh && playbackWind)
        {
            _mixer.SetFloat(_windVolume, -80f);
            WindLoopStop(); 
        }

    }

    void MeasureHeight()
    {
        height = player.transform.position.y;

        float normalizedValue = Mathf.InverseLerp(10.1f, 20f, height);
        float result = Mathf.Lerp(-20f, 0f, normalizedValue);

        if (height >= 10.0f)
        { isHigh = true;
          _mixer.SetFloat(_windVolume, Mathf.Log10(normalizedValue) * 40f);
        }

        else if (height < 10.0f)
        { 
            isHigh = false;
            //_mixer.SetFloat(_windVolume, 0f);
        }
        Debug.Log("Height =" + height + ", is High =" + isHigh);

    
    }

    public void WindLoopPlay()
    {
        audioController.PlayAudio(UnityCore.Audio.AudioType.BCKGR_wind, false);
        playbackWind = true;
    }
    public void WindLoopStop()
    {
        audioController.StopAudio(UnityCore.Audio.AudioType.BCKGR_wind, false);
        playbackWind = false;
    }

}
