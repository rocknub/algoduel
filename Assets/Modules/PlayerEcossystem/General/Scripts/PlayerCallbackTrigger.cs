using Character;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCallbackTrigger : PlayerMonoBehaviour
{
    [SerializeField] private UnityEvent onPositiveCallback;
    [SerializeField] private UnityEvent onNegativeCallback;

    public void TriggerCallbacks(int entryIndex)
    {
        if (entryIndex == playerIndex)
            onPositiveCallback.Invoke();
        else
            onNegativeCallback.Invoke();
    }
}