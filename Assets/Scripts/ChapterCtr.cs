using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChapterCtr : MonoBehaviour
{
    public PlayableDirector director;
    public CameraParallax cameraParallax;

    void Start()
    {
        director.stopped += OnTimelineEnd;
        cameraParallax.enabled = false;
        director.Play();
    }

    void OnTimelineEnd(PlayableDirector pd)
    {
        cameraParallax.enabled = true;
    }
}
