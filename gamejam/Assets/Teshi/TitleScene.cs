using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //Invoke("ChangeScene", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }   

    void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}