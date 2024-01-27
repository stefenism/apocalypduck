using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

enum AiDuckState
{
    none,
    launch,
    followPlayer,
    attackObject
}

public class AIDuckController : MonoBehaviour
{

    public Transform target;
    public GameObject duckPos;
    public GameObject duckMesh;

    AiDuckState state;

    Rigidbody rb;
    NavMeshAgent agent;
    Vector3 lastJumpPos;
    float distToJump = 1;
    bool jumpCooldown = false;

    AIDuckManager manager => AIDuckManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        manager.allDucks.Add(this);
        LaunchDuck();
    }

    private void OnDestroy()
    {
        manager.allDucks.Remove(this);
        manager.busyDucks.Remove(this);
        manager.avaiableDucks.Remove(this);
    }

    async void LaunchDuck()
    {
        state = AiDuckState.launch;

        await Task.Delay(3000);

        if (state == AiDuckState.launch)
            StartPathing();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AIDuckController ai;
        if (!collision.gameObject.TryGetComponent<AIDuckController>( out ai ))
        {
            if (state == AiDuckState.launch)
                StartPathing();
        }
    }


    void StartPathing()
    {   
        duckPos.transform.parent = null;
        lastJumpPos = agent.transform.position;
        agent.enabled = true;
        Destroy(rb);
        FollowPlayer();
    }

    public void FollowPlayer() 
    {
        if (state == AiDuckState.followPlayer) return;
        manager.busyDucks.Remove(this);
        manager.avaiableDucks.Add(this);

        state = AiDuckState.followPlayer;
    }

    public void AttackObject(GameObject attackObj)
    {
        if (state != AiDuckState.attackObject)
        {
            manager.busyDucks.Add(this);
            manager.avaiableDucks.Remove(this);
        }

        state = AiDuckState.attackObject;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AiDuckState.none:
                break;
            case AiDuckState.launch:
                break;
            case AiDuckState.followPlayer:
                if (!agent.isOnNavMesh) 
                { 
                    agent.transform.position = Vector3.zero;
                    agent.enabled = false;
                    agent.enabled = true;
                    return;
                }
                agent.destination = target.position;
                

                if (Vector3.Distance(agent.transform.position, lastJumpPos) >= distToJump)
                {
                    if (!jumpCooldown)
                    {
                        JumpToPosition();
                    }
                }
                break;
            case AiDuckState.attackObject:
                break;
            default:
                break;
        }

    }


    async void JumpToPosition()
    {
        lastJumpPos = agent.transform.position;
        jumpCooldown = true;
        float airTime = 0.5f;
        float groundTime = 0.25f;
        float upTime = airTime * 0.7f;
        float dropTime = airTime - upTime;
        LeanTween.move(duckPos, agent.transform.position, airTime);
        LeanTween.rotate(duckPos, agent.transform.rotation.eulerAngles, airTime);
        
        LeanTween.moveLocalY( duckMesh, 0.6f , upTime).setEaseOutCirc();
        await Task.Delay( Mathf.RoundToInt(upTime * 1000));
        LeanTween.moveLocalY(duckMesh, 0.0f, dropTime).setEaseInCirc();
        await Task.Delay(Mathf.RoundToInt((dropTime + groundTime) * 1000));
        jumpCooldown = false;
    }
}
