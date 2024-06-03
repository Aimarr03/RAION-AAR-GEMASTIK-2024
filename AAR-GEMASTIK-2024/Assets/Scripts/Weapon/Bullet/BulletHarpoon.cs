using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BulletHarpoon : BaseBullet
{
    [SerializeField] private const string WALLTAG = "Wall";
    [SerializeField] private LayerMask wallLayer;
    private bool isCollidedWithWall;
    public override void OnLaunchBullet()
    {
        if (isCollidedWithWall) return;
        transform.position += Time.deltaTime * weaponData.speed * Vector3.right;
    }
    public override void SetUpBullet(WeaponBulletData weaponData, bool isOnRightDirection, Quaternion angle)
    {
        base.SetUpBullet(weaponData, isOnRightDirection, angle);
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
        LoadToPool();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Wall":
                isCollidedWithWall = true;
                canLaunch = false;
                Debug.Log("Collided with wall");
                break;
            default:
                if (collision.gameObject.TryGetComponent(out IDamagable damagableUnit))
                {
                    damagableUnit.TakeDamage(weaponData.totalDamage);
                    AudioManager.Instance.PlaySFX(OnHit);
                }
                break;
        }
    }
}
