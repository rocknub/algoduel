using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
        [SerializeField] private float rotationAngle = 90f;
        [Header("Uncontrolled Movements")]
        [SerializeField] private Ease fallEase;
        [SerializeField] private float fallDuration;
        [Header("Events")]
        [FormerlySerializedAs("OnTransformReset")] [SerializeField] private UnityEvent onTransformReset;
        [SerializeField] private UnityEvent<MovementData> onTranslation;
        [SerializeField] private UnityEvent<MovementData> onRotation;
        [SerializeField] private UnityEvent onFall;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;

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

        public void Move()
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

        public void RotateClockwise()
        {
            if (IsActing())
            {
                return;
            }
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y += rotationAngle;
            motionTween = transform.DORotate(targetRotation, defaultMotionDuration).SetEase(motionEase);
            motionTween.OnComplete(ActionCompletionCallback);
            onRotation.Invoke(new MovementData(transform.rotation.eulerAngles, targetRotation, defaultMotionDuration));
        }
        
        public void RotateCounterClockwise()
        {
            if (IsActing())
            {
                return;
            }
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y -= rotationAngle;
            motionTween = transform.DORotate(targetRotation, defaultMotionDuration).SetEase(motionEase);
            motionTween.OnComplete(ActionCompletionCallback);

        }

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
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
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