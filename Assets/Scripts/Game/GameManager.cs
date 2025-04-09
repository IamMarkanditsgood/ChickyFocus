using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource[] _music;
    [Header("Obstacle Prefab Groups")]
    [SerializeField] private GameObject[] obstacleLeftPrefabs;
    [SerializeField] private GameObject[] obstacleRightPrefabs;
    [SerializeField] private GameObject[] obstacleCenterPrefabs;
    [SerializeField] private GameObject[] obstacleRandomPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float distanceAhead = 10f;
    [SerializeField] private float xOffset = 2f;
    [SerializeField] private Transform player;

    private float timer;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Sounds") == 1)
        {
            foreach (var music in _music)
            {
                music.Play();
            }
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnTwoDifferentObstacles();
        }
    }

    private void SpawnTwoDifferentObstacles()
    {
        // Вибираємо 2 різні типи перешкод (0..3)
        int firstType = Random.Range(0, 4);
        int secondType;
        do
        {
            secondType = Random.Range(0, 4);
        } while (secondType == firstType);

        // Спавнимо по одному з кожного типу
        SpawnObstacleByType(firstType);
        SpawnObstacleByType(secondType);
        SpawnObstacleByType(3);
    }

    private void SpawnObstacleByType(int type)
    {
        GameObject prefab = null;
        Vector3 spawnPos = Vector3.zero;

        switch (type)
        {
            case 0: // Ліворуч
                prefab = GetRandomPrefab(obstacleLeftPrefabs);
                spawnPos = new Vector3(-xOffset, player.position.y + distanceAhead, 0f);
                break;

            case 1: // Праворуч
                prefab = GetRandomPrefab(obstacleRightPrefabs);
                spawnPos = new Vector3(xOffset, player.position.y + distanceAhead, 0f);
                break;

            case 2: // Центр
                prefab = GetRandomPrefab(obstacleCenterPrefabs);
                spawnPos = new Vector3(0f, player.position.y + distanceAhead, 0f);
                break;

            case 3: // Рандомна позиція
                prefab = GetRandomPrefab(obstacleRandomPrefabs);
                float randX = Random.Range(-xOffset, xOffset);
                spawnPos = new Vector3(randX, player.position.y + distanceAhead, 0f);
                break;
        }

        if (prefab != null)
        {
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    private GameObject GetRandomPrefab(GameObject[] prefabs)
    {
        if (prefabs.Length == 0) return null;
        return prefabs[Random.Range(0, prefabs.Length)];
    }
}
