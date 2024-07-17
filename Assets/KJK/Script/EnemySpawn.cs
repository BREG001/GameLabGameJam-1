using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject epicEnemyPrefab;
    public Transform centerPosition;
    public float spawnInterval = 1.0f;
    public float epicSpawnTiming;
    public float stageInterval = 15.0f;
    [SerializeField] private float _minSpawnInterval = 0.5f;

    void Start()
    {
        spawnInterval -= (Constants.LEVEL_ENEMY_SPAWNTIME * GameSceneManager.GameLevel);
        epicSpawnTiming = stageInterval / 2;
        Invoke(nameof(SpawnEpicEnemy), epicSpawnTiming);
        StartCoroutine(StageStart());

        GameSceneManager.Instance.GameSceneEvent.WarningSignal += OnWarningSignalStart;
        GameSceneManager.Instance.GameSceneEvent.GameOver += OnGameOver;
        GameSceneManager.Instance.GameSceneEvent.GameResume += OnGameResume;
    }
    private void OnDestroy()
    {
        GameSceneManager.Instance.GameSceneEvent.WarningSignal -= OnWarningSignalStart;
        GameSceneManager.Instance.GameSceneEvent.GameOver -= OnGameOver;
        GameSceneManager.Instance.GameSceneEvent.GameResume -= OnGameResume;
    }

    private void OnWarningSignalStart(GameSceneEventArgs gameSceneEventArgs)
    {
        CancelInvoke(nameof(SpawnEpicEnemy));
        CancelInvoke(nameof(SpawnEnemy));
    }
    private void OnGameOver(GameSceneEventArgs gameSceneEventArgs)
    {
        CancelInvoke(nameof(SpawnEpicEnemy));
        CancelInvoke(nameof(SpawnEnemy));
    }
    private void OnGameResume(GameSceneEventArgs gameSceneEventArgs)
    {
        spawnInterval -= (Constants.LEVEL_ENEMY_SPAWNTIME * GameSceneManager.GameLevel);
        spawnInterval = Math.Max(spawnInterval, _minSpawnInterval);
        Invoke(nameof(SpawnEpicEnemy), epicSpawnTiming);
        StartCoroutine(StageStart());
    }

    void SpawnEnemy()
    {
        // Generate a random position on the surface of a 3x3x3 cube centered at the centerPosition
        Vector3 spawnPosition = GetRandomPositionOnCubeSurface();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    void SpawnEpicEnemy()
    {
        Vector3 spawnPosition = GetRandomPositionOnCubeSurface();
        Instantiate(epicEnemyPrefab, spawnPosition, Quaternion.identity);
    }
    Vector3 GetRandomPositionOnCubeSurface()
    {
        // Cube dimensions
        float halfSize = 10f; // Since the size of the cube is 3x3x3, the half size is 3 / 2 = 1.5

        // Randomly choose a face of the cube
        int face = Random.Range(0, 6);

        Vector3 position = centerPosition.position;

        switch (face)
        {
            case 0: // Front face
                position += new Vector3(Random.Range(-halfSize, halfSize), Random.Range(-halfSize, halfSize), halfSize);
                break;
            case 1: // Back face
                position += new Vector3(Random.Range(-halfSize, halfSize), Random.Range(-halfSize, halfSize), -halfSize);
                break;
            case 2: // Left face
                position += new Vector3(-halfSize, Random.Range(-halfSize, halfSize), Random.Range(-halfSize, halfSize));
                break;
            case 3: // Right face
                position += new Vector3(halfSize, Random.Range(-halfSize, halfSize), Random.Range(-halfSize, halfSize));
                break;
            case 4: // Top face
                position += new Vector3(Random.Range(-halfSize, halfSize), halfSize, Random.Range(-halfSize, halfSize));
                break;
            case 5: // Bottom face
                position += new Vector3(Random.Range(-halfSize, halfSize), -halfSize, Random.Range(-halfSize, halfSize));
                break;
        }

        return position;
    }

    void OnDrawGizmos()
    {
        // Draw a 3x3x3 cube centered at the centerPosition
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(20, 20, 20);
        Gizmos.DrawWireCube(centerPosition.position, size);
    }

    IEnumerator StageStart()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0.1f, spawnInterval);
        yield return null;
    }
}
