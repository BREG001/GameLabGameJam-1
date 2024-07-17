using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{
    [SerializeField] private float[] bestScore = new float[8];      // ���̽��ھ� ����(7�����)
    [SerializeField] private string[] bestName = new string[8];     // ���̽��ھ� �̸�

    public InputField InputName;
    public InputField InputScore;

    void Update()
    {
        
    }

    // ��ŷ �ҷ�����
    private void ReadRankData()
    {
        for (int i = 0; i < 7; i++)
        {
            if (PlayerPrefs.HasKey(i + "BestName"))
            {
                bestScore[i] = PlayerPrefs.GetFloat(i + "BestScore");
                bestName[i] = PlayerPrefs.GetString(i + "BestName");
            }
            else
            {
                bestScore[i] = 0;
                bestName[i] = "none";
            }
        }
    }

    // ��ŷ �����ϱ�
    private void WriteRankData()
    {
        for (int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetFloat(i + "BestScore", bestScore[i]);
            PlayerPrefs.SetString(i + "BestName", bestName[i]);
        }
    }

    // �÷��̾��� ������ ��ŷ �� �� ��ŷ ����
    private void CompareRankScore(string name, float score)
    {
        float tmpScore;
        string tmpName;

        bestScore[7] = score;
        bestName[7] = name;

        for (int i = 7; i > 0; i--)
        {
            if (bestScore[i] > bestScore[i - 1])
            {
                tmpName = bestName[i - 1];
                tmpScore = bestScore[i - 1];

                bestName[i - 1] = bestName[i];
                bestScore[i - 1] = bestScore[i];

                bestName[i] = tmpName;
                bestScore[i] = tmpScore;
            }
        }
    }

    public void ResetData()
    {
        for (int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetFloat(i + "BestScore", 0);
            PlayerPrefs.SetString(i + "BestName", "none");
        }
    }

    public void test()
    {
        ReadRankData();
        CompareRankScore(InputName.text,float.Parse(InputScore.text));
        WriteRankData();
    }
}