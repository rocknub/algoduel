using System.Collections;
using System.Collections.Generic;
using Character;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Algorithm
{
    public class Algorithm : MonoBehaviour
    {
        [SerializeField] private int maxCommands = 10;
        [SerializeField] private Transform commandsParent;
        [CanBeNull][SerializeField] private Player player; 
        
        private Queue<Command> commandSequence;

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
                    command.DoLoad += EnqueueCommand;
                }
            }
        }

        private void EnqueueCommand(Command command)
        {
            if (commandSequence == null)
                commandSequence = new Queue<Command>(maxCommands);
            else if (commandSequence.Count >= maxCommands)
                return;
            commandSequence.Enqueue(command);
            Debug.Log($"{command.name} has been added to algorithm");
        }

        private void ExecuteNextCommand()
        {
            var command = commandSequence.Dequeue().Execute();
            Debug.Log($"{command.name} has been executed");
        }

        private IEnumerator ExecuteCoroutine()
        {
            if (commandSequence == null || commandSequence.Count == 0)
                yield break;
            Debug.Log("Executing");
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