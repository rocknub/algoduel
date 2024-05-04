using UnityEngine;
using UnityEngine.Serialization;

public class MovementAudio : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;

    public void PlayFireAudio()
    {
        source.PlayOneShot(fireSfx);
    }
}
