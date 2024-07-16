using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool startScoring = true;    // 타이머 및 점수를 체크할 것인가?
    public int score = 0;               // 전체 점수 (처치한 적 당 100점, 경과시간 1초 당 100점)
    public float currentTime = 0f;      // 현재 경과한 시간
    public int killCount = 0;           // 처치한 적의 수

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        if (startScoring) 
        { 
            // 시간 및 점수 계산
            currentTime += Time.unscaledDeltaTime;

            score = (int)(currentTime * 100f) + (killCount * 100); // 
            Debug.Log(score);
        }
    }
}
