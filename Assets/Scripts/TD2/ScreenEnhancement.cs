using UnityEngine;
using UnityEngine.UI;

public class ScreenEnhancement : MonoBehaviour
{
    public Sprite screenBackground; // Fond de l'�cran
    public Material screenMaterial; // Mat�riau de l'�cran (si n�cessaire pour d'autres effets)
    public Texture2D glareTexture; // Texture de brillance ou de r�flexion
    public Sprite frameTexture; // Texture pour la bordure avec coins arrondis

    void Start()
    {
        // Ajout de la bordure autour de l'�cran
        AddScreenFrame();

        // Application des effets de l'�cran
        ApplyScreenTexture();
    }

    void AddScreenFrame()
    {
        // Cr�er l'image de bordure avec des coins arrondis (utiliser un sprite pour la bordure)
        if (frameTexture != null)
        {
            Image frameImage = gameObject.AddComponent<Image>();
            frameImage.sprite = frameTexture; // Utiliser une image avec une bordure arrondie
            frameImage.type = Image.Type.Sliced; // Utiliser le type "Sliced" pour que l'image s'adapte au rectangle
            frameImage.color = new Color(1f, 1f, 1f, 1f); // La couleur de la bordure (modifiable selon le style)

            RectTransform frameTrans = frameImage.rectTransform;
            frameTrans.anchorMin = new Vector2(0, 0);
            frameTrans.anchorMax = new Vector2(1, 1);
            frameTrans.sizeDelta = Vector2.zero; // Pas besoin de taille additionnelle si tu utilises une image avec coins arrondis
        }
        else
        {
            // Si aucune texture n'est d�finie, appliquer un simple cadre gris
            Image frameImage = gameObject.AddComponent<Image>();
            frameImage.color = new Color(0.1f, 0.1f, 0.1f); // Bordure gris fonc�

            RectTransform frameTrans = frameImage.rectTransform;
            frameTrans.anchorMin = new Vector2(0, 0);
            frameTrans.anchorMax = new Vector2(1, 1);
            frameTrans.sizeDelta = new Vector2(20, 20); // Ajouter une bordure de 20 pixels
        }
    }

    void ApplyScreenTexture()
    {
        // Ajouter un effet de brillance/reflet subtil
        RawImage screenEffect = gameObject.AddComponent<RawImage>();
        screenEffect.color = new Color(1f, 1f, 1f, 0.1f); // Subtile superposition de couleur (ajustable)
        screenEffect.texture = glareTexture != null ? glareTexture : CreateScreenGlareTexture();
    }

    // Cr�er une texture de glare/reflet si aucune texture n'est fournie
    Texture2D CreateScreenGlareTexture()
    {
        Texture2D glareTexture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        for (int x = 0; x < glareTexture.width; x++)
        {
            for (int y = 0; y < glareTexture.height; y++)
            {
                float glareIntensity = Mathf.PerlinNoise(x * 0.1f, y * 0.1f) * 0.2f;
                glareTexture.SetPixel(x, y, new Color(1f, 1f, 1f, glareIntensity));
            }
        }
        glareTexture.Apply();
        return glareTexture;
    }
}
