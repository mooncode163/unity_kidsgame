using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndParticle : MonoBehaviour
{

    public ParticleSystem particleSystemGame0;
    public ParticleSystem particleSystemGame1;
    public ParticleSystem particleSystemGame2;
    public ParticleSystem particleSystemGame3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        particleSystemGame0.Play();
        particleSystemGame1.Play();
        particleSystemGame2.Play();
        particleSystemGame3.Play();
    }
}
