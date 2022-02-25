using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))  // Detect collision with enemy to add player score points
        {
            gameManager.isEnemyDestroyed = true;
        }
        Destroy(collision.gameObject);  // Delete any game object that collides with the walls
    }
}
