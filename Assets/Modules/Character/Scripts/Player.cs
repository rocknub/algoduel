﻿using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        //TODO(Marlus - Maybe) Create a grid to calculate distance
        public float translationDistance;
        public float motionDuration;
        public Ease motionEase;
        public float rotationAngle = 90f;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;

        private Tween motionTween;
        private Vector3 targetPosition;
        
        public bool IsActing => motionTween is { active: true };

        private void OnValidate()
        {
            UpdateTargetPosition();
        }

        public void Move()
        {
            if (IsActing)
            {
                return;
            }
            motionTween = transform.DOMove(targetPosition, motionDuration).SetEase(motionEase);
            motionTween.OnComplete(() => UpdateTargetPosition());
        }

        public void RotateClockwise()
        {
            if (IsActing)
            {
                return;
            }
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y += rotationAngle;
            motionTween = transform.DORotate(targetRotation, motionDuration).SetEase(motionEase);
            motionTween.OnComplete(() => UpdateTargetPosition());
        }
        
        public void RotateCounterClockwise()
        {
            if (IsActing)
            {
                return;
            }
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y -= rotationAngle;
            motionTween = transform.DORotate(targetRotation, motionDuration).SetEase(motionEase);
            motionTween.OnComplete(() => UpdateTargetPosition());        }

        public Vector3 UpdateTargetPosition()
        {
            targetPosition = transform.position + transform.forward * translationDistance;
            return targetPosition;
        }

        public void GoToOrigin()
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            UpdateTargetPosition();
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