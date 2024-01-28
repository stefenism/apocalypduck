using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auraController : MonoBehaviour
{

    public GameObject auraClone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown("n")) {
            toggleAura();
        }
    }

    public void toggleAura() {
        if(auraClone.activeSelf) {
            auraClone.SetActive(false);
        } else {
            auraClone.SetActive(true);
        }
    }
}
