using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoSingleton<GameManager>
{
    public float freezeDuration = 2.0f;
    public float pauseThreshold = 0.01f;
    public bool isGamePaused => Time.timeScale < pauseThreshold;
    [SerializeField] private BoolGameEvent onGamePauseToggle;

    private Coroutine endGameRoutine;
    private Camera camera;

    public Camera Camera
    {
        get
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            return camera;
        }
    }

    public void EndGame()
    {
        endGameRoutine = StartCoroutine(FreezeGameCoroutine());
    }
    
    private IEnumerator FreezeGameCoroutine()
    {
        float originalTimeScale = Time.timeScale;
        float elapsedTime = 0;
        
        while (Time.timeScale > pauseThreshold)
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

    public void PauseGame()
    {
        if (endGameRoutine != null)
            return;
        Time.timeScale = isGamePaused ? 1.0f : 0.0f;
        onGamePauseToggle.Raise(isGamePaused);
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
