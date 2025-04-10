using UnityEngine;
using System.Collections.Generic;

public class WaypointGenerator : MonoBehaviour
{
    public Transform trackMesh; // R�f�rence au MeshCollider du circuit
    public GameObject waypointPrefab; // Un prefab de waypoint (Empty GameObject ou sph�re)
    public int numberOfWaypoints = 30; // Ajuste selon la longueur du circuit
    public LayerMask trackLayer; // Assigne le layer de la piste pour le Raycast

    private List<Transform> waypoints = new List<Transform>();

    void Start()
    {
        GenerateWaypoints();
    }

    void GenerateWaypoints()
    {
        if (transform.childCount > 0) // V�rifie s'il y a d�j� des waypoints
        {
            Debug.Log("Waypoints d�j� g�n�r�s !");
            return;
        }

        if (trackMesh == null || waypointPrefab == null)
        {
            Debug.LogError("Assigne le circuit et le prefab de waypoint !");
            return;
        }

        MeshCollider trackCollider = trackMesh.GetComponent<MeshCollider>();
        if (trackCollider == null)
        {
            Debug.LogError("Ajoute un MeshCollider sur la piste !");
            return;
        }

        for (int i = 0; i < numberOfWaypoints; i++)
        {
            float t = (float)i / numberOfWaypoints;
            Vector3 estimatedPos = GetPointAroundTrack(t);

            // Utilisation de Raycast pour ajuster la hauteur du waypoint
            if (Physics.Raycast(estimatedPos + Vector3.up * 10, Vector3.down, out RaycastHit hit, 20, trackLayer))
            {
                estimatedPos = hit.point; // Ajuste � la surface du circuit
            }

            GameObject waypoint = Instantiate(waypointPrefab, estimatedPos, Quaternion.identity);
            waypoint.transform.SetParent(transform);
            waypoint.name = "Waypoint_" + i;
            waypoints.Add(waypoint.transform);
        }

        Debug.Log(waypoints.Count + " waypoints g�n�r�s !");
    }

    // Simulation d'un chemin circulaire autour du centre du circuit
    Vector3 GetPointAroundTrack(float t)
    {
        float angle = t * 360f; // Cr�e un cercle autour du circuit
        float radius = 50f; // Ajuste selon la taille du circuit

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        return new Vector3(x, 5, z); // 5 unit�s en hauteur pour �viter d'�tre sous la piste
    }
}
