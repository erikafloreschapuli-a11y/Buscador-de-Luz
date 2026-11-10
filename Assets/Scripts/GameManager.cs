using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 checkpointPos;
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        checkpointPos = GameObject.FindWithTag("Player").transform.position;
    }

    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
    }

    public void RespawnPlayer()
    {
        player.transform.position = checkpointPos;
        player.SetActive(true);
    }
}