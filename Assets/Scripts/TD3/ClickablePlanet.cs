using UnityEngine;

public class ClickablePlanet : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log($"Tu as cliqu� sur {gameObject.name} !");
        // Tu peux d�clencher une animation ou ouvrir un panneau ici
    }
}
