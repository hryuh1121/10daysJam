using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playChara : MonoBehaviour
{
    private Rigidbody2D rig;
    private float x_val;
    public float jump;
    private float speed;
    public float inputSpeed;

    bool jumpF;

    public ParticleSystem particle;

    public ParticleSystem storm;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        x_val = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && !jumpF)
        {
            rig.AddForce(Vector2.up * jump);
            jumpF = true;
        }

    }

    private void FixedUpdate()
    {
        if (x_val == 0)
        {
            speed = 0;
        }
        else if (x_val > 0)
        {
            speed = inputSpeed;
        }
        else if (x_val < 0)
        {
            speed = inputSpeed * -1;
        }



        rig.velocity = new Vector2(speed, rig.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpF = false;

        Vector3 hitpos = Vector3.zero;

        //�����������W�̎擾
        foreach(ContactPoint2D point in collision.contacts)
        {
            hitpos = point.point;
        }

        BoundsInt.PositionEnumerator position = 
            collision.gameObject.GetComponent<Tilemap>().cellBounds.allPositionsWithin;

        var allPosition = new List<Vector3>();

        //��ԋ߂��ꏊ��ۑ�����ϐ�
        int minPositionNum = 0;

        foreach(var variable in position)
        {
            if(collision.gameObject.GetComponent<Tilemap>().GetTile(variable) != null)
            {
                allPosition.Add(variable);
            }
        }

        //for���ŒT������B�ł���������0����Ă邩��1����X�^�[�g
        for(int i = 1;i < allPosition.Count; i++)
        {
            //���ꂼ��̓��������ꏊ����̑傫�����擾�A�ŏ����X�V������minPositionNum���X�V
            if((hitpos - allPosition[i]).magnitude<
                (hitpos -allPosition[minPositionNum]).magnitude)
            {
                minPositionNum = i;
            }
        }

        //�ŏI�I�Ȉʒu����U�i�[�����BRoundToInt�͎l�̌ܓ�
        Vector3Int finalPosition = Vector3Int.RoundToInt(allPosition[minPositionNum]);

        TileBase tiletmp = collision.gameObject.GetComponent<Tilemap>().GetTile(finalPosition);

        if(tiletmp != null)
        {
            Tilemap map = collision.gameObject.GetComponent<Tilemap>();
            TilemapCollider2D tileCol = collision.gameObject.GetComponent<TilemapCollider2D>();

            map.SetTile(finalPosition, null);
            tileCol.enabled = false;
            tileCol.enabled = true;

            storm.transform.localScale += new Vector3(2.0f, 2.0f,0.0f);
            Destroy(Instantiate(particle, finalPosition + Vector3.one * 0.5f, Quaternion.identity), 1.0f);
        }  
    }
}
