using UnityEngine;

public class ConfettiTrigger : MonoBehaviour
{
    // Référence au système de particules de confettis
    public ParticleSystem confettiSystem;

    void Start()
    {
        // Assurez-vous que les confettis sont désactivés au début
        if (confettiSystem != null)
        {
            confettiSystem.Stop();
        }
    }

    // Méthode pour déclencher les confettis
    public void TriggerConfetti()
    {
        if (confettiSystem != null)
        {
            // Active les confettis
            confettiSystem.Play();
        }
    }
}
