using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPrincess : MonoBehaviour
{
    public Transform player; // Le personnage à suivre
    public Vector3 marginFromPlayer; // Le décalage entre la caméra et le joueur

    void Update()
    {
        transform.position = player.position + marginFromPlayer;
    }
}



