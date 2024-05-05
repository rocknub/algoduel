using System;
using UnityEngine;

namespace GlobalSFX
{
    public class GlobalSFXManager : MonoBehaviour
    {
        [SerializeField] private ClipAndScale duelStartSfx;
        
        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayDuelStartSFX()
        {
            source.PlayOneShot(duelStartSfx.clip, duelStartSfx.scale);
        }
    }
}