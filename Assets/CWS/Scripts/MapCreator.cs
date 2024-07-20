using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelManager;
using Random = UnityEngine.Random;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private int[] roomCodeQueue = new int[20];

    private int mapSize;
    private bool isValidMap = false;
    private int validRoomCount = 0;

    private List<List<List<int>>> map = new List<List<List<int>>>();
    private List<int> roomRandomizeQueue = new List<int>();

    public int[,,] direction = new int[,,]
    {
        {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 },
            { -1, 0, 0 },
            { 0, -1, 0 },
            { 0, 0, -1 }
        }
    };
    
    /* -1: ������
        0: ����
        1: ��������
        2: ��ȭ
        3: ����
        4: �߰�
        5: �����
        6: �ؾ�
        7: Ư��   */

    public void GetRandomMap(int _mapSize, Vector3Int _origin, Vector3Int _goal)
    {
        mapSize = _mapSize;
        
        while (!isValidMap)
        {
            // ���� -1�� �ʱ�ȭ
            ResetMap();
            validRoomCount = 0;

            // ����(2), ��������(1) ����
            SetOriginRoom(_origin);
            SetGoalRoom(_goal);

            // �� ���� ���� �� Ÿ�缺 Ȯ��
            RandomizeMap();
            ValidationMap();
        }
        
        Debug.Log($"Map Randomized (Map size: {_mapSize}, Room count: {(int)Mathf.Pow(_mapSize, 3)})");
    }

    private void SetOriginRoom(Vector3Int _origin)
    {
        // ������ ��ȭ ������ ���� = 2
        LevelManager.Instance.levelMap[_origin.x][_origin.y][_origin.z] = 2;
        Debug.Log($"Set Origin to 1. (Origin: [{_origin.x}, {_origin.y}, {_origin.z}])");
    }

    private void SetGoalRoom(Vector3Int _goal)
    {
        // ���� ���� ���� = 1
        LevelManager.Instance.levelMap[_goal.x][_goal.y][_goal.z] = 1;
        Debug.Log(($"Set goal point to 0. (Goal point: [{_goal.x}][{_goal.y}][{_goal.z}])"));
    }

    private void RandomizeMap()
    {
        // �������� �� ���� ����
        for (int i0 = 0; i0 < mapSize; i0++)
        {
            for (int i1 = 0; i1 < mapSize; i1++)
            {
                for (int i2 = 0; i2 < mapSize; i2++)
                {
                    if (LevelManager.Instance.levelMap[i0][i1][i2] == -1)
                        LevelManager.Instance.levelMap[i0][i1][i2] = GetRandomRoom();

                    if (LevelManager.Instance.levelMap[i0][i1][i2] != 0)
                        validRoomCount++;
                }
            }
        }
    }

    private void ResetMap()
    {
        // -1�� �� �ʱ�ȭ
        for (int i0 = 0; i0 < mapSize; i0++)
        {
            for (int i1 = 0; i1 < mapSize; i1++)
            {
                for (int i2 = 0; i2 < mapSize; i2++)
                {
                    LevelManager.Instance.levelMap[i0][i1][i2] = -1;
                }
            }
        }
    }

    private void ValidationMap()
    {
        // BFS�� �̿��Ͽ� ���� �������� �� �� �ִ��� Ž��

        bool[,,] checkRoad = null;  // �鸥 ������� Ȯ��

        // checkRoad false�� �ʱ�ȭ
        checkRoad = new bool[mapSize, mapSize, mapSize];
        for (int i0 = 0; i0 < mapSize; i0++)
            for (int i1 = 0; i1 < mapSize; i1++)
                for (int i2 = 0; i2 < mapSize; i2++)
                    checkRoad[i0, i1, i2] = false;


    }

    private int GetRandomRoom()
    {
        // ���� ť�� ��������� �������� ����
        if (roomRandomizeQueue.Count <= 0)
            ResetRandomizeQueue();
        
        // �� ť�κ��� ������ �� �̱�
        int randomIndex = Random.Range(0, roomRandomizeQueue.Count);
        int result = roomRandomizeQueue[randomIndex];
        roomRandomizeQueue.RemoveAt(randomIndex);

        return result;
    }

    private void ResetRandomizeQueue()
    {
        // �� ť ����
        for (int i = 0; i < roomCodeQueue.Length;i++)
            roomRandomizeQueue.Add(roomCodeQueue[i]);
    }
}
