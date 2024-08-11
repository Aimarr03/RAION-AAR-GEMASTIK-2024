using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public WeaponBase weaponBase;
    public AudioClip OnCreated;
    public AudioClip OnHit;
    protected bool canLaunch = false;
    [SerializeField] protected float TimeToLiveDurationHolder;
    [SerializeField] protected float TimeToLive;
    protected ObjectPooling parentPool;
    public virtual void SetUpBullet(bool isOnRightDirection, Quaternion angle)
    {
        transform.rotation = angle;
        //speed = isOnRightDirection ? speed : -speed;
        float y_angle = isOnRightDirection ? 0 : 180;
        Vector3 playerRotation = weaponBase.GetPlayerCoreSystem.transform.eulerAngles;
        /*Debug.Log(playerRotation);*/
        transform.rotation = Quaternion.Euler(playerRotation.x, y_angle, playerRotation.z);
        Debug.Log("bullet data has been set up");
        /*Debug.Log($"Bullet Rotation {transform.eulerAngles}");*/
        canLaunch = true;
    }
    public abstract void Update();
    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnLaunchBullet();
    public void SetObjectPooling(ObjectPooling parentPool) => this.parentPool = parentPool;

    public void LoadToPool()
    {
        try
        {
            gameObject.SetActive(false);
            transform.position = parentPool.transform.position;
            transform.parent = parentPool.transform;
            parentPool.LoadBullet(this);
        }
        catch(Exception e)
        {

        }
    }
}
