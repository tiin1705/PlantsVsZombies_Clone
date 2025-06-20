using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehavior : MonoBehaviour
{
    public float fallSpeed = 1f;
    public int sunValue = 25;
    private Vector3 targetPosition;
    public Transform uiSunTarget;
    private bool isMovingToUI = false;
    public float moveSpeed = 5f;
    public AudioClip sunPickup;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }
    private void Update()
    {
        if (isMovingToUI)
        {
            MoveSunToUI();
        }
        else
        {
            MoveSunToTarget();
        }   
    }

    private void OnMouseDown()
    {

        if (!isMovingToUI)
        {
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(sunPickup);
            isMovingToUI = true;
            Debug.Log("SunBehavior: isMovingToUI = true");

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with " + other.gameObject.name);
        if (other.CompareTag("Sun"))
        {
            Debug.Log("Sun picked up: " + other.gameObject.name);
        }
    }

    private void MoveSunToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            transform.position = targetPosition;
            StartCoroutine(WaitForDestroy());
            
        }
    }

    private void MoveSunToUI()
    {
        Vector3 uiWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiSunTarget.position.x, uiSunTarget.position.y, Camera.main.nearClipPlane));
        transform.position = Vector3.MoveTowards(transform.position, uiWorldPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, uiWorldPosition) < 0.1f)
        {
            SunManager.Instance.AddSun(sunValue);
            
            Destroy(gameObject) ;
        }
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
