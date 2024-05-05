using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private PlayerManager player;
        
        [SerializeField] private UnityEvent<int> OnMaximumDefined;
        [SerializeField] private UnityEvent<Command, int> OnCommandLoaded;
        [SerializeField] private UnityEvent<int> OnExecution;
        [SerializeField] private UnityEvent OnSequenceEnd;
        [SerializeField] private UnityEvent<int> OnClearance;

        private List<Command> commandSequence;
        private Coroutine executionRoutine;
        
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
            maximum ??= maxCommands;
            if (commandSequence == null)
            {
                commandSequence = new List<Command>(maximum.Value);
            }
            else if (commandSequence.Capacity != maximum)
            {
                int copyLength = maximum.Value < commandSequence.Capacity ? maximum.Value : commandSequence.Capacity;
                Command[] tempArray = new Command[maximum.Value];
                commandSequence.CopyTo(0, tempArray, 0, copyLength);
                commandSequence = new List<Command>(maximum.Value);
                for (var i = 0; i < tempArray.Length; i++)
                {
                    commandSequence[i] = tempArray[i];
                }
            }
            OnMaximumDefined.Invoke(maximum.Value);
            maxCommands = maximum.Value;
        }

        public void Clear()
        {
            if (executionRoutine != null || commandSequence == null || commandSequence.Count == 0)
                return;
            OnClearance.Invoke(commandSequence.Count);
            commandSequence.Clear();
        }

        private void InsertCommand(Command command)
        {
            if (executionRoutine != null)
                return;
            if (commandSequence == null)
                DefineMaximumSlots();
            else if (commandSequence.Count >= maxCommands)
                return;
            commandSequence.Add(command);
            OnCommandLoaded.Invoke(command, commandSequence.Count - 1);
        }

        private void ExecuteCommand(int executionIndex)
        {
            var command = commandSequence[executionIndex].Execute();
            OnExecution.Invoke(executionIndex);
        }

        private IEnumerator ExecuteCoroutine()
        {
            if (commandSequence == null || commandSequence.Count == 0)
                yield break;
            var currentExecutionIndex = 0;
            while (currentExecutionIndex < commandSequence.Count)
            {
                ExecuteCommand(currentExecutionIndex);
                yield return new WaitUntil(() => player.CanAct);
                currentExecutionIndex++;
            }
            executionRoutine = null;
            OnSequenceEnd.Invoke();
        }
        public void Execute(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false || executionRoutine != null)
                return;
            executionRoutine = StartCoroutine(ExecuteCoroutine());
        }

        public void HaltExecution()
        {
            if (executionRoutine == null) 
                return;
            StopCoroutine(executionRoutine);
            executionRoutine = null;
            OnSequenceEnd.Invoke();
        }
    }
}