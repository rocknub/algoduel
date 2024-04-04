﻿using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Character
{
    public class PlayerMovement : MonoBehaviour
    {
        //TODO(Marlus - Maybe) Create a grid to calculate distance
        [Header("Directed Movements")]
        [SerializeField] private float translationDistance;
        [SerializeField] private float defaultMotionDuration;
        [SerializeField] private Ease motionEase;
        [SerializeField] private float rotationAngle = 90f;
        public UnityEvent OnTransformReset;
        [Header("Uncontrolled Movements")]
        [SerializeField] private Ease fallEase;
        [SerializeField] private float fallDuration;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;

        private Tween motionTween;
        private Vector3 targetPosition;
        private PlayerEnvironmentDetection envDetection;
        
        public bool IsActing => motionTween is { active: true };

        private void OnValidate()
        {
            UpdateTargetPosition();
        }

        private void Start()
        {
            envDetection = GetComponent<PlayerEnvironmentDetection>();
            UpdateTargetPosition();
        }

        public void Move()
        {
            if (IsActing)
            {
                return;
            }

            if (envDetection != null)
            {
                if (envDetection.CheckForObstacles(targetPosition))
                {
                    Debug.Log("Target up ahead!");
                    targetPosition = transform.position;
                }
            }
            motionTween = transform.DOMove(targetPosition, defaultMotionDuration).SetEase(motionEase);
            motionTween.OnComplete(ActionCompletionCallback);
        }

        public void RotateClockwise()
        {
            if (IsActing)
            {
                return;
            }
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y += rotationAngle;
            motionTween = transform.DORotate(targetRotation, defaultMotionDuration).SetEase(motionEase);
            motionTween.OnComplete(ActionCompletionCallback);
        }
        
        public void RotateCounterClockwise()
        {
            if (IsActing)
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
            if (envDetection == null)
                return;
            if (envDetection.IsAboveGround())
                return;
            motionTween = transform.DOMoveY(transform.position.y - 20f, fallDuration);
            motionTween.OnComplete(ResetTransform);
        }

        public Vector3 UpdateTargetPosition()
        {
            targetPosition = transform.position + transform.forward * translationDistance;
            return targetPosition;
        }

        public void ResetTransform()
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            UpdateTargetPosition();
            OnTransformReset.Invoke();
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