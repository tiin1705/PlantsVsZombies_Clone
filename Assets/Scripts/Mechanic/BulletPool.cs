using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject[] bulletPrefabs;
    public int poolSize = 30; //số lượng đạn trong pool

    private Dictionary<string, Queue<Bullet>> bulletPools = new Dictionary<string, Queue<Bullet>>();

    // private Queue<Bullet> bulletPool = new Queue<Bullet>();

    private void Awake()
    {
        //Khi khởi động nếu đạn trong pool ít hơn poolSize thì sẽ cộng đạn vào pool
        Instance = this;
        foreach (var bulletPrefab in bulletPrefabs)
        {
            foreach (var bulletPool in bulletPools)
            {
           //     Debug.Log("Bullet type: " + bulletPool.Key + ", Count: " + bulletPool.Value.Count);
            }
            var bulletQueue = new Queue<Bullet>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bulletQueue.Enqueue(bullet.GetComponent<Bullet>());
          //      Debug.Log("Bullet added to pool: " + bulletPrefab.name); // Thêm debug

            }
            bulletPools.Add(bulletPrefab.name, bulletQueue); // Lưu trữ theo tên prefab
         //   Debug.Log("Pool for " + bulletPrefab.name + " has " + bulletQueue.Count + " bullets.");

        }
    }

    public Bullet GetBullet(string bulletType)
    {
        if (bulletPools.ContainsKey(bulletType) && bulletPools[bulletType].Count > 0)
        {
         //   Debug.Log("Pool Count for " + bulletType + ": " + bulletPools[bulletType].Count);

            Bullet bullet = bulletPools[bulletType].Dequeue();
            bullet.gameObject.SetActive(true);
        //    Debug.Log("Bullet retrieved: " + bullet.gameObject.name);

            return bullet;
        }
        else
        {
          //  Debug.LogWarning("No bullets available in the pool for type: " + bulletType);
            return null;
        }
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        // Tìm kiếm loại viên đạn trong danh sách
        string bulletType = bullet.GetType().Name; // Lấy tên lớp

        if (bulletPools.ContainsKey(bulletType))
        {
            bulletPools[bulletType].Enqueue(bullet); // Trả lại viên đạn vào pool tương ứng
        //    Debug.Log("Bullet returned to pool: " + bulletType);
            return;
        }

     //   Debug.LogWarning("Bullet type not found in any pool: " + bulletType);
    }      

}
