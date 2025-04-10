using UnityEngine;

public class SmoothFollowPlayer : MonoBehaviour
{
    public Transform targetCar; // 🚗 Voiture à suivre
    public float height = 5f; // Hauteur de la caméra
    public float distance = 10f; // Distance derrière la voiture
    public float smoothSpeed = 5f; // Vitesse de transition fluide

    void LateUpdate()
    {
        if (targetCar != null)
        {
            // Calcul dynamique pour toujours être derrière la voiture
            Vector3 desiredPosition = targetCar.position - targetCar.forward * distance + Vector3.up * height;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Regarder légèrement en avant pour une meilleure vue
            transform.LookAt(targetCar.position + targetCar.forward * 5f);
        }
    }
}
