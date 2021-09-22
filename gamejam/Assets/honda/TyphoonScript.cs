using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TyphoonScript : MonoBehaviour
{
    //何回転したかをカウントする
    private int rotate;

    //スティックの角度
    private float radian;
    private int rotatestate;

    //発射したか
    private bool isShot = false;

    //台風のスピードに掛ける係数
    private float speed;

    //
    public GameObject score;

    //タイマー
    private float countUp;
    private float timeLimit;

    private float startCount;

    public Text timeText;
    public Text startText;
    public Text rotateText;
    public Text sceneText;

    //パーティクル
    public ParticleSystem particle;

    public ParticleSystem storm;

    AudioSource audioSource;
    //leapを掛ける割合
    //[SerializeField, Range(0.001f, 0.01f)]
    //private float positionLerpSpeed = 0.001f;




    // Start is called before the first frame update
    void Start()
    {
        rotate = 0;
        radian = 0;
        rotatestate = 0;
        speed = 0;
        isShot = false;
        //speed = 0.01f;
        score = GameObject.Find("Canvas");
   

        countUp = 10;
        timeLimit = 0;
        startCount = 3.5f;
        timeText.enabled = true;
        rotateText.enabled = true;
        startText.enabled = true;
        sceneText.enabled = false;
        startText.color = Color.red;
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
        startCount -= Time.deltaTime;
        //スタートするまでは入力を受け付けない
        if (startCount>0.5)
        {        
            startText.text = startCount.ToString("f0");
            return;
        }
        if(startCount<=0.5)
        {//スタートテキスト
            startText.text = "START!";
        }
        if(startCount<=-0.5)
        {//スタートテキストを非表示
            startText.enabled = false;
        }

        //時間をカウントする
        countUp -= Time.deltaTime;
        //時間を表示する
        timeText.text = "残り時間:"+countUp.ToString("f0") + "秒";

        if(countUp>timeLimit)
        {//左スティックを回転させる時間
            rotateText.text = "まわせ！";
        }

        if (countUp <= timeLimit)
        {
            rotateText.enabled = false;
            timeText.color = Color.red;
            timeText.text = "   GO!!";
            isShot = true;
        }
        if(countUp <= timeLimit -1)
        {
            timeText.enabled = false;
        }

        //transform.localScale = new Vector3(rotate/10, rotate/10, 0);
        

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

        //台風のspeedが0になったら
        if(isShot == true && speed == 0)
        {
            startText.enabled = true;
            int lastScore = score.GetComponent<ScoreScript>().GetScore();
            startText.text = lastScore.ToString();
            sceneText.enabled = true;

            //ボタンが押されたらタイトルへ
            if (Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene("Title");
            }
            //ボタンが押されたらアプリ終了
            if (Input.GetButtonDown("Fire2"))
            {
                Application.Quit();
            }
        }
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
            storm.transform.localScale += new Vector3(0.05f, 0.05f, 0.0f);
        }
    }

    //スティックをはじいたときに発射する
    void Shot()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");





        if (isShot)
        {
            //{//Leapを使った減速
            //    Vector3 posTo = new Vector3(rotate * 10, 0, 0);
            //    transform.position = Vector3.Lerp(transform.position, posTo, positionLerpSpeed);
            //}

            {//乗算を使った減速
                transform.position += new Vector3(speed, 0, 0);
                if (speed > 0.002f)
                {
                    speed *= 0.999f;
                    score.GetComponent<ScoreScript>().AddScore((int)(speed*300));
                }
                else
                {
                    speed = 0;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //jumpF = false;

        Vector3 hitpos = Vector3.zero;

        //当たった座標の取得
        foreach (ContactPoint2D point in collision.contacts)
        {
            hitpos = point.point;
        }

        BoundsInt.PositionEnumerator position =
            collision.gameObject.GetComponent<Tilemap>().cellBounds.allPositionsWithin;

        var allPosition = new List<Vector3>();

        //一番近い場所を保存する変数
        int minPositionNum = 0;

        foreach (var variable in position)
        {
            if (collision.gameObject.GetComponent<Tilemap>().GetTile(variable) != null)
            {
                allPosition.Add(variable);
            }
        }

        //for文で探査する。でも初期化で0入れてるから1からスタート
        for (int i = 1; i < allPosition.Count; i++)
        {
            //それぞれの当たった場所からの大きさを取得、最小を更新したらminPositionNumを更新
            if ((hitpos - allPosition[i]).magnitude <
                (hitpos - allPosition[minPositionNum]).magnitude)
            {
                minPositionNum = i;
            }
        }

        //最終的な位置を一旦格納した。RoundToIntは四捨五入
        Vector3Int finalPosition = Vector3Int.RoundToInt(allPosition[minPositionNum]);

        TileBase tiletmp = collision.gameObject.GetComponent<Tilemap>().GetTile(finalPosition);

        if (tiletmp != null)
        {
            Tilemap map = collision.gameObject.GetComponent<Tilemap>();
            TilemapCollider2D tileCol = collision.gameObject.GetComponent<TilemapCollider2D>();

            map.SetTile(finalPosition, null);
            tileCol.enabled = false;
            tileCol.enabled = true;

           audioSource.Play();
            Destroy(Instantiate(particle, finalPosition + Vector3.one * 0.5f, Quaternion.identity), 0.85f);
        }
    }
}
