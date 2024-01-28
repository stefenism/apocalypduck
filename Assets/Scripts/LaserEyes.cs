using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEyes : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;

    private Transform playerCamera;
    [SerializeField]
    private ObstacleStats obstacleStats = null;
    private GameObject targetObject;
    private ObstacleStats lastSightedEnemy;
    public Material laserMat;
    LineRenderer laserLine;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        ChangeRayColor(Color.yellow);
    }

    // Update is called once per frame
    void Update()
    {
        GetAndDrawRay();
        OnRightMouseClick();
    }

    private void ChangeRayColor(Color color)
    {
        Gradient yellowColor = new Gradient();

        GradientColorKey[] yellowColorKeys = new GradientColorKey[2];
        yellowColorKeys[0] = new GradientColorKey(color,1.0f);
        yellowColorKeys[1] = new GradientColorKey(color,1.0f);
        
        GradientAlphaKey[] yellowAlphaKeys = new GradientAlphaKey[2];
        yellowAlphaKeys[0] = new GradientAlphaKey(1.0f, 1.0f);
        yellowAlphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);
        
        yellowColor.SetKeys(yellowColorKeys, yellowAlphaKeys);

        laserLine.colorGradient = yellowColor;
    }

    private void GetAndDrawRay()
    {
        if (obstacleStats == null)
        {
            ChangeRayColor(Color.yellow);
        }
        int x = (Screen.width / 2);
        int y = (Screen.height / 2);
        laserLine.SetPosition(0, transform.position);
        Ray ray = Camera.main.ScreenPointToRay( new Vector2( x,y ) );
        if (Physics.Raycast(ray, out RaycastHit hit, playerStats.laserRange))
        {
            targetObject = hit.collider.gameObject;

            if (targetObject.layer == 6 && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
            {

                if(lastSightedEnemy != null) {
                    lastSightedEnemy.isInSights = false;
                }
                obstacleStats = targetObject.GetComponent<ObstacleStats>();
            }
            else if (targetObject.layer == 6 && !(Input.GetMouseButton(0) || Input.GetMouseButton(1))) {
                Debug.Log("no mouse button but object is on right layer");
                if(lastSightedEnemy != null) {
                    lastSightedEnemy.isInSights = false;
                }

                obstacleStats = targetObject.GetComponent<ObstacleStats>();
                lastSightedEnemy = obstacleStats;
                lastSightedEnemy.isInSights = true;
            }
            else if (targetObject.layer != 6)
            {
                if (obstacleStats != null)
                {
                    obstacleStats.isLasered = false;
                    obstacleStats.isTargeted = false;
                    obstacleStats = null;
                }

                if(lastSightedEnemy != null) {
                    lastSightedEnemy.isInSights = false;
                    lastSightedEnemy = null;
                }

                ChangeRayColor(Color.yellow);
            }

            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, transform.position);
            lastSightedEnemy = null;
        }
    }

    private void OnRightMouseClick()
    {
        if (obstacleStats == null)
        {
            return;
        }
        if (Input.GetMouseButton(1))
        {
            ChangeRayColor(Color.red);
            obstacleStats.isLasered = true;
            //TODO: Calculate damage based on parameters
            obstacleStats.damageTaken = 1;
            laserLine.enabled = true;
        }
        else
        {
            ChangeRayColor(Color.yellow);
            obstacleStats.isLasered = false;
            laserLine.enabled = false;
        }
    }
}