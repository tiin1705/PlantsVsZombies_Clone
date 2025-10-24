using UnityEngine;

public class ZombieDebug : MonoBehaviour
{
    public Animator animator;
    public float health = 210;

    void Update()
    {
        // Nhấn phím H để giảm máu nhanh
        if (Input.GetKeyDown(KeyCode.H))
        {
            health -= 30;
            animator.SetFloat("health", health);
            Debug.Log("Health: " + health);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            animator.SetFloat("Attacking",0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            animator.SetFloat("Attacking", 1);
        }
    }
}
