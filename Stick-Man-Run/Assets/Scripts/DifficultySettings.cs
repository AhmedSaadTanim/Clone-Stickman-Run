using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySettings : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;
    [SerializeField] int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("Spawn Manager").GetComponent<GameManager>();
        button.onClick.AddListener(SetDifficulty);
    }

    void SetDifficulty()
    {
        gameManager.StartGame(difficulty, 1, 0, "Start");
    }
}
