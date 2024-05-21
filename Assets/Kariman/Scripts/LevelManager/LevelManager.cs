using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] arenas;
    [SerializeField] Transform[] playerNewPositions;
    [SerializeField] Transform playerCurrentPos;
    [SerializeField] GameEvent levelCompleted;

    int currentArena;
    [SerializeField] CharacterController characterController;

    private void Start()
    {
        currentArena = 0;
    }

    private void OnEnable()
    {
        levelCompleted.GameAction += ChangeLevel;
    }

    private void OnDisable()
    {
        levelCompleted.GameAction -= ChangeLevel;
    }

    void ChangeLevel()
    {
        for (int i = 0; i < arenas.Length; i++)
        {
            if (arenas[i].activeSelf)
            {
                arenas[i].SetActive(false);
            }
        }
        if (currentArena < arenas.Length - 1)
        {
            currentArena++;
        }
        // Activate the current arena
        if (currentArena < arenas.Length)
        {
            arenas[currentArena].SetActive(true);
            if (currentArena - 1 < playerNewPositions.Length)
            {
                if (characterController != null)
                {
                    characterController.enabled = false;
                    playerCurrentPos.position = playerNewPositions[currentArena - 1].position;
                    characterController.enabled = true;
                }
                else
                {
                    playerCurrentPos.position = playerNewPositions[currentArena - 1].position;
                }
            }
        }
    }
}