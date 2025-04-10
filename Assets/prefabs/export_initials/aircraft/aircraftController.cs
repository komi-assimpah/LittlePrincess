using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aircraftController : MonoBehaviour
{
    public GameObject objectToReveal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("on aircraft triggerEnter method");
            objectToReveal.SetActive(true);
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("on aircraft triggerExit method");
            objectToReveal.SetActive(false);
        }
    }
}
