using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private UIManager ui_manager;

    public bool DEBUG_MODE = false;

    public int NB_PLAYERS_MAX = 2;
    public float TURN_TIME = 20.0f;

    private string gameVersion = "0.2";
    private string roomName = "Arena";

    public bool isConnecting = false;
    private bool gameLaunch = false;

    public bool mode_choice = false;
    private float timer_turn = 0.0f;
    private bool refresh = false;

    public Dictionary<string, int> players;

    int id_color;
    float life = 100.0f;

    //debug
    float[] lifes_array = {10.0f,80.0f, 30.0f , 20.0f , 60.0f , 70.0f };
    string[] classes_array = { "Assassin","Archer", "Mage", "Paladin","Barbare","Boss" };
    //List<float> lifes = new List<float>();
    //List<string> classes = new List<string>();

    //public int nb_member = 1;
    //public int nb_votes;
    //public int nb_votes_action_1;
    //public int nb_votes_action_2;
    bool action1;
    bool action2;

    PhotonView pv;

    //PENSEZ A ACTUALISER SUR CLIENT LEUR CHARACTER ET ETAT DU JEU

    void Awake()
    {
        //a verifier
        PhotonNetwork.AutomaticallySyncScene = true;
        pv = GetComponent<PhotonView>();
        players = new Dictionary<string, int>();

        //debug
        //lifes.AddRange(lifes_array);
        //classes.AddRange(classes_array);
    }
    // Start is called before the first frame update
    void Start()
    {
        ui_manager.activateMenu("start");
        PlayerPrefs.DeleteAll();
        Debug.Log("Connecting to Photon Network");

        ConnectToPhoton();
    }

    public void setVotes(int type)
    {
        //int res = -1;
        if (mode_choice)
        {
            action1 = (type == 0);
            action2 = !action1;
            //res = (action1 ? 0 : 1);
        }
        //return res;
    }

    /*public bool setVotes(int type, int nb)
    {
        bool res = false;
        if (mode_choice)
        {
            if ((type == 0 ? (nb_votes_action_1 + nb >= 0) : (nb_votes_action_2 + nb >= 0)))
            {
                nb_votes -= nb;
                if (nb_votes >= 0 && nb_votes <= nb_member)
                {
                    if (type == 0)
                        nb_votes_action_1 += nb;
                    else
                        nb_votes_action_2 += nb;

                    res = true;
                }
                else
                {
                    nb_votes = (nb_votes < 0 ? 0 : nb_member);
                }
            }
        }
        return res;
    }*/


    public void updateActions(string name,string [] actions,bool [] alerte)
    {
        pv.RPC("sendActions", RpcTarget.Others, name, actions, alerte);
    }

    [PunRPC]
    public void sendActions(string name, string [] actions,bool [] alerte)
    {
        if (PhotonNetwork.NickName.Equals(name))
        {
            ui_manager.updateActionChoice(actions,alerte);
        }
    }

    [PunRPC]
    public void retrieveVote()
    {
        Sending();
    }


    [PunRPC]
    public void afficherMsg(string msg)
    {
        Debug.Log("Message : " + msg);
    }

    //public void Sending

    /*public void Sending()
    {
        while(nb_votes > 0)
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
            {
                nb_votes_action_1++;
            }
            else
            {
                nb_votes_action_2++;
            }
            nb_votes--;
        }
        pv.RPC("sendingMaster", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName, nb_votes_action_1, nb_votes_action_2);
        hasSend = true;
    }*/

    public void Sending()
    {
        if(!(action1 || action2))
        {
            action1 = (Random.Range(0, 1) == 0);
            action2 = !action1;
        }
        pv.RPC("sendingMaster", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName, (action1 ? 0 : 1));
    }

    /*[PunRPC]
    public void sendingMaster(string name,int action1,int action2)
    {
        //on peut assurer double cheque avec #if WINDOWS mais bon normalement pas besoin avec systeme MasterClient
        //ici transmission vote equipe to gameManager
        Debug.Log("Reception vote equipe " + name + " Action 1: " + action1 + " Action 2:" + action2);
    }*/

    public void associateClasse(string[] classes)
    {
        pv.RPC("receiveClasse", RpcTarget.All, classes);
    }

    [PunRPC]
    public void receiveClasse(string [] classes)
    {
        ui_manager.classe_txt.text = classes[id_color];
    }

    public void updateHp(float [] lifes)
    {
        pv.RPC("sendHp", RpcTarget.All, lifes);
    }

    [PunRPC]
    public void sendHp(float[] lifes)
    {
        life = lifes[id_color];
    }


    [PunRPC]
    public void sendingMaster(string name, int action)
    {
        //on peut assurer double cheque avec #if WINDOWS mais bon normalement pas besoin avec systeme MasterClient
        //ici transmission vote equipe to gameManager
        Debug.Log("Reception vote equipe " + name + " Action : " + action);
        //action == 0 => Action1 sinon Action2
    }

    /*public int getMembers()
    {
        return nb_member;
    }

    public bool setMembers(int n)
    {
        nb_member += n;
        if (nb_member < 1)
        {
            nb_member = 1;
            return false;
        }
        return true;
    }*/

    public bool verifySelectColor(string color)
    {
        bool res = JoinRoom(color);
        switch (color)
        {
            case "Green":
                id_color = 0;
                break;
            case "Orange":
                id_color = 1;
                break;
            case "Blue":
                id_color = 2;
                break;
            case "Violet":
                id_color = 3;
                break;
            case "Black":
                id_color = 4;
                break;
            case "Pink":
                id_color = 5;
                break;
            default:
                break;
        }
        //set du nom du joueur

        //verif dans jeu si déjà présente ?
        //pour le moment juste selectionne
        return res;
    }

    // Update is called once per frame
    void Update()
    {
        //wait players
        if (!gameLaunch)
        {
            LaunchTheGame();
        }
        //core game here
        else
        {
            //side core game
#if UNITY_ANDROID
            //setRefreshMode(true);
            ui_manager.updateHp(life/100.0f);
            life -= Time.deltaTime;
            associateClasse(classes_array);
            updateHp(lifes_array);
            /*if(refresh){
                UIManager.getInstance().activateMenu((mode_choice ? "choice" : "turn"));
                refresh = false;
                //pv.RPC("setRefresh",RpcTarget.All,false);
                
            }*/
#elif UNITY_STANDALONE_WIN
            Debug.Log("Tourne sur la machine centrale !");
                if ((Time.realtimeSinceStartup - timer_turn) > TURN_TIME)
                {
                    timer_turn = Time.realtimeSinceStartup;
                    mode_choice = !mode_choice;
                    //pv.RPC("setRefreshMode", RpcTarget.All, !mode_choice);
                    pv.RPC("retrieveVote",RpcTarget.Others);
                    pv.RPC("setRefreshMode", RpcTarget.Others, mode_choice);
                    
                    //RpcTarget.Others
                refresh = false;
                    Debug.Log("Changement de mode");
                }
#endif

        }
    }

    public void disconnected()
    {
        if (isConnecting)
        {
            if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        isConnecting = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.Log("Disconnected.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Success to join de room");
#if UNITY_STANDALONE_WIN
            bool connected = PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            Debug.Log("Connected master est de " + connected);
#endif
    }



    public bool JoinRoom(string name)
    {
        bool connected = PhotonNetwork.IsConnected;
        if (connected)
        {
            PhotonNetwork.LocalPlayer.NickName = name;
            Debug.Log("Photon.Network.connected() | Trying to Create/Join Room");
            RoomOptions roomOptions = new RoomOptions();
            TypedLobby typedLobby = new TypedLobby("", LobbyType.Default);

            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
        }
        return connected;
    }

    void ConnectToPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    [PunRPC]
    public void setRefreshMode(bool state)
    {
        mode_choice = state;
        ui_manager.activateMenu((mode_choice ? "choice" : "turn"));
        if (mode_choice)
        {
            action1 = action2 = false;
            ui_manager.addVotes(-1);
        }
    }

    [PunRPC]
    void setLaunch(bool state)
    {
        gameLaunch = state;
    }

    public void LaunchTheGame()
    {
        if (isConnecting)
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= NB_PLAYERS_MAX)
                {
                    #if UNITY_ANDROID
                    //debug
                    //gameLaunch = true;
                    //setRefreshMode(true);
                    //real script
                    ui_manager.activateMenu("turn");
#elif UNITY_STANDALONE_WIN
                    timer_turn = Time.realtimeSinceStartup;
                    pv.RPC("setLaunch", RpcTarget.All, true);
                    pv.RPC("setRefreshMode", RpcTarget.Others, mode_choice);
                    ui_manager.activateMenu("");
                    foreach(int k in PhotonNetwork.CurrentRoom.Players.Keys)
                    {
                        Player p = PhotonNetwork.CurrentRoom.Players[k];
                        players.Add(p.NickName,k);
                    }
#endif
                }
                else
                {
                    ui_manager.refreshNbJoueur(PhotonNetwork.CurrentRoom.PlayerCount);
#if UNITY_ANDROID
                    pv.RPC("afficherMsg", RpcTarget.Others, "ceci est un message !");
#endif
                }
            }
        }
    }
}
