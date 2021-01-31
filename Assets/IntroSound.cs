using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSound : MonoBehaviour
{
    public AudioSource playintro;

    // Start is called before the first frame update
    void Start()
    {
        playintro.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
