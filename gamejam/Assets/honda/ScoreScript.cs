using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    //�X�R�A
    private int Score = 0;
    //�X�R�A�e�L�X�g
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�X�R�A�\��
        scoreText.text = Score.ToString();

        if(Input.GetKey(KeyCode.A))
        {
            AddScore();
        }
    }

    public void AddScore()
    {
        Score = Score + 10;
    }
}
