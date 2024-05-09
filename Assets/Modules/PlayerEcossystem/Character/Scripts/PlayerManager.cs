using ScriptableObjectArchitecture;
using UnityEngine;

namespace Character
{
    public class PlayerManager : MonoBehaviour
    {
        [field: SerializeField] public int PlayerIndex { get; private set; }
        [field: SerializeField] public IntGameEvent OnPlayerDamaged { get; private set; }
        
        public PlayerEnvironmentDetection EnvironmentDetection { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerFire Fire { get; private set; }
        public PlayerRendering Rendering { get; private set; }
        public PlayerVictoryCounter VictoryCounter { get; private set; }
        
        public bool IsInvulnerable { get; private set; }
        
        
        private void Awake()
        {
            EnvironmentDetection = GetComponent<PlayerEnvironmentDetection>();
            Rendering = GetComponent<PlayerRendering>();
            VictoryCounter = GetComponent<PlayerVictoryCounter>();
            OnPlayerDamaged.AddListener(VictoryCounter.TryCountVictory);
            Movement = GetComponent<PlayerMovement>().SetManager(this) as PlayerMovement;
            Fire = GetComponent<PlayerFire>().SetManager(this) as PlayerFire;
        }
        
        public void ReceiveHit()
        {
            Movement.ResetTransform();
            OnPlayerDamaged.Raise(PlayerIndex);
            if (IsInvulnerable)
                return;
            IsInvulnerable = true;
            Rendering.TweenMaterials(() => IsInvulnerable = false);
        }
        
        public bool CanAct => Movement.IsActing() == false && Fire.IsActing() == false;
    }
}