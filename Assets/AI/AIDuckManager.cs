using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDuckManager : MonoBehaviourSingleton<AIDuckManager>
{
    public GameObject player;
    public List<AIDuckController> allDucks = new();
    public List<AIDuckController> busyDucks = new();
    public List<AIDuckController> avaiableDucks = new();

    public int DuckCount => allDucks.Count;
    public int AvailableDuckCount => avaiableDucks.Count;

    void Start()
    {
        
    }

    public void SendDuckToAttack(GameObject attackObj)
    {
        if( AvailableDuckCount > 0 )
        {
            avaiableDucks[0].AttackObject( attackObj );
        }
    }


}
