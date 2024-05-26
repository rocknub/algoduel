using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Algorithm
{
    public class Command : MonoBehaviour
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private IconData iconData;
        public UnityEvent OnExecution;
        public UnityAction<Command> DoLoad;

        public IconData IconData => iconData;

        public Command Execute()
        {
            OnExecution.Invoke();
            return this;
        }
    
        public void Execute(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false || GameManager.Instance.isGamePaused)
                return;
            Execute();
        }

        public void Load(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false || GameManager.Instance.isGamePaused)
                return; 
            DoLoad(this);
        }
    }
}
