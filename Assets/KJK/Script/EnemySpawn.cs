using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform centerPosition;
    public float spawnInterval = 1.0f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0.1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Generate a random position on the surface of a 3x3x3 cube centered at the centerPosition
        Vector3 spawnPosition = GetRandomPositionOnCubeSurface();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
} 
