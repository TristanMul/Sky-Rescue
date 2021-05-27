using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInvisible : MonoBehaviour
{
    public static MakeInvisible instance;
    [SerializeField] private List<GameObject> EnemyPlayers;
    [SerializeField] private List<GameObject> AlliedPlayers;
    [HideInInspector] public int PlayersActivated = 0;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        foreach (GameObject Player in EnemyPlayers)
        {
            Player.SetActive(false);
        }
        foreach (GameObject Player in AlliedPlayers)
        {
            Player.SetActive(false);
        }
        StartCoroutine(activatePlayers(PlayersActivated));
        PlayersActivated += 1;
        StartCoroutine(activatePlayers(PlayersActivated));
    }
    IEnumerator activatePlayers(int playerToActivate)
    {
        EnemyPlayers[playerToActivate].SetActive(true);
        AlliedPlayers[playerToActivate].SetActive(true);
        yield return null;
    }
    public IEnumerator deactivatePlayer()
    {
        if (PlayersActivated + 1 < EnemyPlayers.Count || PlayersActivated + 1 < AlliedPlayers.Count)
        {
            StartCoroutine(activatePlayers(PlayersActivated + 1));
        }
            if (EnemyPlayers[PlayersActivated].GetComponent<BallHandling>().isCaught == true && AlliedPlayers[PlayersActivated].GetComponent<BallHandling>().isCaught == true)
            {

                yield return new WaitForSeconds(1f);
                EnemyPlayers[PlayersActivated - 1].SetActive(false);
                AlliedPlayers[PlayersActivated - 1].SetActive(false);
                PlayersActivated += 1;
            }
    }
}

