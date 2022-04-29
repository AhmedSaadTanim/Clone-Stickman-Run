using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] wallPrefabs, pickupPrefabs, triggerPrefabs;

    private PlayerController playerController;
    private int index, gameDifficulty, gameLevel;
    private int speed = 10, endPos = 116;
    private float currentPos, initialPos, span = 10;
    private float enemyPosX = 0, enemyPosY = 0, enemyPosZ = 123;
    private Vector3 wallSpawnLocation, triggerSpawnLocation = new Vector3(0, 0, 0.7f);

    public GameObject titleScreen, gameScreen, gameOverScreen, enemyPrefab;
    public Animator playerAnimator;

    public Button continueButton;
    public TextMeshProUGUI lifeText, levelText, coinText;
    public List<int> heightArray = new List<int>();
    public bool isGameActive;
    public int gameCoins = 0, wallHeight, playerStrength = 1, lifeCount = 10, maxEnemyScale = 5, enemyStrength;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();

        currentPos = transform.position.z;
        initialPos = transform.position.z;
        wallSpawnLocation = new Vector3(0, 0, currentPos);

        //spawning enemy and objects
        SpawnEnemy();
        while(transform.position.z < endPos)
        {
            MoveSpawnManager();
            UpdateCurrentWallLine();
        }
        /*for (int i = 0; i < 11; i++)
            Debug.Log("Wall" + (i+1) + ":" +heightArray[i]);*/
    }

    private void Update()
    {
        if(isGameActive)
        {
            //MoveSpawnManager();
            //UpdateCurrentWallLine();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 pos = new Vector3(enemyPosX, enemyPosY, enemyPosZ);
        Vector3 sizeIncr = new Vector3(0.25f, 0.25f, 0.25f);

        enemyStrength = GenerateEnemyScale();
        enemyPrefab.transform.localScale = sizeIncr * enemyStrength;

        Instantiate(enemyPrefab, pos, Quaternion.Euler(0, 180, 0));
    }

    private int GenerateEnemyScale()
    {
        return Random.Range(1,maxEnemyScale);
    }

    
    void MoveSpawnManager()
    {
        if (transform.position.z < endPos)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void UpdateCurrentWallLine()
    {
        //generating walls and pickups after fixed distance
        int wallLine = (int)(transform.position.z - currentPos);
        if (wallLine == span)
        {
            currentPos = transform.position.z;
            wallSpawnLocation.z = currentPos;
            generateWall();
        }
        else
        {
            if((int)(transform.position.z - initialPos) == 1)
            {
                initialPos = transform.position.z;
                generatePickups();
            }
        }
    }

    void generateWall()
    {
        //which color object to spawn
        index = FindRandomIndex(0,wallPrefabs.Length);

        //trigger for the wall
        Instantiate(triggerPrefabs[index], wallSpawnLocation - triggerSpawnLocation, Quaternion.Euler(0, 0, 0));

        //determining random size of wall
        wallHeight = Random.Range(0, 5) + 1;
        heightArray.Add(wallHeight);
        float height = 0.132f;
        float heightSpan = 0.25f;
        for (int i = 1; i <= wallHeight; i++, height += heightSpan)
        {
            wallSpawnLocation.y = height;
            Instantiate(wallPrefabs[index], wallSpawnLocation, Quaternion.Euler(0, 0, 0));
        }
    }

    void generatePickups()
    {
        int temp = 3;
        float xPos = -1, yPos;
        for(int i = 0; i<temp; i++)
        {
            index = FindRandomIndex(0, pickupPrefabs.Length);
            if (index < 3)
                yPos = 0f;
            else
                yPos = -0.8f;
            Instantiate(pickupPrefabs[index], new Vector3(xPos, yPos, transform.position.z), Quaternion.Euler(0, 180, 0));
            xPos += 1;
        }
    }

    private int FindRandomIndex(int min, int max)
    {
        return Random.Range(min, max);
    }


    public void UpdateCoinCounter()
    {
        gameCoins++;
        coinText.text = gameCoins.ToString();
    }

    public void StartGame(int difficulty, int level, int coinCount, string text)
    {
        //Debug.Log(text + difficulty + level);
        gameDifficulty = difficulty;
        gameLevel = level;

        levelText.text = "LEVEL " + gameLevel;
        coinText.text = coinCount.ToString();
        
        //player speed
        playerController.speed = playerController.speed * difficulty + level;
        playerController.mSpeed = playerController.mSpeed * difficulty + level;
        //spawn manager speed
        speed = speed * difficulty + level;

        playerAnimator.SetInteger("Speed", playerController.speed);
        isGameActive = true;
        titleScreen.SetActive(false);
        gameScreen.SetActive(true);
    }
    public void GameOver()
    {
        playerAnimator.SetBool("isDead", true);
        isGameActive = false;
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        lifeText.text = "LIFE X " + lifeCount;
    }

    public void LevelComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Debug.Log("Complete Level: " + gameDifficulty + gameLevel + gameCoins);
       // StartGame(gameDifficulty, gameLevel + 1, gameCoins, "Complete");
    }


    public void ResetLevel()
    {
        if (lifeCount > 0)
        {
            lifeCount--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Debug.Log("Reset Level: " + gameDifficulty + gameLevel + gameCoins);
            //StartGame(gameDifficulty, gameLevel, gameCoins, "Reset");
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
