using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Animator animator = other.GetComponentInChildren<Animator>();
            animator.SetTrigger("Die");

            other.GetComponent<PlayerController>().enabled = false;
        }
    }
}