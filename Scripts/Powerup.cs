using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        if (playerController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Boundary") || other.tag == ("Enemy"))
        {
            return;
        }

        if (other.tag == "Player")
        {
            StartCoroutine(Countdown());
        }
    }

    IEnumerator Countdown()
    {
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        playerController.fireRate = .12f;
        yield return new WaitForSeconds(5);
        playerController.fireRate = .25f;
        Destroy(gameObject);
    }
}
