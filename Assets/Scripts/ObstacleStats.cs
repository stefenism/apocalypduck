using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStats : MonoBehaviour
{
    private PlayerStats playerStats => PlayerStats.Instance;

    [SerializeField]
    private float o_health = 100;

    [SerializeField]
    private float o_maxHealth = 100;

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
    private bool o_isInSights = false;

    [SerializeField]
    private float o_damageTaken;

    AIDuckManager manager => AIDuckManager.Instance;
    private Coroutine assignDucks;

    private float timeSinceLastReduceHealth = 0;

    public AudioClip explosionSound;

    public float health
    {
        get { return o_health; }
        set { o_health = value; }
    }

    public float maxHealth
    {
        get { return o_maxHealth; }
        set { o_maxHealth = value; }
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

    public bool isInSights
    {
        get { return o_isInSights; }
        set { o_isInSights = value; }
    }

    public float damageTaken
    {
        get { return o_damageTaken; }
        set { o_damageTaken = value; }
    }

    void Start()
    {
        StartCoroutine(ReduceHealth());
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
            yield return null;

            // if(timeSinceLastReduceHealth >= 1) {
            //     // if ((o_isDuckable || o_overrideIsDuckable) && o_health > 0 && o_isLasered)
            //     // {
            //     //     // o_health -= (o_damageTaken * Time.deltaTime);
            //     //     o_health -= GameManager.Instance.playerDps;

            //     //     duckConversionController dcc = this.gameObject.GetComponentInChildren<duckConversionController>();
            //     //     float healthRatio = o_health/o_maxHealth;
            //     //     dcc.SetPercentFilled(healthRatio);

            //     //     if (o_health <= 0)
            //     //     {
            //     //         o_health = 0;
            //     //         spawner s = this.gameObject.GetComponent<spawner>();
            //     //         s.spawn();
            //     //     }
            //     // }
            //     timeSinceLastReduceHealth = 0;
            // } else {
            //     Debug.Log("time delta time is: " + (GameManager.Instance.playerDps * Time.deltaTime));
            //     timeSinceLastReduceHealth += Time.deltaTime;
            // }
            if ((o_isDuckable || o_overrideIsDuckable) && o_health > 0 && o_isLasered)
                {
                    // o_health -= (o_damageTaken * Time.deltaTime);
                    o_health -= (GameManager.Instance.playerDps * Time.deltaTime);

                    duckConversionController dcc = this.gameObject.GetComponentInChildren<duckConversionController>();
                    float healthRatio = o_health/o_maxHealth;
                    dcc.SetPercentFilled(healthRatio);

                    if (o_health <= 0)
                    {
                        o_health = 0;
                        spawner s = this.gameObject.GetComponent<spawner>();
                        s.spawn();
                        SoundManager.PlaySound(explosionSound, transform, new Vector2(-0.4f,0.4f), 0.6f);
                    }
                }
        }
    }

    private IEnumerator AssignDucks()
    {
        float timer = 0f;
        bool duckApplied = false;
        while(true)
        {
            if ((o_isDuckable || o_overrideIsDuckable) && o_health > 0)
            {
                timer -= Time.deltaTime;
                if(timer <= 0f){
                    manager.SendDuckToAttack(this.gameObject.GetComponent<ObstacleStats>());
                    timer = duckApplied ? .1f : 1f;
                    duckApplied = true;
                }
            }
            yield return null;
        }
    }

    private void OnMouseDown() {
        if (!o_isTargeted)
        {
            assignDucks = StartCoroutine(AssignDucks());
        }

        o_isTargeted = true;
    }

    private void OnMouseUp() {
        o_isTargeted = false;
        if (assignDucks != null)
        {
            StopCoroutine(assignDucks);
        }
    }
}
