using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    // player stats
    public float playerDps = 1;
    public float playerSize = 1;

    public PlayerController player;
    public CinemachineVirtualCamera cam;
    public GameObject playerDuckMesh;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelUpPLayer() {
        Vector3 newPlayerScale = new Vector3(player.transform.localScale.x + 1f, player.transform.localScale.y + 1f, player.transform.localScale.z + 1f);
        player.transform.localScale = newPlayerScale;
        playerDuckMesh.transform.localScale = newPlayerScale;
        playerDps += 1;
        playerSize += 1;

        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += 4;
    }

}
