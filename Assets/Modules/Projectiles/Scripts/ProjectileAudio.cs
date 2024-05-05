using System;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileAudio : MonoBehaviour
    {
        [SerializeField] private LayerAudio[] layerAudios;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayCorrespondingCollisionSFX(Collider collider)
        {
            foreach (var layerAudio in layerAudios)
            {
                if ((layerAudio.layerMask & (1 << collider.gameObject.layer)) == 0) 
                    continue;
                Debug.Log("Audio Playerd!");
                _source.PlayOneShot(layerAudio.audioClip, layerAudio.relativeVolume);
                break;
            }
        }
    }

    [Serializable]
    public class LayerAudio
    {
        public string name;
        public LayerMask layerMask;
        public AudioClip audioClip;
        [Range(0, 1)] public float relativeVolume;
    }
}