using UnityEngine;
using UnityEngine.Events;

public class Command : MonoBehaviour
{
    public UnityEvent OnExecution;

    public void Execute()
    {
        OnExecution.Invoke();
    }
}
