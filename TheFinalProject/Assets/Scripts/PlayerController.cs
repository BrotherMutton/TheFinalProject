using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
     public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
     public float speed;
     public float tilt;
     public Boundary boundary;

     public GameObject shot;
     public Transform shotSpawn;
     public float fireRate;

     public bool isPoweredUp;
     public bool isActive;

     private float nextFire;
     private AudioSource audioSource;

     private Rigidbody rb;

     public IEnumerator WeaponsPowerUp()
    {
        fireRate -= 0.10f;
        yield return new WaitForSeconds(5.0f);
        fireRate += 0.10f; 
        isPoweredUp = false;     
        isActive = false;
    }


     private void Start()
     {
          rb = GetComponent<Rigidbody>();
          audioSource = GetComponent<AudioSource>();
          isActive = false;
          isPoweredUp = false;
     }

     void Update ()
     {
          if (Input.GetButton("Fire1") && Time.time > nextFire)
          {
               nextFire = Time.time + fireRate;
               GameObject clone = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
               audioSource.Play();
          }
          if (isPoweredUp == true && isActive == false)
          {
          StartCoroutine(WeaponsPowerUp());
          isActive = true;
          }

     }

     void FixedUpdate()
     {
          float moveHorizontal = Input.GetAxis("Horizontal");
          float moveVertical = Input.GetAxis("Vertical");

          Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
          rb.velocity = movement * speed;

          rb.position = new Vector3
          (
               Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
               0.0f,
               Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
          );

          rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
     }
}