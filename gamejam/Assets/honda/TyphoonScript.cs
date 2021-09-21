using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TyphoonScript : MonoBehaviour
{
    //����]���������J�E���g����
    [SerializeField]
    private int rotate;

    //�X�e�B�b�N�̊p�x
    private float radian;
    private int rotatestate;

    //���˂�����
    [SerializeField]
    private bool isShot = false;

    //�䕗�̃X�s�[�h�Ɋ|����W��
    [SerializeField]
    private float speed;

    GameObject score;

    //�p�[�e�B�N��
    public ParticleSystem particle;

    public ParticleSystem storm;

    //leap���|���銄��
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
        }
    }

    //�X�e�B�b�N���͂������Ƃ��ɔ��˂���
    void Shot()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        //��]����1�ȏ�ŃX�e�B�b�N�𗣂����Ƃ�
        if (rotate >= 1)
        {
            if (h == 0 && v == 0)
            {
                isShot = true;
            }
        }



        if (isShot)
        {
            //{//Leap���g��������
            //    Vector3 posTo = new Vector3(rotate * 10, 0, 0);
            //    transform.position = Vector3.Lerp(transform.position, posTo, positionLerpSpeed);
            //}

            {//��Z���g��������
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

            storm.transform.localScale += new Vector3(2.0f, 2.0f, 0.0f);
            Destroy(Instantiate(particle, finalPosition + Vector3.one * 0.5f, Quaternion.identity), 0.85f);
        }
    }
}
