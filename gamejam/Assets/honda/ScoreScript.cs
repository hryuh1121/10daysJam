using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    //スコア
    private int Score = 0;
    //スコアテキスト
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //スコア表示
        scoreText.text = Score.ToString();

    }

    //スコア加算クラス
    public void AddScore(int point)
    {
        Score = Score + point;
    }
}
