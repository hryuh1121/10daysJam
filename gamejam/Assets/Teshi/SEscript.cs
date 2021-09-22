using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEscript : MonoBehaviour
{

     public AudioSource audioSource1;
    public AudioSource audioSource2;
    // Start is called before the first frame update
    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource1 = GetComponent<AudioSource>();
        audioSource2 = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(audioSource1);
        if (TyphoonScript.rotate < 3)
        {
            audioSource1.Play();
        }
        if (TyphoonScript.rotate > 3)
        {
            audioSource2.Play();
        }
    }
}
