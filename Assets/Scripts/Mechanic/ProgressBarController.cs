using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public AudioClip zombieComing;
    private AudioSource audioSource;
    public Image progressBarFill;
    public RectTransform marker, zombieHead;
    public float totalProgressDuration;
    private float progressTimer = 0f;
    private bool isRuninng = false;
    [SerializeField] private ZombieSpawner zombieSpawner;

    private float fillWidth;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        totalProgressDuration = zombieSpawner.GetTotalProgressDuration();
        float finalTime = zombieSpawner.GetFinalPhaseStartTime(); //Lấy thời gian bắt đầu của final phase
        float finalPercent = finalTime / totalProgressDuration; //lấy phần trăm của final phase trên tổng thời gian

        RectTransform fillRect = (RectTransform)progressBarFill.transform;
        fillWidth = fillRect.rect.width * fillRect.lossyScale.x; //lấy độ dài của fill bar
        marker.anchoredPosition = new Vector2(-finalPercent * fillWidth, marker.anchoredPosition.y);
        audioSource.PlayOneShot(zombieComing);

    }

    private void Update()
    {
        if (!isRuninng) return;

        progressTimer += Time.deltaTime;
        float percent = Mathf.Clamp01(progressTimer / totalProgressDuration);
        progressBarFill.fillAmount = percent;

        if(zombieHead != null)
        {
            zombieHead.anchoredPosition = new Vector2(-percent * fillWidth, zombieHead.anchoredPosition.y);
            if (percent >= 1f)
                isRuninng = false;
         }
    }

    public void StartProgress()
    {
        isRuninng = true;
        progressTimer = 0f;
        progressBarFill.fillAmount = 0f;
       
    }
}
