using Character;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCallbackTrigger : PlayerMonoBehaviour
{
    [SerializeField] private UnityEvent onPositiveCallback;
    [SerializeField] private UnityEvent onNegativeCallback;
    [SerializeField] private int playerIndexOffset;

    public void TriggerCallbacks(int entryIndex)
    {
        if (entryIndex == playerIndex + playerIndexOffset)
        {
            onPositiveCallback.Invoke();
        }
        else
            onNegativeCallback.Invoke();
    }
}