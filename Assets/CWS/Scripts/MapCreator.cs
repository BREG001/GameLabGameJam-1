using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelManager;
using Random = UnityEngine.Random;

public class MapCreator : MonoBehaviour
{
    public MapCreateEventArgs MapCreateEvent;

    [SerializeField] private int[] roomCodeQueue = new int[20];

    private int mapSize;
    private bool isValidMap = false;

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

            // ����(2), ��������(1) ����
            SetOriginRoom(_origin);
            SetGoalRoom(_goal);

            // �� ���� ���� �� Ÿ�缺 Ȯ��
            RandomizeMap();
            ValidationMap(_origin, _goal);
        }
        
        MapCreateEvent.CallMapCreateComplete();
        Debug.Log($"Map Creator: Map Randomize Completed  (Map size: {_mapSize}, Room count: {(int)Mathf.Pow(_mapSize, 3)})");
    }

    public bool CheckMapCreation()
    {
        return isValidMap;
    }

    private void SetOriginRoom(Vector3Int _origin)
    {
        // ������ ��ȭ ������ ���� = 2
        LevelManager.Instance.levelMap[_origin.x][_origin.y][_origin.z] = 2;
        Debug.Log($"Map Creator: Set Origin to 1. (Origin: [{_origin.x}, {_origin.y}, {_origin.z}])");
    }

    private void SetGoalRoom(Vector3Int _goal)
    {
        // ���� ���� ���� = 1
        LevelManager.Instance.levelMap[_goal.x][_goal.y][_goal.z] = 1;
        Debug.Log(($"Map Creator: Set goal point to 0. (Goal point: [{_goal.x}][{_goal.y}][{_goal.z}])"));
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
                }
            }
        }
        Debug.Log("Map Creator: Randomize Map Code");
    }

    private void ResetMap()
    {
        Debug.Log("Map Creator: Reset the map to -1");
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
        for (int i = 0; i < roomCodeQueue.Length; i++)
            roomRandomizeQueue.Add(roomCodeQueue[i]);
    }

    private void ValidationMap(Vector3Int _origin, Vector3 _goal)
    {
        // BFS�� �̿��Ͽ� ���� �������� �� �� �ִ��� Ž��

        bool[,,] checkRoad = null;  // �鸥 ������� Ȯ��

        // checkRoad false�� �ʱ�ȭ
        checkRoad = new bool[mapSize, mapSize, mapSize];
        for (int i0 = 0; i0 < mapSize; i0++)
            for (int i1 = 0; i1 < mapSize; i1++)
                for (int i2 = 0; i2 < mapSize; i2++)
                    checkRoad[i0, i1, i2] = false;

        BFSNode bestNode = null;

        Queue<BFSNode> queue = new Queue<BFSNode>();
        queue.Enqueue(new BFSNode(_origin.x, _origin.y, _origin.z, null));
        checkRoad[_origin.x, _origin.y, _origin.z] = true;

        // Ž���� ���� ���� ������ ����
        while (queue.Count > 0)
        {
            // ��带 ������
            BFSNode node = queue.Dequeue();

            // ��ǥ ������ ���� ��
            if (node.X == _goal.x && node.Y == _goal.y && node.Z == _goal.z)
            {
                isValidMap = true;

                break;
            }

            for (int i = 0; i < direction.GetLength(1); i++)
            {
                // ��� �������� ��� Ž��
                int dx = node.X + direction[0, i, 0];
                int dy = node.Y + direction[0, i, 1];
                int dz = node.Z + direction[0, i, 2];

                // ��尡 �� ���� ����, ��尡 �� �� �ִ� ���, �� ���� �� �� ���� ����� ���
                if (CheckMapRange(dx, dy, dz) && CheckMapWay(dx, dy, dz) && !checkRoad[dx, dy, dz])
                {
                    // ã�� �濡 ���ؼ� ��带 ����� Queue�� �߰�, ���� ���� ã�� ����� ���� ���
                    BFSNode searchNode = new BFSNode(dx, dy, dz, node);
                    queue.Enqueue(searchNode);

                    // �̹� �鸥 ���� üũ
                    checkRoad[dx, dy, dz] = true;
                }
            }
        }

        if (isValidMap)
            Debug.Log("Map Creator: Valid Map Generated.");
        else
            Debug.Log("Map Creator: Invalid Map. Regenerate Random Map.");

        /*if (isValidMap)
        {
            Debug.Log($"[{bestNode.X}, {bestNode.Y}, {bestNode.Z}] = {LevelManager.Instance.levelMap[bestNode.X][bestNode.Y][bestNode.Z]}");
            while (isValidMap && bestNode.PrevCount > 0)
            {
                bestNode = bestNode.PrevNode;
                Debug.Log($"[{bestNode.X}, {bestNode.Y}, {bestNode.Z}] = {LevelManager.Instance.levelMap[bestNode.X][bestNode.Y][bestNode.Z]}");
            }
        }*/
    }

    private bool CheckMapRange(int x, int y, int z)
    {
        // ��尡 �� ���� �����ϴ°�
        return (x >= 0 && x < mapSize &&
                y >= 0 && y < mapSize &&
                z >= 0 && z < mapSize);
    }

    private bool CheckMapWay(int x, int y, int z)
    {
        // �� �� �ִ� ����ΰ�
        return LevelManager.Instance.levelMap[x][y][z] != 0;
    }

    public class BFSNode
    {
        public int X;
        public int Y;
        public int Z;

        public BFSNode PrevNode;
        public int PrevCount;

        public BFSNode(int x, int y, int z, BFSNode prevNode)
        {
            X = x;
            Y = y;
            Z = z;
            PrevNode = prevNode;

            if (PrevNode == null)
            {
                // ���� ��尡 ������ ���� �����̹Ƿ� Count�� 0
                PrevCount = 0;
            }
            else
            {
                // ���� ��尡 ������ ���� ����� '���� ��� ����' + 1
                // ��ǥ������ �ش��ϴ� ���� ���������� ������������ ��ǥ���������� ��� ���� ���� �ȴ�
                PrevCount = PrevNode.PrevCount + 1;
            }
        }
    }
}
