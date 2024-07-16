using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool startScoring = true;    // Ÿ�̸� �� ������ üũ�� ���ΰ�?
    public int score = 0;               // ��ü ���� (óġ�� �� �� 100��, ����ð� 1�� �� 100��)
    public float currentTime = 0f;      // ���� ����� �ð�
    public int killCount = 0;           // óġ�� ���� ��

    // Start is called before the first frame update
    void Start()
    {

    }

    public void IncreaseKillCount()
    {
        killCount++;
    }
    // Update is called once per frame
    void Update()
    { 
        if (startScoring) 
        { 
            // �ð� �� ���� ���
            currentTime += Time.unscaledDeltaTime;

            score = (int)(currentTime * 100f) + (killCount * 100); // 
            Debug.Log(score);
        }
    }
}
