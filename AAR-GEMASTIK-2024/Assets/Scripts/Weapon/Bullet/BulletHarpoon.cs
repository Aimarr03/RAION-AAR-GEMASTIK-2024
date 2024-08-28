using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BulletHarpoon : BaseBullet
{
    public WeaponHarpoon weaponHarpoon => weaponBase as WeaponHarpoon;
    public int level => weaponBase.level;
    [SerializeField] private const string WALLTAG = "Wall";
    [SerializeField] private LayerMask wallLayer;
    private bool isCollidedWithWall;
    private float speed;
    public override void OnLaunchBullet()
    {
        if (isCollidedWithWall) return;
        TimeToLive = TimeToLiveDurationHolder;
        transform.Translate(speed * Time.deltaTime * Vector3.right, Space.Self);
    }
    public override void SetUpBullet(bool isOnRightDirection, Quaternion angle)
    {
        base.SetUpBullet(isOnRightDirection, angle);
        speed = weaponHarpoon.GetMultiplierSpeed(level);
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
        if(weaponBase != null)
        {
            Debug.Log("Bullet exceed time to live");
            LoadToPool();
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
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
                    damagableUnit.TakeDamage(weaponHarpoon.GetMultiplierDamage(level));
                    damagableUnit.GetSlowed(weaponHarpoon.GetMultiplierSlowDuration(level), weaponHarpoon.GetMultiplierSlow(level));
                    AudioManager.Instance?.PlaySFX(OnHit);
                }
                break;
        }
    }
}
