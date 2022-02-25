using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject spawnPos1;
    public GameObject spawnPos2;
    public GameObject spawnPos3;
    public GameObject spawnPos4;
    public TextMeshProUGUI scoreText;

    public bool isEnemyDestroyed;

    private float startOffset = 2;
    private float repeatRate = 2f;
    private float score;

    void Start()
    {
        score = 0;
        InvokeRepeating("SpawnEnemy", startOffset, repeatRate);
    }

    void Update()
    {
        UpdateScore();
    }

    void SpawnEnemy()
    {
        if (player != null)
        {
            Instantiate(enemy, spawnPos1.transform.position, spawnPos1.transform.rotation);
            Instantiate(enemy, spawnPos2.transform.position, spawnPos2.transform.rotation);
            Instantiate(enemy, spawnPos3.transform.position, spawnPos3.transform.rotation);
            Instantiate(enemy, spawnPos4.transform.position, spawnPos4.transform.rotation);
        }
    }

    void UpdateScore()
    {
        if (isEnemyDestroyed == true)
        {
            score += 25;
            scoreText.text = "Score: " + score;
            isEnemyDestroyed = false;
        }
    }
}
