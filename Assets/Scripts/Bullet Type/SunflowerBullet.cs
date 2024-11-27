using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerBullet : Bullet
{
    private Vector2 direction;
    private Animator animator;
    private static float bulletSpacingZ = 0.00001f;
    private static float currentZPosition = -8f;
    public Transform uiSunTarget;
    private bool isMovingToUI = false;
    public float moveSpeedToUI = 25f;

    private void Start()
    {
        if(uiSunTarget == null)
        {
            GameObject sunDummy = GameObject.Find("SunDummyTarget");
            if(sunDummy != null )
            {
                uiSunTarget = sunDummy.transform;
            }
            else
            {
                Debug.LogWarning("SunDummyTarget không được tìm thấy tron scene");
            }
        }
    }
    public override void Initialize(float bulletSpeed, int bulletDamage)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        animator = GetComponent<Animator>();
    }
    public override void Fire(Vector2 direction)
    {
        this.direction = direction;
        StartCoroutine(MoveBullet(direction));
    }
    private void Update()
    {
        if (isMovingToUI)
        {
            MoveBulletToUI();
        }
    }
    protected override IEnumerator MoveBullet(Vector2 direction)
    {
        // Lưu trữ vị trí ban đầu
        Vector3 startPosition = transform.position;

        // Điều chỉnh vị trí Z của viên đạn
        startPosition.z = currentZPosition;

        // Cập nhật vị trí Z cho viên đạn tiếp theo
        currentZPosition += bulletSpacingZ;

        transform.position = startPosition;

        // Đảm bảo collider không bị ảnh hưởng
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.enabled = false; // Tắt collider tạm thời
        collider.enabled = true;  // Bật lại collider sau khi thay đổi Z

        float heigh = 0.5f;
        float duration = 0.2f;
        float elapsedTime = 0f;

        Vector3 endPosition = startPosition + new Vector3(0, heigh, 0);

        //nhảy lên
        while(elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //Dừng lại một chút
        yield return new WaitForSeconds(0.1f);

        //rơi xuống
        float fallDuration = 0.2f;
        elapsedTime = 0f;
        while(elapsedTime < fallDuration)
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, (elapsedTime / fallDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private void OnMouseDown()
    {
        isMovingToUI = true;
    }

    private void MoveBulletToUI()
    {
        if (uiSunTarget != null)
        {
            Vector3 uiScreenPosition = Camera.main.WorldToScreenPoint(uiSunTarget.position);
            Vector3 bulletPositionInScreenSpace = new Vector3(uiScreenPosition.x, uiScreenPosition.y, 0f);
            Vector3 targetWorldPosition = Camera.main.ScreenToWorldPoint(bulletPositionInScreenSpace);

            transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeedToUI * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetWorldPosition) < 0.1f)
            {
                SunManager.Instance.AddSun(50);
                BulletPool.Instance.ReturnBullet(this);

            }
        }
    }
}
