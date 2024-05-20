using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        while (Time.timeScale > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(originalTimeScale, 0, elapsedTime / freezeDuration);
            yield return null;
        }
        ResetGame();
    }

    public void ResetGame()
    {
        Time.timeScale = 1.0f;
        DOTween.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
