using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA;

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
    public List<string> classeHeros;
    public Dictionary<string,string> AllActions;
    public PhotonManager photonManager;
    public string msg = "";

    private float timer = 0;
    public Phase currentPhase;


    public enum Phase{
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

    private void Start()
    {
        //TODO : faire un random des classes
        photonManager.associateClasse(classeHeros.ToArray());
        AttributeColors();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        //print(msg);
        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case Phase.Positioning:
                RandomizePositions();
                

                //Switch to next Phase               
                timer = 0;
                BeginChoice();
                break;

            case Phase.ActionsSelection:
                  
                //TODO : Selection of actions by players

                //End of selection time
                if (timer >= selectionTime)
                {
                    currentPhase = Phase.ActionsResolution;
                    EndChoice();
                }
                                    
                break;

            case Phase.ActionsResolution:
                foreach (var heroe in CharacterManager.sharedInstance.characters)
                {
                    print("PLAY ACTRION " + heroe.actionToPlay);
                }

                if (orderedPlayers.Count == 0)
                {
                    foreach (var heroe in CharacterManager.sharedInstance.characters)
                    {
                        heroe.Reset();
                        
                    }
                    BeginChoice();
                }
                else
                {
                    if (!orderedPlayers[0].Busy && orderedPlayers[0].actionToPlay !=-1)
                    {
                        Character heroePlaying = orderedPlayers[0];
                        print("PLAY ACTRION " + heroePlaying.actionToPlay);
                        heroePlaying.PlayAction(heroePlaying.actionToPlay);                        
                    }
                    else if(orderedPlayers[0].Busy && orderedPlayers[0].actionToPlay != -1)
                    {
                        orderedPlayers[0].actionToPlay = -1;
                        orderedPlayers.RemoveAt(0);
                    }
                }
                                         
                break;

            default:
                break;
        }

        //End of the round
#endif
    }
    #endregion

    #region "Private methods"
    private void RandomizePositions()
    {
        orderedPlayers.Clear();
        AllActions.Clear();

        foreach (var heroe in CharacterManager.sharedInstance.characters)
        {
            for (int i = 0; i < heroe.Actions.Count; i++)
            {
                AllActions.Add(heroe.Actions[i], heroe.descActions[i]);
            }
            
        }

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

    private void BeginChoice()
    {
        currentPhase = Phase.ActionsSelection;
        int i = 0;
        foreach (var heroes in CharacterManager.sharedInstance.characters)
        {
            heroes.PickRandomActions();
            List<string> temp = new List<string>();
            temp.Add(AllActions[heroes.ProposalActions.Keys.ToArray()[0]]);

            if (heroes.RemainingActions.Count == 3)
            {
                
                photonManager.updateActions(Character.teams[i], heroes.ProposalActions.Keys.ToArray(), temp.ToArray(), true);
            }
            else
            {
                temp.Add(AllActions[heroes.ProposalActions.Keys.ToArray()[1]]);
                photonManager.updateActions(Character.teams[i], heroes.ProposalActions.Keys.ToArray(),temp.ToArray(), false);
                print("ACTION " + AllActions[heroes.ProposalActions.Keys.ToArray()[0]]);
                //print("OZOKOEZKOEZO " + heroes.ProposalActions.Keys.ToArray()[0]);
                //foreach (KeyValuePair<string, string> kvp in GameManager.sharedInstance.AllActions)
                //{
                //    //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                //    print("ACTION "+ kvp.Key);
                //}

            }
            
            i++;
        }
        

        photonManager.ChangeMode(true);
    }
    private void AttributeColors()
    {
        for (int i = 0; i < CharacterManager.sharedInstance.characters.Count; i++)
        {
            CharacterManager.sharedInstance.characters[i].Team = Character.teams[i];
        }
    }
    private void EndChoice()
    {
        currentPhase = Phase.ActionsResolution;
        photonManager.ChangeMode(false);
    }

    public void DetermineAction(string color, int action)
    {
        foreach (var heroe in CharacterManager.sharedInstance.characters)
        {
            if (heroe.Team == color)
            {
                for (int i = 0; i < heroe.Actions.Count; i++)
                {
                    if(heroe.Actions[i] == heroe.ProposalActions.Keys.ToArray()[action])
                    {
                        heroe.actionToPlay = i;
                    }
                }              
            }      
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
