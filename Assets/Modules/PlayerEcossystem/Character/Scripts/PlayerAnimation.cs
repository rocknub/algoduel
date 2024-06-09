using System.Collections;
using Character;
using UnityEngine;

public class PlayerAnimation : PlayerBehaviour
{
    public Animator animator;
    [SerializeField] private string moveAnimName;
    [SerializeField] private string fallAnimName;
    [SerializeField] private string fireAnimName;

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

    public void SetFireAnimation()
    {
        animator.SetBool(fireAnimName, true);
    }

    public void ResetAnimations()
    {
        animator.SetBool(moveAnimName, false);
        animator.SetBool(fallAnimName, false);
        animator.SetBool(fireAnimName, false);
    }
}