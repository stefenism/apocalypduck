using System;
using System.Collections;
using UnityEngine;

public class growthTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        speed = speed * -1;
    }
}