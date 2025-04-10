using UnityEngine;

public class TriggerCapybara : MonoBehaviour
{
    public Animator capybaraAnimator;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("on triggerEnter method");
            capybaraAnimator.SetBool("isNearPlayer", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("on triggerExit method");
            capybaraAnimator.SetBool("isNearPlayer", false);
        }
    }
}
