using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private int m_laserPower;

    [SerializeField]
    private int m_laserRange;

    [SerializeField]
    private int m_ducksTotal;

    [SerializeField]
    private int m_ducksAssigned;

    public int laserPower
    {
        get
        {
            return m_laserPower;
        }
        set
        {
            m_laserPower = value;
        }
    }

    public int laserRange
    {
        get { return m_laserRange; }
        set { m_laserRange = value; }
    }
}
