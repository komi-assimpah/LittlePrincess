using UnityEngine;

public class ConfettiTrigger : MonoBehaviour
{
    // R�f�rence au syst�me de particules de confettis
    public ParticleSystem confettiSystem;

    void Start()
    {
        // Assurez-vous que les confettis sont d�sactiv�s au d�but
        if (confettiSystem != null)
        {
            confettiSystem.Stop();
        }
    }

    // M�thode pour d�clencher les confettis
    public void TriggerConfetti()
    {
        if (confettiSystem != null)
        {
            // Active les confettis
            confettiSystem.Play();
        }
    }
}
