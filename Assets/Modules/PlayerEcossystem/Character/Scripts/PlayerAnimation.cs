using System;
using System.Collections;
using Character;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : PlayerBehaviour
{
    public Animator animator;
    [SerializeField] private string moveAnimName;
    [SerializeField] private string fallAnimName;
    [SerializeField] private string fireAnimName;
    [Header("Events")]
    [SerializeField] private UnityEvent onFire;
    [SerializeField] private UnityEvent onBeginFireAnimation;
    [SerializeField] public UnityEvent onEndFireAnimation;

    private Coroutine moveRoutine;

    public void SetMoveAnimation(MovementData movementData)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveAnimationRoutine(movementData));   
    }
    public IEnumerator MoveAnimationRoutine(MovementData movementData)
    {
        animator.SetBool(moveAnimName, true);
        yield return new WaitForSeconds(movementData.duration);
        animator.SetBool(moveAnimName, false);
    }

    public void SetFallAnimation()
    {
        animator.SetBool(fallAnimName, true);
    }

    public void StartFireAnimation()
    {
        onBeginFireAnimation.Invoke();
        animator.SetBool(fireAnimName, true);
    }

    public void ResetAnimations()
    {
        animator.SetBool(moveAnimName, false);
        animator.SetBool(fallAnimName, false);
        animator.SetBool(fireAnimName, false);
    }
    
    public UnityEvent OnFire => onFire;

    public void Fire()
    {
        onFire.Invoke();
    }
}