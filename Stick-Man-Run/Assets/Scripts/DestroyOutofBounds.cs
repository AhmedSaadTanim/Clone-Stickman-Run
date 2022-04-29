using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutofBounds : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerController playerController;
    private Vector3 offset = new Vector3(0, 0.23f, -3f);
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
    }

    void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }
}
