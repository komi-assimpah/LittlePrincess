using UnityEngine;
using UnityEngine.UI;

public class CarPhotoTaker : MonoBehaviour
{
    public Camera photoCamera;
    public int photoResolution = 256;

    // Prendre une photo d'une voiture et la convertir en sprite
    public Sprite TakeCarPhoto(GameObject car)
    {
        // Configuration de la caméra
        RenderTexture renderTexture = new RenderTexture(photoResolution, photoResolution, 24);
        photoCamera.targetTexture = renderTexture;

        // Positionner la caméra
        Vector3 originalCarPosition = car.transform.position;
        Quaternion originalCarRotation = car.transform.rotation;

        // Positionner la voiture temporairement pour la photo
        car.transform.position = photoCamera.transform.position + photoCamera.transform.forward * 5;
        car.transform.rotation = Quaternion.Euler(0, 180, 0);

        // Prendre la photo
        photoCamera.Render();

        // Créer une texture à partir du rendu
        RenderTexture.active = renderTexture;
        Texture2D carTexture = new Texture2D(photoResolution, photoResolution, TextureFormat.RGB24, false);
        carTexture.ReadPixels(new Rect(0, 0, photoResolution, photoResolution), 0, 0);
        carTexture.Apply();

        // Restaurer l'état de la voiture
        car.transform.position = originalCarPosition;
        car.transform.rotation = originalCarRotation;

        // Nettoyer
        RenderTexture.active = null;
        photoCamera.targetTexture = null;
        Destroy(renderTexture);

        // Créer et retourner le sprite
        return Sprite.Create(carTexture, new Rect(0, 0, photoResolution, photoResolution), new Vector2(0.5f, 0.5f));
    }
}