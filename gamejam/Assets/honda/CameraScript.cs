using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Typhoon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 typhoonPos = Typhoon.transform.position;
        transform.position = new Vector3(typhoonPos.x+7, 0, -40);
    }
}
