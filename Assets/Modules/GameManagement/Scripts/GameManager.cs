using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float freezeDuration = 2.0f;

    public void FreezeGame()
    {
        StartCoroutine(FreezeGameCoroutine());
    }
    
    private IEnumerator FreezeGameCoroutine()
    {
        float originalTimeScale = Time.timeScale;
        float elapsedTime = 0;

        while (Time.timeScale > 0)
        {
            elapsedTime += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(originalTimeScale, 0, elapsedTime / freezeDuration);
            yield return null;
        }
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
