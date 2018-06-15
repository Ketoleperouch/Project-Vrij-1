using UnityEngine;

public class Trapdoor : MonoBehaviour {

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Active");
            Destroy(this);
        }
    }
}
