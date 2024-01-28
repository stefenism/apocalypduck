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

    Transform target;
    ObstacleStats attackObstacle;
    public GameObject duckPos;
    public GameObject duckMesh;

    AiDuckState state;

    Rigidbody rb;
    NavMeshAgent agent;
    Vector3 lastJumpPos;
    float distToJump = 1;
    bool jumpCooldown = false;
    bool isLasering = false;

    public LineRenderer laserLineLeft;
    public LineRenderer laserLineRight;
    public Transform LeftEye;
    public Transform RightEye;

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
        isLasering = false;
        target = manager.player.transform;
        state = AiDuckState.followPlayer;
    }

    public void AttackObject(ObstacleStats attackObj)
    {
        if (state != AiDuckState.attackObject)
        {
            manager.busyDucks.Add(this);
            manager.avaiableDucks.Remove(this);
        }
        attackObstacle = attackObj;
        state = AiDuckState.attackObject;
        isLasering = true;
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
                agent.radius = 1.5f;
                agent.speed = 4.5f;


                if (Vector3.Distance(agent.transform.position, lastJumpPos) >= distToJump)
                {
                    if (!jumpCooldown)
                    {
                        JumpToPosition();
                    }
                }
                break;
            case AiDuckState.attackObject:
                if (!agent.isOnNavMesh)
                {
                    agent.transform.position = Vector3.zero;
                    agent.enabled = false;
                    agent.enabled = true;
                    return;
                }
                if ( !attackObstacle || attackObstacle.health <= 0)
                {
                    FollowPlayer();
                    return;
                }
                agent.destination = attackObstacle.transform.position;
                agent.radius = 1.3f;
                agent.speed = 8f;


                if (Vector3.Distance(agent.transform.position, lastJumpPos) >= distToJump)
                {
                    if (!jumpCooldown)
                    {
                        JumpToPosition();
                    }
                }

                break;
            default:
                break;
        }

        UpdateLaser();
    }

    void UpdateLaser()
    {
        if ( isLasering )
        {
            //add damage
            attackObstacle.health -= manager.AiDPS * Time.deltaTime;

            //Render Laser
            laserLineLeft.enabled = true;
            laserLineLeft.SetPosition(0, LeftEye.position);
            laserLineLeft.SetPosition(1, attackObstacle.transform.position);
            laserLineRight.enabled = true;
            laserLineRight.SetPosition(0, RightEye.position);
            laserLineRight.SetPosition(1, attackObstacle.transform.position);
        }
        else
        {
            laserLineLeft.enabled = false;
            laserLineRight.enabled = false;
        }
    }

    private void ChangeRayColor(Color color)
    {
        Gradient yellowColor = new Gradient();

        GradientColorKey[] yellowColorKeys = new GradientColorKey[2];
        yellowColorKeys[0] = new GradientColorKey(color, 1.0f);
        yellowColorKeys[1] = new GradientColorKey(color, 1.0f);

        GradientAlphaKey[] yellowAlphaKeys = new GradientAlphaKey[2];
        yellowAlphaKeys[0] = new GradientAlphaKey(1.0f, 1.0f);
        yellowAlphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

        yellowColor.SetKeys(yellowColorKeys, yellowAlphaKeys);

        laserLineLeft.colorGradient = yellowColor;
        laserLineRight.colorGradient = yellowColor;
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
