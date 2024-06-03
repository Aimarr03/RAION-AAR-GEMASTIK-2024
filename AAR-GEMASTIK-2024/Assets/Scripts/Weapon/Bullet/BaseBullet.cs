using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    public AudioClip OnCreated;
    public AudioClip OnHit;
    public WeaponBulletData weaponData;
    protected bool canLaunch = false;
    [SerializeField] protected float TimeToLive;
    protected ObjectPooling parentPool;
    public virtual void SetUpBullet(WeaponBulletData weaponData, bool isOnRightDirection, Quaternion angle)
    {
        transform.rotation = angle;
        weaponData.speed = isOnRightDirection ? weaponData.speed : -1 * weaponData.speed;
        this.weaponData = weaponData;
        Debug.Log("bullet data has been set up");
        canLaunch = true;
    }
    public abstract void Update();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnLaunchBullet();
    public void SetObjectPooling(ObjectPooling parentPool) => this.parentPool = parentPool;

    public void LoadToPool()
    {
        gameObject.SetActive(false);
        transform.position = parentPool.transform.position;
        transform.parent = parentPool.transform;
        parentPool.LoadBullet(this);
    }
}
