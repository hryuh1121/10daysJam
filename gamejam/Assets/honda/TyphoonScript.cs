using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TyphoonScript : MonoBehaviour
{
    //����]���������J�E���g����
    private int rotate;

    //�X�e�B�b�N�̊p�x
    private float radian;
    private int rotatestate;

    //���˂�����
    private bool isShot = false;

    //�䕗�̃X�s�[�h�Ɋ|����W��
    private float speed;

    //
    public GameObject score;

    //�^�C�}�[
    private float countUp;
    private float timeLimit;

    private float startCount;

    public Text timeText;
    public Text startText;
    public Text rotateText;
    public Text sceneText;

    //�p�[�e�B�N��
    public ParticleSystem particle;

    public ParticleSystem storm;

    AudioSource audioSource;
    //leap���|���銄��
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
        //Component���擾
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
        startCount -= Time.deltaTime;
        //�X�^�[�g����܂ł͓��͂��󂯕t���Ȃ�
        if (startCount>0.5)
        {        
            startText.text = startCount.ToString("f0");
            return;
        }
        if(startCount<=0.5)
        {//�X�^�[�g�e�L�X�g
            startText.text = "START!";
        }
        if(startCount<=-0.5)
        {//�X�^�[�g�e�L�X�g���\��
            startText.enabled = false;
        }

        //���Ԃ��J�E���g����
        countUp -= Time.deltaTime;
        //���Ԃ�\������
        timeText.text = "�c�莞��:"+countUp.ToString("f0") + "�b";

        if(countUp>timeLimit)
        {//���X�e�B�b�N����]�����鎞��
            rotateText.text = "�܂킹�I";
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


        //���˂���O�Ȃ��]���ǉ�
        if (!isShot)
        {
            GetRotate();
            speed = rotate * 0.01f;
        }

        Shot();

        //��]���ɂ���Ď��o�I�ɕ�����悤�ɂ���
        //float a = 1 + rotate / 10;
        //transform.localScale = new Vector3(1, a, 0);

        //�䕗��speed��0�ɂȂ�����
        if(isShot == true && speed == 0)
        {
            startText.enabled = true;
            int lastScore = score.GetComponent<ScoreScript>().GetScore();
            startText.text = lastScore.ToString();
            sceneText.enabled = true;

            //�{�^���������ꂽ��^�C�g����
            if (Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene("Title");
            }
            //�{�^���������ꂽ��A�v���I��
            if (Input.GetButtonDown("Fire2"))
            {
                Application.Quit();
            }
        }
    }

    //��]���擾�֐�
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

    //�X�e�B�b�N���͂������Ƃ��ɔ��˂���
    void Shot()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");





        if (isShot)
        {
            //{//Leap���g��������
            //    Vector3 posTo = new Vector3(rotate * 10, 0, 0);
            //    transform.position = Vector3.Lerp(transform.position, posTo, positionLerpSpeed);
            //}

            {//��Z���g��������
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

        //�����������W�̎擾
        foreach (ContactPoint2D point in collision.contacts)
        {
            hitpos = point.point;
        }

        BoundsInt.PositionEnumerator position =
            collision.gameObject.GetComponent<Tilemap>().cellBounds.allPositionsWithin;

        var allPosition = new List<Vector3>();

        //��ԋ߂��ꏊ��ۑ�����ϐ�
        int minPositionNum = 0;

        foreach (var variable in position)
        {
            if (collision.gameObject.GetComponent<Tilemap>().GetTile(variable) != null)
            {
                allPosition.Add(variable);
            }
        }

        //for���ŒT������B�ł���������0����Ă邩��1����X�^�[�g
        for (int i = 1; i < allPosition.Count; i++)
        {
            //���ꂼ��̓��������ꏊ����̑傫�����擾�A�ŏ����X�V������minPositionNum���X�V
            if ((hitpos - allPosition[i]).magnitude <
                (hitpos - allPosition[minPositionNum]).magnitude)
            {
                minPositionNum = i;
            }
        }

        //�ŏI�I�Ȉʒu����U�i�[�����BRoundToInt�͎l�̌ܓ�
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
