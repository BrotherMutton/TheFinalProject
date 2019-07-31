using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    private PlayerController playerWeapons;
    private GameController gameController;
    public int scoreValue;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent <GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        GameObject playerWeaponsObject = GameObject.FindWithTag ("Player");
        if (playerWeaponsObject != null)
        {
            playerWeapons = playerWeaponsObject.GetComponent <PlayerController>();
        }
        if (playerWeaponsObject == null)
        {
            Debug.Log("Cannot find 'PlayerController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("PowerUp") )  
        {
            return;
        }
        if (explosion != null)
        {
        Instantiate(explosion, transform.position, transform.rotation);
        }
        if(other.CompareTag("Player") && playerExplosion != null)
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        gameController.AddScore (scoreValue);
        if(gameObject.tag != "PowerUp") // Only powerups will not destroy the collider and itself
        {
        Destroy(other.gameObject);
        Destroy(gameObject);
        }
        if (gameObject.tag == "PowerUp" && playerWeapons.isPoweredUp == false && other.CompareTag("Player"))
        {
        playerWeapons.isPoweredUp = true;   
        Destroy(gameObject);
        }
    }
}
