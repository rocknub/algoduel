using System;
using UnityEngine;

namespace Algorithm
{
    public class AlgorithmAudio : MonoBehaviour
    {
        public ClipAndScale endSfx;
        public ClipAndScale clearSfx;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlayEndSFX()
        {
            source.PlayOneShot(endSfx.clip, endSfx.scale);
        }
        
        public void PlayClearSFX()
        {
            source.PlayOneShot(clearSfx.clip, clearSfx.scale);
        }
    }

    [Serializable]
    public class ClipAndScale
    {
        public AudioClip clip;
        [Range(0, 1)] public float scale;
    }
}