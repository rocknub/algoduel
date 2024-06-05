using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Algorithm
{
    public class Algorithm : PlayerMonoBehaviour
    {
        [SerializeField] private int maximumSlots;
        [FormerlySerializedAs("maxCommands")] [SerializeField] private int initialAvailableSlots;
        [SerializeField] private int maximumIncrementStep;
        [SerializeField] private float rechargePeriod;
        [SerializeField] private Transform commandsParent;
        [SerializeField] private PlayerManager player;

        [SerializeField] private bool clearAlgorithmOnConclusion = true;
        [SerializeField] private bool clearAlgorithmOnHalt = true;

        [SerializeField] private UnityEvent<int> OnMaximumDefined;
        [SerializeField] private UnityEvent<Command, int> OnCommandLoaded;
        [SerializeField] private UnityEvent<int> OnSlotCleared;
        [SerializeField] private UnityEvent<int> OnExecution;
        [SerializeField] private UnityEvent<float> OnSequenceEnd;
        [SerializeField] private UnityEvent<float> OnClearance;

        private bool isRecharging;
        private List<Command> commandSequence;
        private Coroutine executionRoutine;
        private Coroutine rechargeRoutine;

        public float SequenceFulfillmentRate;

        private void Start()
        {
            SetCommandCallbacks();
            DefineMaximumSlots(initialAvailableSlots);
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
            if (initialAvailableSlots >= maximumSlots)
                return;
            maximum ??= initialAvailableSlots;
            if (commandSequence == null || commandSequence.Capacity != maximum)
            {
                commandSequence = new List<Command>(maximum.Value);
            }
            OnMaximumDefined.Invoke(maximum.Value);
            initialAvailableSlots = maximum.Value;
        }

        [ContextMenu("Increment Maximum Slots")]
        public void IncrementMaximumSlots(int incrementValue) => DefineMaximumSlots(initialAvailableSlots + incrementValue);

        public void TryIncrementMaximumSlots(int entryIndex)
        {
            if (entryIndex != playerIndex)
                return;
            IncrementMaximumSlots(maximumIncrementStep);
        }

        public void Clear()
        {
            if (executionRoutine != null || commandSequence == null || commandSequence.Count == 0 || GameManager.Instance.isGamePaused)
                return;
            OnClearance.Invoke(SequenceFulfillmentRate);
            commandSequence.Clear();
            rechargeRoutine = StartCoroutine(Recharge());
        }

        private IEnumerator Recharge()
        {
            isRecharging = true;
            yield return new WaitForSeconds(rechargePeriod);
            isRecharging = false;
        }
        
        private void InsertCommand(Command command)
        {
            if (isRecharging || executionRoutine != null)
                return;
            if (commandSequence == null)
                DefineMaximumSlots();
            else if (commandSequence.Count >= initialAvailableSlots)
                return;
            commandSequence.Add(command);
            OnCommandLoaded.Invoke(command, commandSequence.Count - 1);
        }

        public void ClearLastSlot(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false || GameManager.Instance.isGamePaused)
            {
                return;
            }
            if (executionRoutine != null || commandSequence == null || commandSequence.Count == 0)
                return;
            commandSequence.Remove(commandSequence.Last());
            OnSlotCleared.Invoke(commandSequence.Count);
        } 

        private void ExecuteCommand(int executionIndex)
        {
            var command = commandSequence[executionIndex].Execute();
            OnExecution.Invoke(executionIndex);
        }

        private IEnumerator ExecuteCoroutine()
        {
            if (isRecharging || commandSequence == null || commandSequence.Count == 0)
                yield break;
            var currentExecutionIndex = 0;
            while (currentExecutionIndex < commandSequence.Count)
            {
                ExecuteCommand(currentExecutionIndex);
                yield return new WaitUntil(() => player.CanAct);
                currentExecutionIndex++;
            }
            executionRoutine = null;
            OnSequenceEnd.Invoke(SequenceFulfillmentRate);
            if (clearAlgorithmOnConclusion)
            {
                Clear();
            }
        }
        public void Execute(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false || executionRoutine != null  || GameManager.Instance.isGamePaused)
                return;
            executionRoutine = StartCoroutine(ExecuteCoroutine());
        }

        public void HaltExecution()
        {
            if (executionRoutine == null) 
                return;
            StopCoroutine(executionRoutine);
            executionRoutine = null;
            OnSequenceEnd.Invoke(SequenceFulfillmentRate);
            if (clearAlgorithmOnHalt)
            {
                Clear();
            }
        }
    }
}