using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AmbianceTrigger : MonoBehaviour
{
    public AudioSource ambianceSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !ambianceSource.isPlaying)
        {
            ambianceSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && ambianceSource.isPlaying)
        {
            ambianceSource.Stop();
        }
    }
}
