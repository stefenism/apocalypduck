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

    public float AiDPS = 0.5f;

    void Start()
    {
        
    }

    public void SendDuckToAttack(ObstacleStats attackObj)
    {
        if( AvailableDuckCount > 0 )
        {
            avaiableDucks[0].AttackObject( attackObj );
        }
    }


}
