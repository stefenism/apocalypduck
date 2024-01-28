using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowDuck : MonoBehaviour
{
    public AudioClip hitSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += transform.localScale * Time.deltaTime * 0.2f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb;
        if( collision.gameObject.TryGetComponent<Rigidbody>( out rb))
        {
            
            rb.isKinematic = false;
            SoundManager.PlaySound(hitSound, rb.gameObject.transform, new Vector2(-0.4f, 0.4f), 1f);
        }
    }
}
