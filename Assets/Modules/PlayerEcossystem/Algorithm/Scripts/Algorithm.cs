using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Algorithm
{
    public class Algorithm : MonoBehaviour
    {
        [SerializeField] private int maxCommands = 10;
        [SerializeField] private Transform commandsParent;
        [SerializeField] private Player player;
        
        [SerializeField] private UnityEvent<int> OnMaximumDefined;
        [SerializeField] private UnityEvent<Command, int> OnCommandLoaded;
        [SerializeField] private UnityEvent<int> OnExecution;

        private int currentExecutionIndex = 0;
        private Command[] commandSequence;
        
        private void Start()
        {
            SetCommandCallbacks();
        }

        private void SetCommandCallbacks()
        {
            Command command;
            commandsParent.GetComponents<Command>();
            for (int i = 0; i < commandsParent.childCount; i++)
            {
                if (commandsParent.GetChild(i).TryGetComponent(out command))
                {
                    command.DoLoad += InsertCommand;
                }
            }
        }

        public void DefineMaximumSlots(int? maximum = null)
        {
            if (commandSequence == null)
            {
                commandSequence = new Command[maximum ?? maxCommands];
            }
            else
            {
                Command tempArray = commandSequence.Clone();
                commandSequence = new Command[maximum ?? maxCommands];
                for (var i = 0; i < tempArray.Length; i++)
                {
                    commandSequence[i] = tempArray[i];
                }
            }
            OnMaximumDefined.Invoke(maximum ?? maxCommands);
        }

        private void InsertCommand(Command command)
        {
            if (commandSequence == null)
                DefineMaximumSlots();
            else if (commandSequence.Length >= maxCommands)
                return;
            commandSequence[currentExecutionIndex] = command;
            OnCommandLoaded.Invoke(command, commandSequence.Length);
            Debug.Log($"{command.name} has been added to algorithm");
        }

        private void ExecuteNextCommand()
        {
            var command = commandSequence.Dequeue().Execute();
            OnExecution.Invoke(commandSequence.Count);
        }

        private IEnumerator ExecuteCoroutine()
        {
            if (commandSequence == null || commandSequence.Count == 0)
                yield break;
            while (commandSequence.Count > 0)
            {
                ExecuteNextCommand();
                yield return new WaitWhile(() => player.IsActing);
            }
        }
        public void Execute(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false)
                return;
            StartCoroutine(ExecuteCoroutine());
        }
    }
}