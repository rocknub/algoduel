using System;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Character
{
    public class PlayerVictoryCounter : PlayerMonoBehaviour
    {
        [SerializeField] private int maxVictories;
        [field: SerializeField] public UnityEvent<int> OnRoundWon { get; private set; }
        [field: SerializeField] public IntGameEvent OnFinalVictory { get; private set; }
        
        public int CurrentVictories { private set; get; }

        private void OnDisable()
        {
            OnRoundWon.RemoveAllListeners();
            // OnFinalVictory.RemoveAll();
        }

        public void TryCountVictory(int entryIndex)
        {
            if (playerIndex != entryIndex)
            {
                return;
            }          
            CountVictory(playerIndex);
        }
        
        private void CountVictory(int entryIndex)
        {
            CurrentVictories++;
            OnRoundWon.Invoke(CurrentVictories);

            if (CurrentVictories >= maxVictories)
            {
                OnFinalVictory.Raise(entryIndex);
            }
        }        
    }
}