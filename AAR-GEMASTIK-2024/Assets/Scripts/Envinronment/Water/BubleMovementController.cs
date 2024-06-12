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
        AirReplenisher.OnResurface += AirReplenisher_OnResurface;
        PlayerMoveSystem.onMoving += PlayerMoveSystem_onMoving;
    }
    private void OnDisable()
    {
        AirReplenisher.OnResurface -= AirReplenisher_OnResurface;
        PlayerMoveSystem.onMoving -= PlayerMoveSystem_onMoving;
    }
    private void AirReplenisher_OnResurface(bool resurface)
    {
        if(resurface) particles.Stop();
        else particles.Play();
    }
    private void PlayerMoveSystem_onMoving(Vector3 velocity)
    {
        var velocityOvertime = particles.velocityOverLifetime;
        velocityOvertime.x = new ParticleSystem.MinMaxCurve(-velocity.x);
        velocityOvertime.y = new ParticleSystem.MinMaxCurve(-velocity.y);
        velocityOvertime.z = new ParticleSystem.MinMaxCurve(velocity.z);
    }
}
