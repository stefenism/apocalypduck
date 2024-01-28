using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class spawner : MonoBehaviour {

    public GameObject spawnPrefab;
    public int spawCount = 1;
    public float spawnForce = 10f;

    void Start() {

    }

    void Update() {
    }

    public void spawn(float destroyInSeconds = 0) {
        for (int i = 0; i < spawCount; i++) {
            GameObject clone = Instantiate(spawnPrefab, transform.position, transform.rotation);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.velocity = (clone.transform.up * spawnForce);

            // // choose launch forward angle: directly forward on Z, and up on Y (45 degree up angle):
            // Vector3 velocity = clone.transform.forward * 1.0f + clone.transform.up * 1.0f;
            
            // // choose an angle left /right
            // float angleLeftRight = Random.Range( -22.5f, 22.5f);
            
            // // create a rotation around the Y axis based on the angle we chose
            // Quaternion rotation = Quaternion.Euler( 0, angleLeftRight, 0);
            
            // // rotate the proposed straight-ahead velocity around the Y axis:
            // Vector3 actualVelocity = rotation * velocity;
            
            // // scale the toss velocity up and down randomly (80% to 120% of base)
            // actualVelocity *= Random.Range( 0.8f, 1.2f);
            
            // // TODO: perhaps scale the velocity up even more?
            // actualVelocity *= spawnForce;  // depending on gravity
            
            // // you don't really need forces: just set the velocity!
            // rb.velocity = actualVelocity;

        }
        Invoke("destroyMe", destroyInSeconds);
    }

    void destroyMe() {
        Destroy(this.gameObject);
    }
}