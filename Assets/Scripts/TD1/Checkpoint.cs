using UnityEngine.Events;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent<GameObject, Checkpoint> onCheckpointEnter;

    void OnTriggerEnter(Collider collider)
    {
        // if entering object is tagged as the Player
        /*        if (collider.gameObject.tag == "Player")
                {
                    // fire an event giving the entering gameObject and this checkpoint
                    onCheckpointEnter.Invoke(collider.gameObject, this);
                }*/

        // Or can be done by getting a component that means this is a car, like CarController

        if (collider.gameObject.GetComponent<CarIdentity>() == null)
        {
            return;
        }
        CarIdentity carIdentity = collider.gameObject.GetComponent<CarIdentity>();
        if (carIdentity != null)
        {
            Debug.Log($"Checkpoint triggered by: {carIdentity.carName}");
            onCheckpointEnter.Invoke(collider.gameObject, this);
        }
        else
        {
            Debug.LogError($"CarIdentity component not found on GameObject: {collider.gameObject.name}");
        }

    }
}