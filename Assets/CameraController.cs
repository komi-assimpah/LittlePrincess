using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 15f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        // Mouvements avec flèches
        if (Input.GetKey(KeyCode.UpArrow))
            movement += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow))
            movement += Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow))
            movement += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow))
            movement += Vector3.right;

        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // 🔍 Zoom avant : Entrée
        if (Input.GetKey(KeyCode.Return))
        {
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > minZoom)
                transform.position += direction * zoomSpeed * Time.deltaTime;
        }

        // 🔎 Zoom arrière : touche "<-" (Backspace)
        if (Input.GetKey(KeyCode.Backspace))
        {
            Vector3 direction = (transform.position - player.position).normalized;
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < maxZoom)
                transform.position += direction * zoomSpeed * Time.deltaTime;
        }
    }
}


