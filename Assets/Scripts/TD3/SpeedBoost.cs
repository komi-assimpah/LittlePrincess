using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float boostAmount = 20f; // Vitesse suppl�mentaire
    public float boostDuration = 2f; // Dur�e du boost
    public GameObject speedEffectPrefab; // Effet visuel du boost
    public AudioClip boostSound; // Son du boost

    private void OnTriggerEnter(Collider other)
    {
        CarMovement car = other.GetComponent<CarMovement>();

        if (car != null)
        {
            StartCoroutine(ApplySpeedBoost(car));
            Destroy(gameObject); // D�truit l'objet bonus apr�s activation
        }

        CarEffects carEffects = car.GetComponent<CarEffects>();
        if (carEffects != null)
        {
            carEffects.PlayBoostSound();
        }
    }

    private IEnumerator ApplySpeedBoost(CarMovement car)
    {
        // Augmente la vitesse
        car.forwardMoveSpeed += boostAmount;

        // Instancier l'effet de boost sur la voiture
        GameObject effect = Instantiate(speedEffectPrefab, car.transform);
        effect.transform.localPosition = Vector3.zero;

        // Jouer le son du boost
        AudioSource audioSource = car.GetComponent<AudioSource>();
        if (audioSource != null && boostSound != null)
        {
            audioSource.PlayOneShot(boostSound);
        }

        // Attendre la fin du boost
        yield return new WaitForSeconds(boostDuration);

        // R�tablir la vitesse normale
        car.forwardMoveSpeed -= boostAmount;

        // Supprimer l'effet visuel
        if (effect != null)
        {
            Destroy(effect);  // D�truire l'effet visuel
        }
    }
}
