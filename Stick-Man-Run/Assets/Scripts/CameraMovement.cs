using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float camSpeed, camRotationSpeed;

    private PlayerController playerController;
    private Vector3 offset = new Vector3(2.5f, 2.5f, -2.5f);
    private Vector3 waypoint = new Vector3(4.1f, 3.26f, 121f);

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void LateUpdate()
    {
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
        transform.position = player.transform.position + offset;
    }

    void EndShot()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoint, Time.deltaTime * camSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), camRotationSpeed * Time.deltaTime);
    }
}
