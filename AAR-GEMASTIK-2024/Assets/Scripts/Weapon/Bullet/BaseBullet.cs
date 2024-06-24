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
        Debug.Log("bullet data has been set up");
        canLaunch = true;
    }
    public abstract void Update();
    public abstract void OnTriggerEnter(Collider other);
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
