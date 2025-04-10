using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    public Rigidbody rg;
    public float forwardMoveSpeed;
    public float backwardMoveSpeed;
    public float steerSpeed;

    public Vector2 input;

    public GameObject mainPathContainer;
    public GameObject mainPath2Container;
    public GameObject altPath1Container;
    public GameObject altPath2Container;

    private List<Transform> allWaypoints = new List<Transform>();

    public LayerMask trackLayer;
    public float checkInterval = 2f;
    public float rayDistance = 5f;

    private bool isTeleporting = false;

    void Start()
    {

        LoadWaypoints();
        StartCoroutine(CheckIfOutOfTrack());
    }

    public void SetInputs(Vector2 input)
    {
        this.input = input;
    }

    void FixedUpdate()
    {
        if (isTeleporting) return;  // Empêcher le mouvement pendant la téléportation

        float speed = input.y > 0 ? forwardMoveSpeed : backwardMoveSpeed;
        rg.AddForce(this.transform.forward * input.y * speed, ForceMode.Acceleration);

        float rotation = input.x * steerSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, rotation, 0, Space.World);
    }


    void LoadWaypoints()
    {
        allWaypoints.Clear();
        AddWaypointsFromContainer(mainPathContainer);
        AddWaypointsFromContainer(mainPath2Container);
        AddWaypointsFromContainer(altPath1Container);
        AddWaypointsFromContainer(altPath2Container);
    }

    void AddWaypointsFromContainer(GameObject container)
    {
        if (container == null) return;
        foreach (Transform waypoint in container.transform)
        {
            allWaypoints.Add(waypoint);
        }
    }

    IEnumerator CheckIfOutOfTrack()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (!IsOnTrack())
            {
                Debug.Log(gameObject.name + " est hors-piste ! Repositionnement...");
                StartCoroutine(TeleportToNearestWaypoint());
            }
        }
    }

    bool IsOnTrack()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance))
        {
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Track");
        }
        return false;
    }

    IEnumerator TeleportToNearestWaypoint()
    {

        Transform nearestWaypoint = GetNearestWaypoint();
        if (nearestWaypoint != null)
        {
            rg.velocity = Vector3.zero;
            transform.position = nearestWaypoint.position;

            // Ralentir légèrement après la téléportation
            yield return new WaitForSeconds(0.5f);
            isTeleporting = true;
            yield return new WaitForSeconds(0.5f);  // Durée du ralentissement
            isTeleporting = false;
        }
    }


    Transform GetNearestWaypoint()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform waypoint in allWaypoints)
        {
            float dist = Vector3.Distance(transform.position, waypoint.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = waypoint;
            }
        }
        return nearest;
    }

    public int GetSpeed()
    {
        return (int)rg.velocity.magnitude;
    }

    public void ClearWaypoints()
    {
        allWaypoints.Clear();
    }

    public void AddWaypoint(Transform wp)
    {
        allWaypoints.Add(wp);
    }

}
