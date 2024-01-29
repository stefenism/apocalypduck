using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auraController : MonoBehaviour
{

    public GameObject auraClone;
    ObstacleStats thisObstacle;

    // Start is called before the first frame update
    void Start()
    {
        thisObstacle = GetComponent<ObstacleStats>();
    }

    // Update is called once per frame
    void Update() {
        if(thisObstacle.isInSights && auraClone.activeSelf == false) {
            setTargeted();
        } else if(!thisObstacle.isInSights && auraClone.activeSelf == true) {
            setUnTargeted();
        }
    }

    public void toggleAura() {
        if(auraClone.activeSelf) {
            auraClone.SetActive(false);
        } else {
            auraClone.SetActive(true);
        }
    }

    public void setTargeted() {
        auraClone.SetActive(true);
    }

    public void setUnTargeted() {
        auraClone.SetActive(false);
    }
}
