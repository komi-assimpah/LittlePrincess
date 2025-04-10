using UnityEngine;

public class DynamicFollowPlayer : MonoBehaviour
{
    public Transform targetCar; // 🚗 Voiture à suivre
    public Rigidbody targetRb; // 🚗 Rigidbody pour récupérer la vitesse
    public Vector3 baseOffset = new Vector3(0, 5, -10); // Décalage de base
    public float smoothSpeed = 5f; // Vitesse de transition fluide
    public float maxSpeed = 50f; // Vitesse max estimée

    void LateUpdate()
    {
        if (targetCar != null && targetRb != null)
        {
            float speed = targetRb.velocity.magnitude; // Récupère la vitesse actuelle

            // Ajuste l’offset en fonction de la vitesse (recule la caméra légèrement)
            Vector3 dynamicOffset = baseOffset + new Vector3(0, 0, -speed / maxSpeed * 5f);
            Vector3 desiredPosition = targetCar.position + dynamicOffset;

            // Mouvement fluide
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Regarder légèrement devant la voiture pour éviter les angles bizarres
            transform.LookAt(targetCar.position + targetCar.forward * 5f);
        }
    }
}
