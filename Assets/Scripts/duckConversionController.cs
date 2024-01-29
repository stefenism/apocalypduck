using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class duckConversionController : MonoBehaviour {

    Renderer rend;
    ObstacleStats thisObstacle;

    void Update() {
    }

    void Start() {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Shader Graphs/gradient_shader");

        thisObstacle = transform.parent.GetComponent<ObstacleStats>();
        float healthRatio = thisObstacle.health/thisObstacle.maxHealth;
        // Debug.Log("starting health ratio is: " + (transform.position.y + healthRatio));
        rend.material.SetFloat("_origin", (transform.position.y + transform.parent.localScale.y) - (2 * transform.parent.localScale.y * healthRatio));

    }

    public void SetPercentFilled(float healthRatio) {
        float evaluatedOrigin = (transform.position.y + transform.parent.localScale.y) - (2 * transform.parent.localScale.y * healthRatio);
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