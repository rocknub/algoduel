using System;
using DG.Tweening;
using UnityEngine;

namespace Modules.Soundtrack
{
    public class SoundtrackManager : MonoBehaviour
    {
        public AudioClip duelSoundtrack;
        public float duelTweenDuration;
        
        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public void SetDuelSoundtrack()
        {
            if (source.clip != duelSoundtrack)
            {
                source.clip = duelSoundtrack;
                source.Play();
            }

            source.DOFade(source.volume * 2, duelTweenDuration);
        }
    }
}