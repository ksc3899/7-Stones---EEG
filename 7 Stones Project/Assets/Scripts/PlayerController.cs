using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float force;
    public float sidewardsVelocity;
    public float minimumXSpawnCoordinate, maximumXSpawnCoordinate;
    public AudioClip strikeClip;

    private Rigidbody playerRigibody;
    private bool shot = false;
    private bool strike = false;
    private bool collide = true;
    private bool stones = false;

    private void Start()
    {
        playerRigibody = GetComponent<Rigidbody>();
        playerRigibody.velocity = new Vector3(sidewardsVelocity, 0f, 0f);
    }

    private void Update()
    {
        if (GameManager.gameRunning && !shot)
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                shot = true;
                GameManager.chances++;
                strike = true;
                this.GetComponent<AudioSource>().Play();
            }
        }

        if (shot)
        {
            playerRigibody.useGravity = true;
            playerRigibody.AddForce(Vector3.forward * force);
        }
        else
        {
            if (transform.position.x < minimumXSpawnCoordinate || transform.position.x > maximumXSpawnCoordinate)
            {
                if (this.gameObject.transform.position.x > 1.1f && sidewardsVelocity > 0)
                    sidewardsVelocity = -sidewardsVelocity;
                else if (this.gameObject.transform.position.x < 1.1f && sidewardsVelocity < 0)
                    sidewardsVelocity = -sidewardsVelocity;

                playerRigibody.velocity = new Vector3(sidewardsVelocity, 0f, 0f);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Stones")
        {
            this.GetComponent<AudioSource>().clip = strikeClip;
            this.GetComponent<AudioSource>().Play();
            if (!stones)
            {
                FindObjectOfType<GameManager>().score += 4;
                stones = true;
                Debug.Log("asdf");
            }
        }

        if (strike)
        {
            DestroyGameObjects();

            FindObjectOfType<GameManager>().StartCoroutine("InstantiateGameObject", 2.5f);
            strike = false;
            GameManager.gameRunning = false;
        }
    }

    private void DestroyGameObjects()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Stones"))
        {
            Destroy(g, 2f);
        }
        Destroy(this.gameObject, 2f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.name == "sideR" || other.gameObject.name == "sideL") && collide == true)
            FindObjectOfType<GameManager>().score += 2;
       
        if (other.gameObject.name == "centre" && collide == true)
            FindObjectOfType<GameManager>().score += 6;
        collide = false;

        Debug.Log("1234");
    }
}
