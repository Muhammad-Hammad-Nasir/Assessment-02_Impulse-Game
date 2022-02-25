using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject shockwave;

    private Rigidbody playerRb;
    private float horizontal;
    private float vertical;
    private float speed;
    // Set private variable values
    private float zLimit = 9.5f;
    private float bounce = 1.5f;
    private float radius = 6;
    private float knockForce = 2000;
    private bool isHit;
    private bool isFired;

    void Start()
    {
        speed = 7;
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();  // Player Movement controller
        ForcePower();  // Player power
        BoundPlayer();  // Player limited to set area
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        playerRb.AddForce(Vector3.right * horizontal * speed);
        playerRb.AddForce(Vector3.forward * vertical * speed);
    }

    void BoundPlayer()
    {
        if (transform.position.z >= zLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zLimit);
            playerRb.AddForce(Vector3.back * bounce, ForceMode.Impulse);
        }
        else if (transform.position.z <= -zLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zLimit);
            playerRb.AddForce(Vector3.forward * bounce, ForceMode.Impulse);
        }
    }

    void ForcePower()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isHit == false && isFired == false)  // Player power to push away enemies
        {
            isFired = true;
            GameObject exp = Instantiate(shockwave, transform.position, transform.rotation * Quaternion.Euler(90,90,90));
            exp.GetComponentInChildren<ParticleSystem>().Play();
            Destroy(exp, 0.8f);

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (Collider nearby in colliders)
            {
                Rigidbody enemyRb = nearby.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    enemyRb.AddExplosionForce(knockForce, transform.position, radius);
                }
            }
            StartCoroutine(FireDelay());  // Added delay for consecutive firing
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Check collision with enemy
        {
            // Player handicap for colliding with enemy
            Vector3 direction = Random.insideUnitCircle.normalized;
            playerRb.AddForce(direction * 2, ForceMode.Impulse);
            speed = 0;
            isHit = true;
            StartCoroutine(Delay()); // Time to end player handicap
            Destroy(collision.gameObject); // Enemy destroyed on collision
        }
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(0.8f); // Delay between fire rounds
        isFired = false;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f); // Delay for player handicap
        isHit = false;
        speed = 7;
    }
}
