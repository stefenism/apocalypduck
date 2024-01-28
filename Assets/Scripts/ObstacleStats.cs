using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStats : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private float o_health;

    [SerializeField]
    private int o_convertToDucks;

    [SerializeField]
    private bool o_isDuckable = false;

    [SerializeField]
    private bool o_overrideIsDuckable = false;

    [SerializeField]
    private bool o_isLasered = false;

    [SerializeField]
    private bool o_isTargeted = false;

    [SerializeField]
    private float o_damageTaken;

    AIDuckManager manager => AIDuckManager.Instance;

    public float health
    {
        get { return o_health; }
        set { o_health = value; }
    }

    public int convertToDucks
    {
        get { return o_convertToDucks; }
        set { o_convertToDucks = value; }
    }

    public bool isDuckable
    {
        get { return o_isDuckable; }
        set { o_isDuckable = value; }
    }

    public bool isLasered
    {
        get { return o_isLasered; }
        set { o_isLasered = value; }
    }

    public bool isTargeted
    {
        get { return o_isTargeted; }
        set { o_isTargeted = value; }
    }

    public float damageTaken
    {
        get { return o_damageTaken; }
        set { o_damageTaken = value; }
    }

    void Start()
    {
        StartCoroutine(ReduceHealth());
        StartCoroutine(AssignDucks());
    }

    void Update()
    {
        CheckIfDuckable();
    }

    void CheckIfDuckable()
    {
        if (o_health / 3 <= playerStats.laserPower)
        {
            o_isDuckable = true;
        }
    }

    private IEnumerator ReduceHealth()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if ((o_isDuckable || o_overrideIsDuckable) && o_health > 0 && o_isLasered)
            {
                o_health -= o_damageTaken;
                if (o_health < 0)
                {
                    o_health = 0;
                }
            }
        }
    }

    private IEnumerator AssignDucks()
    {
        float timeElapsed = 0;
        while(true)
        {
            timeElapsed += Time.deltaTime;
            //TODO: increase ducks assigned rate based on time passed and max ducks allowed
            if ((o_isDuckable || o_overrideIsDuckable) && o_health > 0 && o_isTargeted)
            {
                manager.SendDuckToAttack(this.gameObject.GetComponent<ObstacleStats>());
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void StopAllCoroutinesObstacle()
    {
        StopCoroutine(ReduceHealth());
    }
}
