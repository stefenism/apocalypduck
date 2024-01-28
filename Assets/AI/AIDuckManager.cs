using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class AIDuckManager : MonoBehaviourSingleton<AIDuckManager>
{
    public PlayerController player;
    public List<AIDuckController> allDucks = new();
    public List<AIDuckController> busyDucks = new();
    public List<AIDuckController> avaiableDucks = new();

    public int DuckCount => allDucks.Count;
    public int AvailableDuckCount => avaiableDucks.Count;
    public int BusyDuckCount => busyDucks.Count;

    public float AiDPS = 0.5f;

    public TMP_Text duckCountText;
    public TMP_Text assignDuckText;
    public TMP_Text recallDuckText;

    string activeTextColor = "fff100";
    string inactiveTextColor = "9b9b9b";

    void Start()
    {
        duckCountText.SetText($"{AvailableDuckCount}/{DuckCount}");
        assignDuckText.gameObject.SetActive(false);
        recallDuckText.gameObject.SetActive(false);
    }

    public void SendDuckToAttack(ObstacleStats attackObj)
    {
        if( AvailableDuckCount > 0 )
        {
            avaiableDucks[0].AttackObject( attackObj );
        }
    }

    public void RecallAllDucks()
    {
        while( BusyDuckCount > 0 )
        {
            busyDucks[0].FollowPlayer();
        }
    }

    public void updateDuckCount() {
        duckCountText.SetText($"{AvailableDuckCount}/{DuckCount}");

        if(AvailableDuckCount > 0) {
            if(!assignDuckText.gameObject.activeSelf) {
                assignDuckText.gameObject.SetActive(true);
            }
            assignDuckText.color = getColor("active");
        } else if(AvailableDuckCount == 0) {
            assignDuckText.color = getColor("inactive");
        }

        if(AvailableDuckCount == DuckCount) {
            if(!recallDuckText.gameObject.activeSelf) {
                recallDuckText.gameObject.SetActive(true);
            }
            recallDuckText.color = getColor("inactive");
        } else if(AvailableDuckCount < DuckCount) {
            recallDuckText.color = getColor("active");
        }
    }

    public Color getColor(string activeState) {
        switch (activeState)
        {
            case "active":
                string hex = activeTextColor;
                var r = hex.Substring(0, 2);
                var g = hex.Substring(2, 2);
                var b = hex.Substring(4, 2);
                string alpha;
                if (hex.Length >= 8)
                    alpha = hex.Substring(6, 2);
                else
                    alpha = "FF";

                return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                                (int.Parse(g, NumberStyles.HexNumber) / 255f),
                                (int.Parse(b, NumberStyles.HexNumber) / 255f),
                                (int.Parse(alpha, NumberStyles.HexNumber) / 255f));

            case "inactive":
                string hex2 = inactiveTextColor;
                var r2 = hex2.Substring(0, 2);
                var g2 = hex2.Substring(2, 2);
                var b2 = hex2.Substring(4, 2);
                string alpha2;
                if (hex2.Length >= 8)
                    alpha2 = hex2.Substring(6, 2);
                else
                    alpha2 = "FF";

                return new Color((int.Parse(r2, NumberStyles.HexNumber) / 255f),
                                (int.Parse(g2, NumberStyles.HexNumber) / 255f),
                                (int.Parse(b2, NumberStyles.HexNumber) / 255f),
                                (int.Parse(alpha2, NumberStyles.HexNumber) / 255f));
            default:
                return new Color();
        }
    }
}
