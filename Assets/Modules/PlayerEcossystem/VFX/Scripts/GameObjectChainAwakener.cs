using System;
using System.Collections;
using Character;
using UnityEngine;

public class GameObjectChainAwakener : PlayerMonoBehaviour
{
    [SerializeField] private bool startDisabled;
    [SerializeField] private GameObject[] gameObjects;
    [SerializeField] private float[] delayTimes;

    private void Awake()
    {
        if (startDisabled)
        {
            Array.ForEach(gameObjects, go => go.SetActive(false));
        }
    }

    private IEnumerator ChainAwaken()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            yield return new WaitForSecondsRealtime(delayTimes[i]);
            gameObjects[i].SetActive(true);
        }
    }

    public void Execute() => StartCoroutine(ChainAwaken());

    public void TryExecute(int index)
    {
        if (index != playerIndex)
            return;
        Execute();
    }

    [ContextMenu("Get All Children")]
    private void GetAllChildren()
    {
        gameObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            gameObjects[i] = transform.GetChild(i).gameObject;
        }  
    } 
}
