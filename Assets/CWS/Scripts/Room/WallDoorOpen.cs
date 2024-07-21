using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WallDoorOpen : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _wallClosedCollider;

    public DoorPassColliderEventArgs DoorPassEvent;
    public bool isDoorActive = true;
    public Vector3Int passDirection = Vector3Int.zero;

    void Start()
    {
        DoorPassEvent.DoorPass += MoveRoom;
    }

    void OnDestroy()
    {
        DoorPassEvent.DoorPass -= MoveRoom;
    }

    void Update()
    {
        
    }

    public void ActiveDoor()
    {
        isDoorActive = !isDoorActive;
        _door.SetActive(isDoorActive);
        _wallClosedCollider.SetActive(isDoorActive);
    }

    public void ActiveDoor(bool _isDoorActive)
    {
        isDoorActive = _isDoorActive;
        _door.SetActive(isDoorActive);
        _wallClosedCollider.SetActive(isDoorActive);
    }

    public void MoveRoom(DoorPassColliderEventArgs doorPassColliderEvent)
    {
        // �̵��ϴ� ���� �Ͻ�����
        Time.timeScale = 0;

        // �̵��� �� ������Ʈ ���� �� �θ� ����
        GameObject newRoom = 
            Instantiate(LevelManager.Instance.roomList[0].prefab, 
            LevelManager.Instance.RoomParentTransform);

        // �̵��� �� ��ġ ����
        newRoom.transform.position = LevelManager.Instance.RoomParentTransform.position + (passDirection * 21);
        newRoom.transform.rotation = LevelManager.Instance.RoomParentTransform.rotation;

        // �� �̵� ����
        LevelManager.Instance.MoveRoom(passDirection, newRoom, gameObject.transform.parent.transform.parent.gameObject);
        
    }
}
