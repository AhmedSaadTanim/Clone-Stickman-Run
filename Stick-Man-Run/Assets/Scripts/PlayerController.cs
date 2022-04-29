using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject nextLevel, enemy;
    [SerializeField] ParticleSystem fireworks;

    private GameManager gameManager;
    private float horizontalInput;
    private float sideRange = 1.2f, fixSpeed = 5;
    private bool isEndReached = false, finishPlay = false;
    private Vector3 lookDirection, fireworksPos = new Vector3(0, 1, 122);

    public float endStage = 118;
    public int mSpeed = 5;
    public int speed = 1;
    private void Start()
    {
        endStage = 118; 
        enemy = GameObject.Find("Enemy(Clone)");
        gameManager = GameObject.Find("Spawn Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.isGameActive)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        //player movement until reaching end stage
        if (Mathf.Abs(transform.position.z) <= endStage)
        {
            //move player forward at constant speed
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            //player control to move sideways 
            horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.left * horizontalInput * mSpeed * Time.deltaTime);

            //capping siderange
            if (transform.position.x < -sideRange)
            {
                transform.position = new Vector3(-sideRange, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > sideRange)
            {
                transform.position = new Vector3(sideRange, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (isEndReached)
            {
                if (gameManager.playerStrength >= gameManager.enemyStrength)
                {
                    if (!finishPlay)
                    {
                        PlayFireworks();
                    }
                    gameManager.playerAnimator.SetBool("hasWon", true);
                    enemy.GetComponent<Animator>().SetBool("isDead", true);
                    Invoke("ActivateNext", 3.5f);
                    gameManager.gameScreen.SetActive(false);
                }
                else
                {
                    enemy.GetComponent<Animator>().SetBool("hasWon", true);
                    gameManager.GameOver();
                }
            }
            else
            {
                lookDirection = (enemy.transform.position - transform.position).normalized;
                lookDirection.z = lookDirection.z * -1;
                lookDirection.y = 0;
                lookDirection.x = lookDirection.x * -1;
                transform.Translate(lookDirection * fixSpeed * Time.deltaTime);
                if (Mathf.Abs(transform.position.z) >= 122)
                    isEndReached = true;
            }

        }
    }
    
    private void ActivateNext()
    {
        nextLevel.SetActive(true);
    }

    private void PlayFireworks()
    {
        Instantiate(fireworks, fireworksPos, Quaternion.Euler(90, 0, 0));
        fireworks.Play();
        finishPlay = true;
    }
}
