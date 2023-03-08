using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private enum Behavior
    {
        LoadLevel,
        QuitGame
    }
    
    [SerializeField] private Behavior behavior;
    [SerializeField] private string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chicken"))
        {
            switch (behavior)
            {
                case Behavior.LoadLevel:
                    SceneManager.LoadScene(levelName);
                    break;
                case Behavior.QuitGame:
                    Application.Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
