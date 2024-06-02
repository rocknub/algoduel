using System;
using DG.Tweening;
using UnityEngine;

namespace Modules.Soundtrack
{
    public class SoundtrackManager : MonoBehaviour
    {
        public AudioClip duelSoundtrack;
        public float duelTweenDuration;
        public float initialVolume;
        public float gameplayVolume;

        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            source.volume = initialVolume;
        }

        public void SetDuelSoundtrack()
        {
            if (source.clip != duelSoundtrack)
            {
                source.clip = duelSoundtrack;
                source.Play();
            }

            source.DOFade(gameplayVolume, duelTweenDuration);
        }
    }
}