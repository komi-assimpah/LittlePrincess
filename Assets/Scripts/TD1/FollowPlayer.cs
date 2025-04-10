using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform targetCar; // 🚗 Voiture à suivre
    public Vector3 offset = new Vector3(0, 5, -10); // Décalage de la caméra

    void LateUpdate()
    {
        if (targetCar != null)
        {
            transform.position = targetCar.position + offset;
            transform.LookAt(targetCar.position);
        }
    }
}
