using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenTrigger : MonoBehaviour
{
    public Animator kitchenAnimator;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("on kitchen triggerEnter method");
            kitchenAnimator.SetBool("isNearPlayer", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("on kitchen triggerExit method");
            kitchenAnimator.SetBool("isNearPlayer", false);
        }
    }
}
