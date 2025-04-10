using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIControls : MonoBehaviour
{
    public UnityEvent<Vector2> onInput;

    public Transform[] waypointsHolders;
    private List<List<Transform>> allWaypoints = new List<List<Transform>>();
    private List<Transform> currentWaypoints;
    private Transform nextWaypoint;
    private Vector3 nextWaypointPosition;

    public float maxDistanceToTarget = 5f;
    public float randomJitterOnPosition = 0.5f;
    public float aggressiveness = 0.5f;
    public float skillLevel = 0.5f;

    private Vector3 lastPosition;
    private float blockTimer = 0f;
    private float detectionTime = 2f;
    private float minSpeed = 0.1f;
    private bool hasStartedFirstLap = false;

    public int mainPathIndex = 0;

    private enum AIState { Patrolling, Blocked, Attacking, Defending, Avoiding }
    private AIState currentState = AIState.Patrolling;

    [Header("Obstacle Detection")]
    public float rayLength = 5f;
    public LayerMask obstacleLayer;

    [Header("AI Personality")]
    public AIBehaviorType behaviorType = AIBehaviorType.Aggressive;

    private void Awake()
    {
        foreach (Transform holder in waypointsHolders)
        {
            var waypoints = new List<Transform>(holder.GetComponentsInChildren<Transform>());
            waypoints.Remove(holder);
            allWaypoints.Add(waypoints);
        }

        SelectPathForFirstLap();
    }

    private void Start()
    {
        SelectWaypoint(currentWaypoints[0]);
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;
        float distanceToTarget = Vector3.Distance(transform.position, nextWaypointPosition);

        // Mise à jour du timer de blocage
        if (Vector3.Distance(transform.position, lastPosition) < minSpeed)
            blockTimer += Time.deltaTime;
        else
            blockTimer = 0f;

        // FSM : changement d’état selon conditions
        if (blockTimer >= detectionTime)
            currentState = AIState.Blocked;
        else if (DetectObstacleAhead())
            currentState = AIState.Avoiding;
        else
            currentState = AIState.Patrolling;

        switch (currentState)
        {
            case AIState.Patrolling:
                input = HandlePatrolling(distanceToTarget);
                break;
            case AIState.Blocked:
                input = HandleBlocked();
                break;
            case AIState.Avoiding:
                input = HandleAvoiding();
                break;
            case AIState.Attacking:
                input = HandleAttacking();
                break;
            case AIState.Defending:
                input = HandleDefending();
                break;
        }

        lastPosition = transform.position;
        onInput?.Invoke(input);
    }

    private Vector2 HandlePatrolling(float distanceToTarget)
    {
        // Suivre la trajectoire comme dans l'existant
        if (distanceToTarget < maxDistanceToTarget)
        {
            int nextIndex = currentWaypoints.IndexOf(nextWaypoint) + 1;
            if (nextIndex < currentWaypoints.Count)
                SelectWaypoint(currentWaypoints[nextIndex]);
            else
            {
                if (hasStartedFirstLap)
                    SelectRandomPath();
                SelectWaypoint(currentWaypoints[0]);
            }
        }

        Vector3 diff = nextWaypointPosition - transform.position;
        float forward = Vector3.Dot(diff, transform.forward);
        float right = Vector3.Dot(diff, transform.right);

        // Détecter le prochain virage
        float turnAngle = CalculateTurnAngle();

        // Ajuster la vitesse en fonction du virage
        Vector2 input = new Vector2(right, forward).normalized;
        return AdjustDrivingBehavior(input, forward, turnAngle);
    }

    private float CalculateTurnAngle()
    {
        // Vérifier si nous avons assez de waypoints (au moins 3 points : le courant, le suivant, et un autre après)
        int currentWaypointIndex = currentWaypoints.IndexOf(nextWaypoint);
        if (currentWaypointIndex >= 0 && currentWaypointIndex + 2 < currentWaypoints.Count)
        {
            Vector3 currentToNext = currentWaypoints[currentWaypointIndex + 1].position - transform.position;
            Vector3 nextToNext = currentWaypoints[currentWaypointIndex + 2].position - currentWaypoints[currentWaypointIndex + 1].position;

            return Vector3.Angle(currentToNext, nextToNext);
        }

        return 0f; // Si pas assez de waypoints pour calculer l'angle
    }


    private Vector2 AdjustDrivingBehavior(Vector2 input, float forwardComponent, float turnAngle)
    {
        // Ajuster la vitesse selon l'agressivité et la difficulté du virage
        input.y *= Mathf.Lerp(0.6f, 1.2f, aggressiveness); // Agressivité = vitesse
        input.x *= Mathf.Lerp(0.3f, 1f, skillLevel); // Skill = précision virage

        // Ralentir dans les virages serrés
        if (turnAngle > 30f)  // Si l'angle est plus grand que 30°, c'est un virage serré
        {
            input.y *= 0.5f;  // Réduire la vitesse
        }

        // Comportement supplémentaire en fonction de la personnalité
        switch (behaviorType)
        {
            case AIBehaviorType.Aggressive:
                input.y *= 1.5f;  // Plus rapide
                break;
            case AIBehaviorType.Defensive:
                input.y *= 0.7f;  // Plus prudent
                break;
            case AIBehaviorType.Opportunistic:
                input.y *= 1.0f;  // Comportement équilibré
                break;
        }

        return input;
    }

    private Vector2 HandleBlocked()
    {
        // Si bloqué, reculer et essayer de se libérer
        return new Vector2(Random.Range(-1f, 1f), 1f); // Reculer + tourner
    }

    private Vector2 HandleAvoiding()
    {
        // Comportement d'évitement : changer légèrement de trajectoire
        return new Vector2(Random.value > 0.5f ? 1f : -1f, 0.5f); // Tourner pour éviter
    }

    private Vector2 HandleAttacking()
    {
        // Attaquer si proche d'un adversaire
        return new Vector2(0, 1f); // Aller tout droit et attaquer
    }

    private Vector2 HandleDefending()
    {
        // Défendre la trajectoire : fermer l'angle
        return new Vector2(-0.5f, 1f); // Ralentir et défendre
    }

    private bool DetectObstacleAhead()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, rayLength, obstacleLayer);
    }

    private void SelectWaypoint(Transform waypoint)
    {
        nextWaypoint = waypoint;
        nextWaypointPosition = nextWaypoint.position +
            new Vector3(Random.Range(-randomJitterOnPosition, randomJitterOnPosition), 0,
                        Random.Range(-randomJitterOnPosition, randomJitterOnPosition));
    }

    private void SelectPathForFirstLap()
    {
        if (!hasStartedFirstLap)
        {
            currentWaypoints = allWaypoints[mainPathIndex];
            hasStartedFirstLap = true;
        }
        else
        {
            SelectRandomPath();
        }
    }

    private void SelectRandomPath()
    {
        int index = Random.Range(0, allWaypoints.Count);
        currentWaypoints = allWaypoints[index];
    }
}

public enum AIBehaviorType
{
    Aggressive,
    Defensive,
    Opportunistic
}
