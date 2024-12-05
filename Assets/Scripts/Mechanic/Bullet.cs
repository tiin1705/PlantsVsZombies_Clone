using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] public int damage;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
   
    
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public virtual void ResetState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
      

    }

    public abstract void Initialize(float bulletSpeed, int bulletDamage);
    public abstract void Fire(Vector2 direction);
    public abstract void Explode(int explodeDamage);
    protected virtual IEnumerator MoveBullet(Vector2 direction)
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            //kiểm tra viên đạn có ngoài khung camera hay không
            if (!IsInCameraView())
            {
                BulletPool.Instance.ReturnBullet(this);
                yield break;
            }
            yield return null;
        }
    }

    protected bool IsInCameraView()
    {
        //lấy thông tin camera
        Camera camera = Camera.main;

        //chuyển đổi vị trí của viên đạn sang không gian camera 
        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);

        //kiểm tra xem viên đạn có nằm trong giới hạn của camera không
        return (screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1);

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            Zombie zombie = collision.GetComponent<Zombie>();
            if(zombie != null)
            {
                zombie.TakeDamage(damage);
               // Debug.Log("Zombie took damage: " + damage);

            }
        }
    }
}
   
