using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDisabler : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;

    public void DisableThem(bool value)
    {
        Array.ForEach(targets, t => t.SetActive(!value));
    }
}