using System;
using UnityEngine;

public class SFXController : SingletonBehaviourDontDestroy<SFXController>{
    [SerializeField] private AudioSource sndSource;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

    private void PlayOnShoot(AudioClip clip) {
        if (!sndSource || !clip) return;

        sndSource.PlayOneShot(clip);
    }

    public void PlaySndWin() {
        PlayOnShoot(winClip);
    }

    public void PLaySndLose() {
        PlayOnShoot(loseClip);
    }
}