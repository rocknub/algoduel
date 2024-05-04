using UnityEngine;
using UnityEngine.Serialization;

public class MovementAudio : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private AudioClip fallSfx;

    public void PlayFireAudio()
    {
        source.PlayOneShot(fireSfx);
    }

    public void PlayFallSfx()
    {
        source.PlayOneShot(fallSfx);
    }
}
