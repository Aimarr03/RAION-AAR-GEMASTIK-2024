using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubleMovementController : MonoBehaviour
{
    private ParticleSystem particles;
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        PlayerMoveSystem.onMoving += PlayerMoveSystem_onMoving;
    }
    private void OnDisable()
    {
        PlayerMoveSystem.onMoving -= PlayerMoveSystem_onMoving;
    }
    private void PlayerMoveSystem_onMoving(Vector3 velocity)
    {
        var velocityOvertime = particles.velocityOverLifetime;
        velocityOvertime.x = new ParticleSystem.MinMaxCurve(-velocity.x);
        velocityOvertime.y = new ParticleSystem.MinMaxCurve(-velocity.y);
        velocityOvertime.z = new ParticleSystem.MinMaxCurve(velocity.z);
    }
}
