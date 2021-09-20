using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphoonScript : MonoBehaviour
{
    //何回転したかをカウントする
    [SerializeField]
    private int rotate;

    //スティックの角度
    private float radian;
    private int rotatestate;

    //発射したか
    [SerializeField]
    private bool isShot = false;

    //台風のスピードに掛ける係数
    [SerializeField]
    private float speed;

    GameObject score;

    //leapを掛ける割合
    //[SerializeField, Range(0.001f, 0.01f)]
    //private float positionLerpSpeed = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        rotate = 0;
        radian = 0;
        rotatestate = 0;
        isShot = false;
        //speed = 0.01f;
        score = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        radian = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

        //発射する前なら回転数追加
        if (!isShot)
        {
            GetRotate();
            speed = rotate * 0.01f;
        }

        Shot();
        


        //回転数によって視覚的に分かるようにする
        //float a = 1 + rotate / 10;
        //transform.localScale = new Vector3(1, a, 0);
    }

    //回転数取得関数
    void GetRotate()
    {

        if (rotatestate == 0 && radian == 45)
        {
            rotatestate = 1;
        }
        if (rotatestate == 1 && radian == 90)
        {
            rotatestate = 2;
        }
        if (rotatestate == 2 && radian == 135)
        {
            rotatestate = 3;
        }
        if (rotatestate == 3 && radian == 180)
        {
            rotatestate = 4;
        }
        if (rotatestate == 4 && radian == -135)
        {
            rotatestate = 5;
        }
        if (rotatestate == 5 && radian == -90)
        {
            rotatestate = 6;
        }
        if (rotatestate == 6 && radian == -45)
        {
            rotatestate = 7;
        }
        if (rotatestate == 7 && radian == 0)
        {
            rotatestate = 0;
            rotate += 1;
        }
    }

    //スティックをはじいたときに発射する
    void Shot()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        //回転数が1以上でスティックを離したとき
        if (rotate >= 1)
        {
            if (h == 0 && v == 0)
            {
                isShot = true;
            }
        }



        if (isShot)
        {
            //{//Leapを使った減速
            //    Vector3 posTo = new Vector3(rotate * 10, 0, 0);
            //    transform.position = Vector3.Lerp(transform.position, posTo, positionLerpSpeed);
            //}

            {//乗算を使った減速
                transform.position += new Vector3(speed, 0, 0);
                if (speed > 0.001f)
                {
                    speed *= 0.999f;
                    score.GetComponent<ScoreScript>().AddScore(1);
                }
                else
                {
                    speed = 0;
                }
            }
        }
    }
}
