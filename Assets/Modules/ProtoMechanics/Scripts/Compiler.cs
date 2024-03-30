using System.Collections.Generic;
using UnityEngine;

namespace Modules.ProtoMechanics.Scripts
{
    public class Compiler : MonoBehaviour
    {
        [SerializeField] private int maxCommands = 10;
        [SerializeField] private Transform commandsParent;
        // [SerializeField] private 
        
        private Queue<Command> commandSequence;

        private void SetCommandCallbacks()
        {
            Command command;
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
        }

        private void ExecuteNextCommand()
        {
            commandSequence.Dequeue().Execute();
        }
    }
}