using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject player;
    private Rigidbody enemyRb;
    private Vector3 lookDirection;

    // Set private variable values
    private float rotationSpeed = 5;
    private float speed = 4.0f;
    private float yLimit = 5;
    private float xLimit = 25;
    private float zLimit = 15;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  // Initialize GameManager Script
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        EnemyMovement();  // Gameobject AI movement
        DestroyOutOfBounds();  // GameObject destroyed when out of set limit
    }

    void LateUpdate()
    {
        AvoidEdges();
    }

    void EnemyMovement()
    {
        if (player != null)
        {
            //rotate to look at the player
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotationSpeed * Time.deltaTime);

            //move towards the player
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    void DestroyOutOfBounds()
    {
        if (transform.position.y < -yLimit || transform.position.x > xLimit || transform.position.x < -xLimit || transform.position.z > zLimit || transform.position.z < -zLimit)
        {
            gameManager.isEnemyDestroyed = true;  // For adding player score
            Destroy(gameObject);
        }
    }

    void AvoidEdges()
    {
        RaycastHit hit;  // Initialize Ray
        // Declare directions
        var forward = transform.TransformDirection(Vector3.forward);
        var rightSide = transform.TransformDirection(Vector3.right);
        var leftSide = transform.TransformDirection(Vector3.left);

        // Check for raycast hits
        if (Physics.Raycast(transform.position, forward, out hit, 1.5f))  // Check Front Side
        {
            if (hit.collider.tag == "LeftWall")
            {
                transform.Rotate(Vector3.up * 10);
            }
            else if (hit.collider.tag == "RightWall")
            {
                transform.Rotate(Vector3.down * 10);
            }
        }
        else if (Physics.Raycast(transform.position, rightSide, out hit, 1.5f))  // Check Right Side
        {
            if (hit.collider.tag == "LeftWall" || hit.collider.tag == "RightWall")
            {
                transform.Rotate(Vector3.down * 10);
            }
        }
        else if (Physics.Raycast(transform.position, leftSide, out hit, 1.5f))  // Check Left Side
        {
            if (hit.collider.tag == "LeftWall" || hit.collider.tag == "RightWall")
            {
                transform.Rotate(Vector3.up * 10);
            }
        }
    }
}
