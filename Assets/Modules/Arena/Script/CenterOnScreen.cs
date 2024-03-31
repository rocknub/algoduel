using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOnScreen : MonoBehaviour
{
    [ContextMenu("DoIt")]
    public void DoIt()
    {
        Camera cam = Camera.main;
        float distanceToCamera = (transform.position - cam.transform.position).magnitude;
        Vector3 screenCenter =
            cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, distanceToCamera));
        transform.position = screenCenter;
    }
}
