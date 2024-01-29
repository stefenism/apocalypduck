using System;
using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class growthTrigger : MonoBehaviour {

    bool doingStuff = false;

    private async Task OnTriggerEnter(Collider other)
    {
        Debug.Log("other.gameobject.tag: " + other.gameObject.tag);
        if(other.gameObject.tag == "Player" && !doingStuff) {
            doingStuff = true;
            Debug.Log("on trigger enter");
            await AIDuckManager.Instance.ConsumeAllDucks();
            //scale upu

            //increase damage

            GameManager.Instance.LevelUpPLayer();
            //increase camera distance

            Destroy(this.gameObject);
        }
    }
}