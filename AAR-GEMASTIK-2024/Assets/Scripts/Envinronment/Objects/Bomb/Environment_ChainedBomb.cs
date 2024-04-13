using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_ChainedBomb : Environment_BombBase
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}

