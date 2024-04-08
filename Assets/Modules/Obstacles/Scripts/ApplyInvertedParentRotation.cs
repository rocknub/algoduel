using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyInvertedParentRotation : SingleTaskComponent
{
    [ContextMenu("DoIt")]
    public override void DoIt()
    {
        transform.rotation = Quaternion.Inverse(transform.parent.rotation);
    }
}
