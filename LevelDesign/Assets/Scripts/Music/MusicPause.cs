using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPause : MonoBehaviour
{
    public List<AudioSource> audios;

    public void PauseAudio()
    {
        foreach (var a in audios)
        {
            a.Pause();
        }
    }
    
    public void ResumeAudio()
    {
        foreach (var a in audios)
        {
            a.UnPause();
        }
    }
}
