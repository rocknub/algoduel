using UnityEngine;

namespace Modules.Soundtrack
{
    public class SoundtrackManager : MonoBehaviour
    {
        public AudioClip duelSoundtrack;

        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public void SetDuelSoundtrack()
        {
            if (source.clip == duelSoundtrack) 
                return;
            source.clip = duelSoundtrack;
            source.Play();
        }
    }
}