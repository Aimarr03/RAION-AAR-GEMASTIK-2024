using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BulletHarpoon : BaseBullet
{
    public override void OnCollisionEnter(Collision collision)
    {
        
    }
    public override void OnLaunchBullet()
    {
        transform.position += Time.deltaTime * weaponData.speed * Vector3.right;
    }
    public override void SetUpBullet(WeaponBulletData weaponData, bool isOnRightDirection)
    {
        base.SetUpBullet(weaponData, isOnRightDirection);
        TimeToLiveBullet();
    }
    public override void Update()
    {
        if (!canLaunch) return;
        OnLaunchBullet();   
    }
    private async void TimeToLiveBullet()
    {
        Debug.Log(TimeToLive);
        while(TimeToLive > 0)
        {
            TimeToLive -= Time.deltaTime;
            await Task.Yield();
        }
        Debug.Log("Bullet exceed time to live");
        Destroy(gameObject);
    }
}
