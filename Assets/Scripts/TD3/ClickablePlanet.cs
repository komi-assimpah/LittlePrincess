using UnityEngine;

public class ClickablePlanet : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log($"Tu as cliqué sur {gameObject.name} !");
        // Tu peux déclencher une animation ou ouvrir un panneau ici
    }
}
