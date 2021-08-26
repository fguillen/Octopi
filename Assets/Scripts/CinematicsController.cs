using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicsController : MonoBehaviour
{
    PlayableDirector playableDirector;
    [SerializeField] float skipTime;


    void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void Skip()
    {
        if(playableDirector.time > skipTime)
            return;

        playableDirector.time = skipTime;
    }

}
