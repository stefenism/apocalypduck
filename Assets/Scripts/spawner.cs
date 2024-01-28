using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {

    public GameObject spawnPrefab;
    public int spawCount = 1;

    void Start() {

    }

    void Update() {
        if(Input.GetKeyDown("i")) {
            spawn(3);
        }
    }

    void spawn(float destroyInSeconds = 0) {
        for (int i = 0; i < spawCount; i++) {
            GameObject clone = Instantiate(spawnPrefab, transform.position, transform.rotation);
        }
        Invoke("destroyMe", destroyInSeconds);
    }

    void destroyMe() {
        Destroy(this.gameObject);
    }
}