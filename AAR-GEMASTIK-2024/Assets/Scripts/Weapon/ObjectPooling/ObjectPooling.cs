using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    private Queue<BaseBullet> bulletsOnLoad = new Queue<BaseBullet>();
    private Transform prefab;
    [SerializeField] private int ammountToHold;
    public void InitializePool(WeaponSO weaponData)
    {
        Debug.Log("Initialize Object Pooling with " + weaponData);
        ammountToHold = weaponData.ammountToHold;
        prefab = weaponData.bullet;
        for(int index = 0; index < ammountToHold; index++)
        {
            Transform bullet = Instantiate(prefab, transform);
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(false);
            BaseBullet baseBullet = bullet.GetComponent<BaseBullet>();
            baseBullet.SetObjectPooling(this);
            bulletsOnLoad.Enqueue(baseBullet);
        }
    }
    
    public BaseBullet UnloadBullet()
    {
        BaseBullet baseBullet;
        if(bulletsOnLoad.Count > 0)
        {
            baseBullet =  bulletsOnLoad.Dequeue();
            baseBullet.gameObject.SetActive(true);
            return baseBullet;
        }
        baseBullet = Instantiate(prefab, transform).GetComponent<BaseBullet>();
        baseBullet.transform.position = transform.position;
        baseBullet.SetObjectPooling(this);
        baseBullet.gameObject.SetActive(true);
        return baseBullet;
    }
    public void LoadBullet(BaseBullet baseBullet)
    {
        bulletsOnLoad.Enqueue(baseBullet);
    }
}
