using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private MainMenu menu_manager;

    static UIManager instance = null;

    [SerializeField]
    private GameObject StartMenu;
    [SerializeField]
    private GameObject SelectMenu;
    [SerializeField]
    private GameObject WaitingMenu;
    [SerializeField]
    private GameObject GameChoice;
    [SerializeField]
    private GameObject GameTurn;
    [SerializeField]
    private GameObject Number;

    [SerializeField]
    private string str_wait = "Waiting Room\nPlayers : ";

    [SerializeField]
    private string str_members = "Members : ";

    [SerializeField]
    private string str_votes_restants = "Votes Restants :\n";


    public int nb_joueur = 0;

    //Button
    [SerializeField]
    Button bt_start_start;
    [SerializeField]
    Button bt_start_quit;

    [SerializeField]
    Button bt_select_green;
    [SerializeField]
    Button bt_select_red;
    [SerializeField]
    Button bt_select_blue;
    [SerializeField]
    Button bt_select_white;
    [SerializeField]
    Button bt_select_black;
    [SerializeField]
    Button bt_select_pink;
    [SerializeField]
    Button bt_select_quit;

    [SerializeField]
    Button bt_members_plus;
    [SerializeField]
    Button bt_members_minus;

    [SerializeField]
    Button bt_attaque_1;
    [SerializeField]
    Button bt_attaque_2;
    [SerializeField]
    Button bt_attaque_1_m;
    [SerializeField]
    Button bt_attaque_2_m;
    [SerializeField]
    Button bt_validate;


    public Text wait_txt;
    public Text members_txt;
    public Text votes_restants_txt;


    // Start is called before the first frame update

    public static UIManager getInstance()
    {
        return instance;
    }

    public void refreshNbJoueur(int nb)
    {
        nb_joueur = nb;
        wait_txt.text = str_wait + nb_joueur;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        //set up des Boutons
        bt_start_start.onClick.AddListener(clickStart);
        bt_start_quit.onClick.AddListener(clickQuit);

        bt_select_green.onClick.AddListener(() => { clickColor("green");});
        bt_select_red.onClick.AddListener(() => { clickColor("red"); });
        bt_select_blue.onClick.AddListener(() => { clickColor("blue"); });
        bt_select_white.onClick.AddListener(() => { clickColor("white"); });
        bt_select_black.onClick.AddListener(() => { clickColor("black"); });
        bt_select_pink.onClick.AddListener(() => { clickColor("pink"); });
        bt_select_quit.onClick.AddListener(clickQuit);
        bt_attaque_1.onClick.AddListener(() => { addVotes(0, 1); });
        bt_attaque_2.onClick.AddListener(() => { addVotes(1, 1); });
        bt_attaque_1_m.onClick.AddListener(() => { addVotes(0, -1); });
        bt_attaque_2_m.onClick.AddListener(() => { addVotes(1, -1); });
        bt_validate.onClick.AddListener(send);

        bt_members_plus.onClick.AddListener(() => { setMembers(1); });
        bt_members_minus.onClick.AddListener(() => { setMembers(-1); });

        wait_txt.text = str_wait + nb_joueur;
    }
    void Start()
    {

    }

    public void send()
    {
        //ici send et compléter en local si pas tout les votes, avec votes aléa
    }

    public void addVotes(int num,int nb)
    {
        if (menu_manager.setVotes(num, nb))
        {
            votes_restants_txt.text = str_votes_restants + menu_manager.nb_votes;
        }
    }

    public void setMembers(int n)
    {
        if (menu_manager.setMembers(n))
            members_txt.text = str_members + menu_manager.getMembers();
    }
    public void clickStart()
    {
        //join or create
        bool state = menu_manager.isConnecting;
        if (state)
        {
            activateMenu("select");
        }
    }
    public void clickColor(string name)
    {
        bool state = menu_manager.verifySelectColor(name);
        if (state)
        {
            activateMenu("wait");
        }
    }
    public void clickQuit()
    {
        menu_manager.disconnected();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
              Application.Quit();
        #endif
    }

    public void activateMenu(string s)
    {
        bool start, select, wait,choice,turn;
        start = select = wait = choice = turn = false;
        switch (s)
        {
            case "start":
                start = true;
                break;
            case "select":
                select = true;
                break;
            case "wait":
                wait = true;
                break;
            case "choice":
                choice = true;
                break;
            case "turn":
                turn = true;
                break;
            default:
                break;
        }
        StartMenu.SetActive(start);
        SelectMenu.SetActive(select);
        WaitingMenu.SetActive(wait);
        GameChoice.SetActive(choice);
        GameTurn.SetActive(turn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
