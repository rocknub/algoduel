using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AudioFader : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    [SerializeField] private int loopCount;
    [SerializeField] private LoopType loopType;
    [SerializeField] private AudioSource[] sources;
    [Range(0, 1)] [SerializeField] private float[] targetVolume;
    [SerializeField] private bool fadeOnAwake;
    [SerializeField] private bool unscaledTime;

    private float[] originalVolume;
    
    public Tween[] Tween { get; private set; } 

    private void Awake()
    {
        if (fadeOnAwake)
        {
            StartFade();
        }
    }

    [ContextMenu("Start Fade")]
    public void StartFade()
    {
        originalVolume = new float[sources.Length];
        Tween = new Tween[sources.Length];
        for (var i = 0; i < sources.Length; i++)
        {
            var source = sources[i];
            originalVolume[i] = source.volume;
            Tween[i] = source.DOFade(targetVolume[i], duration).SetLoops(loopCount, loopType).SetEase(easeType);
            if (unscaledTime)
            {
                Tween[i].SetUpdate(UpdateType.Normal, true);
            }
        }
    }

    // [ContextMenu("Use The First Alpha For All")]
    // private void UseAlphaForAll()
    // {
    //     var targetAlpha = targetAlphas[0];
    //     if (targetAlphas.Length != graphics.Length) targetAlphas = new float[graphics.Length];
    //     for (var index = 0; index < targetAlphas.Length; index++)
    //     {
    //         targetAlphas[index] = targetAlpha;
    //     }
    // }

    public void CeaseTween()
    {
        for (var i = 0; i < sources.Length; i++)
        {
            Tween[i].Kill();
            var graphic = sources[i];
            graphic.DOFade(originalVolume[i], 0);
        }
    }
}