using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float camSpeed, camRotationSpeed;
    [SerializeField] float fraction;
    [SerializeField] Camera myCam;

    private PlayerController playerController;
    private float width;
    private Vector3 offset = new Vector3(2.5f, 2.5f, -2.5f);
    private Vector3 waypoint = new Vector3(4.1f, 3.26f, 121f);
    private Vector3 screenOffset = new Vector3(1.036f, 2.5f, -2.5f);

    private void Start()
    {

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
        /*width = Screen.width * fraction;
        screenOffset.x = width;*/

        if (player.transform.position.z < playerController.endStage)
        {
            FollowPlayer();
        }
        else
        {
            EndShot();
        }
    }

    void FollowPlayer()
    {
        if (Screen.width < 1080)
        {
            transform.position = player.transform.position + screenOffset;
            myCam.fieldOfView = 75f;
        }
        else
        {
            transform.position = player.transform.position + offset;
            myCam.fieldOfView = 60f;
        }
    }

    void EndShot()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint, Time.deltaTime * camSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), camRotationSpeed * Time.deltaTime);
    }

    /*private (Vector3 center, float size) CalculateOrthoSize()
    {
        var bounds = new Bounds();
        foreach (var col in FindObjectsOfType<Collider2D>()) bounds.Encapsulate(col.bounds);
        bounds.Expand(buffer)
    }*/
}
