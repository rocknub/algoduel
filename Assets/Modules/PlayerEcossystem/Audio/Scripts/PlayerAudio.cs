using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private AudioClip fallSfx;
    [SerializeField] private int validationKey;
    [SerializeField] private ClipAndScale playerInputConfirmationSFX;

    public void PlayFireAudio()
    {
        source.PlayOneShot(fireSfx);
    }

    public void PlayFallSfx()
    {
        source.PlayOneShot(fallSfx);
    }

    public void PlayInputConfirmationSfx(int entryKey)
    {
        if (entryKey != validationKey)
        {
            return;
        }
        source.PlayOneShot(playerInputConfirmationSFX.clip, playerInputConfirmationSFX.scale);
    }
}
