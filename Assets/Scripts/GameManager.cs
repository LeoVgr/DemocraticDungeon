﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    #region "Attributs"
    public float selectionTime;
    public List<Character> players;
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
                
                //TODO : Selection of actions by players

                //End of selection time
                if (timer >= selectionTime)
                    currentPhase = Phase.ActionsResolution;

                break;

            case Phase.ActionsResolution:
                if(orderedPlayers.Count == 0)
                {
                    foreach (var heroe in players)
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
