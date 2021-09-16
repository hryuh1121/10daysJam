using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphoonScript : MonoBehaviour
{
    //何回転したかをカウントする
    [SerializeField]
    private int rotate;

    //スティックの角度
    [SerializeField]
    private float radian;

    //
    [SerializeField]
    private int rotatestate;

    // Start is called before the first frame update
    void Start()
    {
        rotate = 0;
        radian = 0;
        rotatestate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        radian = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

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
        if(rotatestate == 5 && radian == -90)
        {
            rotatestate = 6;
        }
        if(rotatestate == 6 && radian == -45)
        {
            rotatestate = 7;
        }
        if (rotatestate == 7 && radian == 0)
        {
            rotatestate = 0;
            rotate += 1;
        }
    }
}
