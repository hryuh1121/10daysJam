using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
