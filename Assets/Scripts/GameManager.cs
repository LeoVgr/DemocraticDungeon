using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region "Attributs"
    public float selectionTime;
    public List<GameObject> players;
    public List<GameObject> orderedPlayers;

    private float timer = 0;
    private Phase currentPhase;

    private enum Phase{
        Menu,
        Positioning,
        ActionsSelection,
        ActionsResolution
    }

    #endregion

    #region "Events"
    private void Awake()
    {
        orderedPlayers = new List<GameObject>();
        currentPhase = Phase.Menu;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        print(currentPhase);
        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.Positioning:
                RandomizePositions();

                //Switch to next Phase
                currentPhase = Phase.ActionsSelection;
                timer = 0;
                break;

            case Phase.ActionsSelection:

                //TODO : Selection of actions by players

                //End of selection time
                if (timer >= selectionTime)
                    currentPhase = Phase.ActionsResolution;
                break;

            case Phase.ActionsResolution:
                break;

            default:
                break;
        }

        //End of the round
 
    }
    #endregion

    #region "Private methods"
    private void RandomizePositions()
    {
        orderedPlayers.Clear();

        while(orderedPlayers.Count < players.Count)
        {
            int randomIndex = Random.Range(0, players.Count);

            while (orderedPlayers.Contains(players[randomIndex]))
            {
                randomIndex = Random.Range(0, players.Count);
            }

            orderedPlayers.Add(players[randomIndex]);
        }
    }
    #endregion

    #region "GameOptions"

    public void GameOver()
    {

    }

    public void Victory()
    {

    }
    #endregion
}
