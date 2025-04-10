using UnityEngine;

public class MultiCameraManager : MonoBehaviour
{
    public Camera playerCamera;  // 🎥 Caméra du joueur (pleine écran)
    public Camera aiCamera1;     // 🎥 Caméra de l'IA 1
    public Camera aiCamera2;     // 🎥 Caméra de l'IA 2

    void Start()
    {
        // Configuration de la caméra du joueur (plein écran)
        playerCamera.rect = new Rect(0f, 0f, 1f, 1f);

        // Caméra IA 1 (en haut à gauche)
        aiCamera1.rect = new Rect(0f, 0.75f, 0.25f, 0.25f);

        // Caméra IA 2 (en haut à droite)
        aiCamera2.rect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
    }
}
