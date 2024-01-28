using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class duckConversionController : MonoBehaviour {

    Renderer rend;
    ObstacleStats thisObstacle;

    void Update() {
        if(Input.GetKeyDown("f")) {
            Debug.Log("setting new percent filled");
            Debug.Log("rend.material is " + rend.material);
            float newRandom = Random.value;
            Debug.Log("new random is: " + newRandom);
            SetPercentFilled(newRandom);
        }
    }

    void Start() {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Shader Graphs/gradient_shader");

        thisObstacle = GetComponent<ObstacleStats>();
        float healthRatio = thisObstacle.health/thisObstacle.maxHealth;
        // Debug.Log("starting health ratio is: " + (transform.position.y + healthRatio));
        rend.material.SetFloat("_origin", (transform.position.y + 1f) - (2 * healthRatio));

    }

    public void SetPercentFilled(float healthRatio) {
        Debug.Log("mat origin bottom: " + (transform.position.y - .5f));
        float evaluatedOrigin = (transform.position.y + 1f) - (2 * healthRatio);
        // Debug.Log("evaluated origin is: " + transform.position.y - .5f + evaluatedOrigin);
        rend.material.SetFloat("_origin", evaluatedOrigin);
    }

}

// public class Example : MonoBehaviour
// {
//     Renderer rend;

//     void Start()
//     {
//         rend = GetComponent<Renderer> ();

//         // Use the Specular shader on the material
//         rend.material.shader = Shader.Find("Specular");
//     }

//     void Update()
//     {
//         // Animate the Shininess value
//         float shininess = Mathf.PingPong(Time.time, 1.0f);
//         rend.material.SetFloat("_Shininess", shininess);
//     }
// }