using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
    [SerializeField] private Transform pointRef;

    [ContextMenu("Look at Ref")]
    public void LookAtRef()
    {
        Vector3 lookDirection = pointRef.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }
}
