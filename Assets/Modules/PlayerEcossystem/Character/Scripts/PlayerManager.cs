using System;
using Coffee.UIExtensions;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerManager : MonoBehaviour
    {
        [field: SerializeField] public int PlayerIndex { get; private set; }
        [field: SerializeField] public IntGameEvent OnPlayerDamaged { get; private set; }
        [field: SerializeField] private IntGameEvent OnPlayerSuccessHit { get; set; }

        public PlayerEnvironmentDetection EnvironmentDetection { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerFire Fire { get; private set; }
        public PlayerRendering Rendering { get; private set; }
        public PlayerAnimation Animation { get; private set; }
        public PlayerVictoryCounter VictoryCounter { get; private set; }

        public bool IsInvulnerable { get; private set; }
        public bool IsPlayerReady { get; private set; }
        
        
        private void Awake()
        {
            EnvironmentDetection = GetComponent<PlayerEnvironmentDetection>();
            Rendering = GetComponent<PlayerRendering>();
            VictoryCounter = GetComponent<PlayerVictoryCounter>();
            Animation = GetComponentInChildren<PlayerAnimation>(true).SetManager<PlayerAnimation>(this);
            Movement = GetComponent<PlayerMovement>().SetManager<PlayerMovement>(this);
            Fire = GetComponent<PlayerFire>().SetManager<PlayerFire>(this);
        }

        private void OnEnable()
        {
            OnPlayerSuccessHit.AddListener(VictoryCounter.TryCountVictory);
            Movement.OnTranslation.AddListener(Animation.SetMoveAnimation);
            Movement.OnRotation.AddListener(Animation.SetMoveAnimation);
            Movement.OnFall.AddListener(Animation.SetFallAnimation);
            Movement.OnTransformReset.AddListener(Animation.ResetAnimations);
            // Fire.OnReload.AddListener(Animation.ResetAnimations);
            // Fire.OnFireStart.AddListener(Animation.SetFireAnimation);
        }

        private void OnDisable()
        {
            OnPlayerSuccessHit.RemoveListener(VictoryCounter.TryCountVictory);
            OnPlayerSuccessHit.RemoveAll();
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

        public void TryPauseGame(InputAction.CallbackContext ctx)
        {
            if (ctx.performed == false)
                return;
            GameManager.Instance.PauseGame();  
        } 

        public bool CanAct => Movement.IsActing() == false && Fire.IsActing() == false;

        public void TryConcludeAttackSuccess(Collider collider)
        {
            if (collider.transform.parent.TryGetComponent(out PlayerManager hitManager) == false)
            {
                return;
            }
            if (hitManager.PlayerIndex != PlayerIndex)
            {
                OnPlayerSuccessHit.Raise(PlayerIndex);
            }
        }

        public void TrySetPlayerReady(int entryIndex)
        {
            if (IsPlayerReady)
                return;
            IsPlayerReady = entryIndex == PlayerIndex;
        }
        
        //TODO: Colocar chamadas dependentes como callback disso
        public void TryDisable()
        {
            if (IsPlayerReady == false)
                gameObject.SetActive(false);
        }
    }
}