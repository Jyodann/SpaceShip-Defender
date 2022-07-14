using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
   [SerializeField] private Bullet _bulletPrefab;
   [SerializeField] private int spawnAmount;

   public static ObjectPool<Bullet> Pool;

   private void Start()
   {
      Pool = new ObjectPool<Bullet>(
         () =>
         {
            var bullet = Instantiate(_bulletPrefab);
            bullet.InitialSetUp();
            return bullet;
         }, 
         bullet =>
         {
            bullet.gameObject.SetActive(true);
         },
         bullet =>  bullet.gameObject.SetActive(false),
         bullet => Destroy(bullet),
         collectionCheck: false,
         defaultCapacity: 200
         );
   }
}
