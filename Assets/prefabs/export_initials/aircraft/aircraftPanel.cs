using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aircraftPanel : MonoBehaviour
{
    public Animator animator;           
    public GameObject infoPanel;     

    private void Start()
    {
        infoPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isNearPlayer", true);
            infoPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isNearPlayer", false);
            infoPanel.SetActive(false);
        }
    }
}
