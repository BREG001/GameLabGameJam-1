using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public bool startScoring = true;    // Ÿ�̸� �� ������ üũ�� ���ΰ�?
    public int score = 0;               // ��ü ���� (óġ�� �� �� 100��, ����ð� 1�� �� 100��)
    public float currentTime = 0f;      // ���� ����� �ð�
    public int killCount = 0;           // óġ�� ���� ��

    void Awake()
    {
        ScoreManager.instance = this;
        
    }

    void Start()
    {
        GameSceneManager.Instance.GameSceneEvent.GameOver += StopScoring;
    }

    private void OnDestroy()
    {
        GameSceneManager.Instance.GameSceneEvent.GameOver -= StopScoring;
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
            currentTime += Time.deltaTime;

            score = ((int)currentTime * 100) + (killCount * 100);
            //Debug.Log(score);
        }
    }

    private void StopScoring(GameSceneEventArgs gameSceneEventArgs)
    {
        startScoring = false;
    }
}
