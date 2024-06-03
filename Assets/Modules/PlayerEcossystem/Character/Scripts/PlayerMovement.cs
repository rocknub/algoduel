using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Character
{
    public class PlayerMovement : PlayerBehaviour
    {
        //TODO(Marlus - Maybe) Create a grid to calculate distance
        [Header("Directed Movements")]
        [SerializeField] private float translationDistanceDefault;
        [SerializeField] private float translationDistanceAlternate;
        [SerializeField] private float defaultMotionDuration;
        [SerializeField] private Ease motionEase;
        [FormerlySerializedAs("rotationAngle")] [SerializeField] private float defaultRotationAngle = 90f;
        [Header("Uncontrolled Movements")]
        [SerializeField] private Ease fallEase;
        [SerializeField] private float fallDuration;
        [SerializeField] private bool useRandomRespawn;
        [SerializeField] private Vector3[] randomRespawnOffsets;
        [Header("Events")]
        [FormerlySerializedAs("OnTransformReset")] [SerializeField] private UnityEvent onTransformReset;
        [SerializeField] private UnityEvent<MovementData> onTranslation;
        [SerializeField] private UnityEvent<MovementData> onRotation;
        [SerializeField] private UnityEvent onFall;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private CardinalDirection debugDirection;

        private Tween motionTween;
        private Vector3 targetPosition;
        private PlayerEnvironmentDetection envDetection => manager.EnvironmentDetection;
        
        private void OnValidate()
        {
            UpdateTargetPosition();
        }

        private void Start()
        {
            UpdateTargetPosition();
        }
        
        public override bool IsActing() => motionTween is { active: true };

        [ContextMenu("Debug Coordinates")]
        private void DebugCoordinates()
        {
            var camera = Camera.main;
            var camForward = debugDirection switch
            {
                CardinalDirection.Up => camera.transform.forward,
                CardinalDirection.Down => -camera.transform.forward,
                CardinalDirection.Left => -camera.transform.right,
                CardinalDirection.Right => camera.transform.right,
                _ => throw new ArgumentOutOfRangeException()
            };
            camForward.y = 0f;
            var projectedDirection = camForward.normalized;
            var signedAngle = Vector3.SignedAngle(transform.forward, projectedDirection, Vector3.down);
            Debug.Log(signedAngle);
        }

        public float GetCameraRelativeRotation(CardinalDirection direction)
        {
            var camera = GameManager.Instance.Camera;
            var camDirection = direction switch
            {
                CardinalDirection.Up => camera.transform.forward,
                CardinalDirection.Down => -camera.transform.forward,
                CardinalDirection.Left => -camera.transform.right,
                CardinalDirection.Right => camera.transform.right,
                _ => throw new ArgumentOutOfRangeException()
            };
            camDirection.y = 0f;
            return Vector3.SignedAngle(transform.forward, camDirection.normalized,Vector3.up);
        }
        
        public void MoveRelativeToCamera(CardinalDirection direction)
        {
            float angle = GetCameraRelativeRotation(direction);
            if (Mathf.Approximately(angle, 0.0f))
            {
                MoveForward();
            }
            else
            {
                Rotate(angle, defaultMotionDuration, true);
            }
        }
        public void MoveForwardRelativeToCamera() => MoveRelativeToCamera(CardinalDirection.Up);
        public void MoveRightRelativeToCamera() => MoveRelativeToCamera(CardinalDirection.Right);
        public void MoveBackwardRelativeToCamera() => MoveRelativeToCamera(CardinalDirection.Down);
        public void MoveLeftRelativeToCamera() => MoveRelativeToCamera(CardinalDirection.Left);

        public void MoveForward()
        {
            if (IsActing())
            {
                return;
            }

            if (envDetection != null)
            {
                if (envDetection.CheckForObstacles(targetPosition))
                {
                    targetPosition = transform.position;
                }
            }
            motionTween = transform.DOMove(targetPosition, defaultMotionDuration).SetEase(motionEase);
            motionTween.OnComplete(ActionCompletionCallback);
            onTranslation.Invoke(new MovementData(transform.position, targetPosition, defaultMotionDuration));
        }

        public void RotateClockwise() => Rotate(defaultRotationAngle, defaultMotionDuration, true);

        public void Rotate(float angle, float duration, bool doTriggerCallback)
        {
            if (IsActing())
            {
                return;
            }
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y += angle;
            motionTween = transform.DORotate(targetRotation, duration).SetEase(motionEase);
            onRotation.Invoke(new MovementData(transform.rotation.eulerAngles, targetRotation, duration));
            if (doTriggerCallback == false)
                return;
            motionTween.OnComplete(ActionCompletionCallback);
        }
        
        public void RotateCounterClockwise() => Rotate(-defaultRotationAngle, defaultMotionDuration, true);

        private void ActionCompletionCallback()
        {
            TryToFall();
            UpdateTargetPosition();
        }

        public void TryToFall()
        {
            if (envDetection.IsAboveGround())
                return;
            onFall.Invoke();
            motionTween = transform.DOMoveY(transform.position.y - 20f, fallDuration);
            motionTween.OnComplete(manager.ReceiveHit);
        }

        [ContextMenu("Update Target Position")]
        public Vector3 UpdateTargetPosition()
        {
            float translationDistance = IsHorizontallyDirected() ? translationDistanceAlternate : translationDistanceDefault;
            targetPosition = transform.position + transform.forward * translationDistance;
            return targetPosition;
        }

        public bool IsHorizontallyDirected()
        {
            float absoluteRotation = Mathf.Abs(transform.localRotation.eulerAngles.y);
            return Mathf.Approximately(absoluteRotation, 90) || Mathf.Approximately(absoluteRotation, 270);
        }

        public void ResetTransform()
        {
            motionTween?.Kill();
            var respawnPosition = Vector3.zero;
            if (useRandomRespawn)
            {
                respawnPosition = randomRespawnOffsets[Random.Range(0, randomRespawnOffsets.Length)];
            }
            transform.SetLocalPositionAndRotation(respawnPosition, Quaternion.identity);
            UpdateTargetPosition();
            onTransformReset.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            if (showGizmos == false)
                return;
            Gizmos.color = Color.red;
            Vector3 TargetPosition = targetPosition;
            Gizmos.DrawLine(TargetPosition, TargetPosition + Vector3.up * 100f);
        }
    }
}