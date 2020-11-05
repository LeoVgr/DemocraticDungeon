using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    //public GameObject roomJoinUI;
    [SerializeField]
    UIManager ui_manager;
    
    public bool DEBUG_MODE = false;

    public int NB_PLAYERS_MAX = 2;
    public float TURN_TIME = 20.0f;

    private string gameVersion = "0.1";
    private string roomName = "Room 1";

    public int nb_member = 1;

    List<RoomInfo> createdRooms = new List<RoomInfo>();

    private Text connectionStatus;
    public bool isConnecting = false;
    private bool gameLaunch = false;

    public bool mode_choice = false;
    private float timer_turn = 0.0f;
    private bool refresh = false;

    public int nb_votes;
    public int nb_votes_action_1;
    public int nb_votes_action_2;

    PhotonView pv;

    void Awake()
    {
        //a verifier
        PhotonNetwork.AutomaticallySyncScene = true;
        pv = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ui_manager.activateMenu("start");
        /*if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }*/
        PlayerPrefs.DeleteAll();
        Debug.Log("Connecting to Photon Network");

        ConnectToPhoton();
    }

    public bool setVotes(int type,int nb)
    {
        bool res = false;
        if (mode_choice)
        {
            if((type == 0 ? (nb_votes_action_1 + nb >= 0) : (nb_votes_action_2 + nb >= 0)))
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
        Debug.Log("Set vote Main Menu "+res);
        return res;
    }

    //etude d'un controleur avec les phase Began et End => pour mouvement
    //Mais la problème avec vote, pour le moment boutons

    [PunRPC]
    public void afficherMsg(string msg)
    {
        Debug.Log("Message : "+msg);
    }

    public int getMembers()
    {
        return nb_member;
    }

    public bool setMembers(int n)
    {
        Debug.Log("Ici set members " + n);
        nb_member += n;
        if (nb_member < 1)
        {
            nb_member = 1;
            return false;
        }
        return true;
    }

    public bool verifySelectColor(string color)
    {
        bool res = JoinRoom(color);
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
                    pv.RPC("setRefreshMode", RpcTarget.Others, mode_choice);
                    //RpcTarget.Others
                refresh = false;
                    Debug.Log("Changement de mode");
                }
#endif
            /*if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {

            }

            //side players + boss
            if (Application.platform == RuntimePlatform.Android)
            {

            }*/
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
        //connectionStatus.text = "Connected to Photon";
        //connectionStatus.color = Color.green;
        //set button active to join
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.Log("Disconnected.");
    }

    /*
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnected");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have callback, update the room list");
        createdRooms = roomList;
    }*/

    public override void OnJoinedRoom()
    {
        Debug.Log("Success to join de room");
#if UNITY_STANDALONE_WIN
            bool connected = PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
            Debug.Log("Connected master est de " + connected);
#endif
        /*if (PhotonNetwork.IsMasterClient)
        {
            //set ui
            //pc
        }
        else
        {
            //playerStatus
            //smartphone
        }*/
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
            //pas vraiment if faudrait verifier si elle existe avant pour que le pc soit le master
            
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);

            //#if UNITY_ANDROID
                    
                //PhotonNetwork.CurrentRoom.IsOpen = false;
                //Debug.Log("Creation Room "+connected+"    "+ PhotonNetwork.CountOfRooms+" et "+PhotonNetwork.CurrentRoom.Name);
            //pour pc
            /*else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                connected = PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Creation Room");
            }
            //pour smartphone
            else if (Application.platform == RuntimePlatform.Android)
                connected = PhotonNetwork.JoinRoom(roomName);
            */
        }
        return connected;
    }

    void ConnectToPhoton()
    {
        //connectionStatus.text = "Connecting...";
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    [PunRPC]
    void setRefreshMode(bool state)
    {
        mode_choice = state;
        ui_manager.activateMenu((mode_choice ? "choice" : "turn"));
        if (mode_choice)
        {
            nb_votes = nb_member;
            nb_votes_action_1 = nb_votes_action_2 = 0;
            ui_manager.addVotes(0);
        }

        //refresh = true;
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
            if(PhotonNetwork.CurrentRoom != null)
            {
                if(PhotonNetwork.CurrentRoom.PlayerCount >= NB_PLAYERS_MAX)
                {
                    //pv = PhotonView.Get(this);
                    timer_turn = Time.realtimeSinceStartup;
#if UNITY_ANDROID
                    gameLaunch = true;
                    setRefreshMode(true);
                    //UIManager.getInstance().activateMenu("turn");
#elif UNITY_STANDALONE_WIN
                    pv.RPC("setLaunch", RpcTarget.All, true);
                    ui_manager.activateMenu("");
#endif
                }
                else
                {
                    ui_manager.refreshNbJoueur(PhotonNetwork.CurrentRoom.PlayerCount);
                    #if UNITY_ANDROID
                                pv.RPC("afficherMsg",RpcTarget.Others,"ceci est un message !");
                    #endif
                }
            }
        }
    }

    /*
    public void JoinRoom()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.player.NickName = "";
            Debug.Log("Photon.Network.connected() | Trying to Create/Join Room");
            RoomOptions roomOptions = new RoomOptions();
            TypedLobby typedLobby = new TypedLobby("", LobbyType.Default);
            PhotonNetwork.JoinOrCreateRoom("", roomOptions, typedLobby);
        }
    }
    public void LoadArena()
    {
        if(PhotonNetwork.room.PlayerCount > 1)
        {
            PhotonNetwork.LoadLevel("MainArena");
        }
        else
        {
            //Minimum 2 Players required to Load Arena!;
        }
    }
    */
    /*public override void OnConnected()
    {
        ;
    }*/
}
