using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    [SerializeField] Material green, red, yellow;
    [SerializeField] ParticleSystem explosion;

    private GameManager gameManager;
    private new Renderer renderer;
    private int counter = 0, size;
    private string currentColor = "PickupR";
    private Vector3 leastVector = new Vector3(0.12f, 0.12f, 0.12f);
    private Vector3 playerPos, explosionOffset = new Vector3(.5f, .5f, -0.5f);
    private void Start()
    {
        renderer = GameObject.Find("Player").GetComponentInChildren<Renderer>();
        gameManager = GameObject.Find("Spawn Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Red" || other.gameObject.tag == "Green" || other.gameObject.tag == "Yellow")
        {
            Destroy(other.gameObject);
            /*for (int i = 0; i < 11; i++)
                Debug.Log("WallX" + (i + 1) + ":" + gameManager.heightArray[i]);*/
            size = gameManager.heightArray[counter];
/*            Debug.Log(counter + 1);
            Debug.Log("Encounter " + (counter+1) + " : " + other.gameObject.tag + size + "wall vs strength " + gameManager.playerStrength);*/
            if (gameManager.playerStrength < size)
            {
                Explode();
                gameManager.GameOver();
            }
            else
            {
                if (gameManager.playerStrength == size)
                    size--;
                ScaleChange(false, size);
                if (other.gameObject.tag == "Green")
                {
                    renderer.material = green;
                    currentColor = "PickupG";
                }
                else if (other.gameObject.tag == "Red")
                {
                    renderer.material = red;
                    currentColor = "PickupR";
                }
                else
                {
                    renderer.material = yellow;
                    currentColor = "PickupY";
                }
            }
            counter += 1;
        }
        else if (other.gameObject.tag == "PickupR" || other.gameObject.tag == "PickupG" || other.gameObject.tag == "PickupY" || other.gameObject.tag == "Coin")
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.UpdateCoinCounter();
            }
            else
            {
                if (currentColor == other.gameObject.tag)
                {
                    ScaleChange(true, 1);
                }
                else
                {
                    ScaleChange(false, 1);
                }
            }
            Destroy(other.gameObject);
        }
    }

    private void ScaleChange(bool increase, int multiple)
    {
        if (increase)
        {
            transform.localScale = transform.localScale + leastVector * multiple;
            gameManager.playerStrength += multiple;
        }
        else
        {
            if (gameManager.playerStrength > 1)
            {
                transform.localScale = transform.localScale - leastVector * multiple;
                gameManager.playerStrength -= multiple;
            }
        }
        //Debug.Log("Size:" + multiple + " Strength: " + gameManager.playerStrength);
    }

    private void Explode()
    {
        playerPos = GameObject.Find("Player").transform.position + explosionOffset;
        explosion.transform.localScale = GameObject.Find("Player").transform.localScale;
        Instantiate(explosion, playerPos, Quaternion.Euler(90,0,0));
        explosion.Play();
    }
}
