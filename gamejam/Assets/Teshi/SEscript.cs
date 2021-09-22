using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEscript : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.PlayOneShot(clip1);

    }
}
