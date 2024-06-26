﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Character
{
    public class PlayerMovement : PlayerBehaviour, IPlayerActor
    {
        //TODO(Marlus - Maybe) Create a grid to calculate distance
        [Header("Directed Movements")]
        [SerializeField] private float translationDistanceDefault;
        [SerializeField] private float translationDistanceAlternate;
        [SerializeField] private float defaultMotionDuration;
        [SerializeField] private Ease motionEase;
        [SerializeField] private float defaultRotationAngle = 90f;
        [Header("Fall and Landing")]
        [SerializeField] private Ease fallEase;
        [SerializeField] private float fallInterval;
        [SerializeField] private float fallDuration;
        [SerializeField] private float afterFallHeight;
        [SerializeField] private float landingDuration;
        [SerializeField] private Ease landEase;
        [Header("Respawn Movements")]
        [SerializeField] private bool useRandomRespawn;
        [SerializeField] private Vector3[] randomRespawnOffsets;
        [Header("Events")]
        [SerializeField] private UnityEvent onTransformReset;
        [SerializeField] private UnityEvent<MovementData> onTranslation;
        [SerializeField] private UnityEvent<MovementData> onRotation;
        [SerializeField] private UnityEvent onTrip;
        [SerializeField] private UnityEvent onFall;
        [SerializeField] private UnityEvent onLand;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private CardinalDirection debugDirection;

        private Tween motionTween;
        private Vector3 targetPosition;
        private List<Vector3> respawnList;
        private PlayerEnvironmentDetection envDetection => manager.EnvironmentDetection;
        
        public UnityEvent<MovementData> OnTranslation => onTranslation;
        public UnityEvent<MovementData> OnRotation => onRotation;
        public UnityEvent OnFall => onFall;
        public UnityEvent OnTransformReset => onTransformReset;

        private void OnValidate()
        {
            UpdateTargetPosition();
        }

        private void Start()
        {
            UpdateTargetPosition();
        }
        
        public bool IsActing() => motionTween is { active: true };

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
            StartCoroutine(FallRoutine());
        }
        public IEnumerator FallRoutine()
        {
            motionTween = transform.DOMoveY(transform.position.y - 20f, fallDuration).SetEase(fallEase).Pause();
            motionTween.OnComplete(LandRoutine);
            onTrip.Invoke();
            yield return new WaitForSeconds(fallInterval);
            motionTween.Play();
            onFall.Invoke();
        }

        public void LandRoutine()
        {
            ResetTransform();
            var originalPosition = transform.position;
            var landingStartPos = originalPosition;
            landingStartPos.y += afterFallHeight;
            transform.position = landingStartPos;
            motionTween = transform.DOMoveY(originalPosition.y, landingDuration).SetEase(landEase);
            motionTween.OnComplete(onLand.Invoke);
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
                if (respawnList == null || respawnList.Count == 0)
                {
                    respawnList = randomRespawnOffsets.ToList();
                }
                respawnPosition = respawnList[Random.Range(0, respawnList.Count)];
                respawnList.Remove(respawnPosition);
            }
            transform.SetLocalPositionAndRotation(respawnPosition, Quaternion.identity);
            UpdateTargetPosition();
            onTransformReset.Invoke();
        }

        private void OnDisable()
        { 
            onTransformReset.RemoveAllListeners();
            onTranslation.RemoveAllListeners();
            onRotation.RemoveAllListeners();
            onFall.RemoveAllListeners();        
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