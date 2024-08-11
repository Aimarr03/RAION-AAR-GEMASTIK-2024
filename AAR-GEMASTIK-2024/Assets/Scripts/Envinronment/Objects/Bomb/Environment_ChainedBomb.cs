using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_ChainedBomb : Environment_BombBase
{
    [SerializeField] private ParticleSystem explosion01;
    [SerializeField] private ParticleSystem explosion02;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        explosion01.transform.parent = null;
        explosion02.transform.parent = null;
        explosion01.Play();
        explosion02.Play();
    }
}

