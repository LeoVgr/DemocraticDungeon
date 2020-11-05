using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region "Singleton"
    private static GameManager instance = null;

    public static GameManager sharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    #endregion

    #region "Attributs"
    public float selectionTime;
    public List<Character> orderedPlayers;

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
        orderedPlayers = new List<Character>();
        currentPhase = Phase.Menu;

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
                foreach (var heroes in CharacterManager.sharedInstance.characters)
                {
                    heroes.PickRandomActions();
                }

                    
                //TODO : Selection of actions by players

                //End of selection time
                if (timer >= selectionTime)
                    currentPhase = Phase.ActionsResolution;

                break;

            case Phase.ActionsResolution:
                if(orderedPlayers.Count == 0)
                {
                    foreach (var heroe in CharacterManager.sharedInstance.characters)
                    {
                        heroe.Reset();
                    }
                }
                else
                {
                    if (!orderedPlayers[0].Busy)
                    {
                        Character heroePlaying = orderedPlayers[0];
                        heroePlaying.PlayAction(0);
                    }
                    else
                    {
                        orderedPlayers.RemoveAt(0);
                    }
                }
                                         
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

        while(orderedPlayers.Count < CharacterManager.sharedInstance.characters.Count)
        {
            int randomIndex = Random.Range(0, CharacterManager.sharedInstance.characters.Count);

            while (orderedPlayers.Contains(CharacterManager.sharedInstance.characters[randomIndex]))
            {
                randomIndex = Random.Range(0, CharacterManager.sharedInstance.characters.Count);
            }

            orderedPlayers.Add(CharacterManager.sharedInstance.characters[randomIndex]);
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
