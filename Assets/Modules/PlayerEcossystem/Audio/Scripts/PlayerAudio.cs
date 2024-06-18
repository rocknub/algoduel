using Character;
using UnityEngine;

public class PlayerAudio : PlayerMonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private AudioClip fallSfx;
    [SerializeField] private ClipAndScale playerInputConfirmationSFX;
    [SerializeField] private ClipAndScale playerSingleWinSFX;

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
        if (entryKey != playerIndex)
        {
            return;
        }
        source.PlayOneShot(playerInputConfirmationSFX.clip, playerInputConfirmationSFX.scale);
    }

    public void PlaySingleWinSFX(int entryKey)
    {
        if (entryKey != playerIndex)
        {
            return;
        }
        source.PlayOneShot(playerSingleWinSFX.clip, playerSingleWinSFX.scale);
    }
}
