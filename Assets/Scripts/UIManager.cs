using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PhotonManager photon_manager;

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
    private string str_wait = "Waiting Room\nPlayers : ";

    /*[SerializeField]
    private string str_members = "Members : ";

    [SerializeField]
    private string str_votes_restants = "Votes Restants :\n";*/

    public Color color;

    public int nb_joueur = 0;

    //Button
    [SerializeField]
    Button bt_start_start;
    [SerializeField]
    Button bt_start_quit;

    [SerializeField]
    Button bt_select_green;
    [SerializeField]
    Button bt_select_orange;
    [SerializeField]
    Button bt_select_blue;
    [SerializeField]
    Button bt_select_violet;
    [SerializeField]
    Button bt_select_black;
    [SerializeField]
    Button bt_select_pink;
    [SerializeField]
    Button bt_select_quit;

    /*[SerializeField]
    Button bt_members_plus;
    [SerializeField]
    Button bt_members_minus;*/

    [SerializeField]
    Image im_attaque1;
    [SerializeField]
    Image im_attaque2;
    [SerializeField]
    Button bt_attaque_1;
    [SerializeField]
    Button bt_attaque_2;


    /*[SerializeField]
    Button bt_attaque_1_m;
    [SerializeField]
    Button bt_attaque_2_m;
    [SerializeField]
    Button bt_validate;*/


    public Text wait_txt;
    public Text classe_txt;
    [SerializeField]
    Text team_txt;
    [SerializeField]
    Text attaque_1_title_txt;
    [SerializeField]
    Text attaque_1_desc_txt;
    [SerializeField]
    Text attaque_2_title_txt;
    [SerializeField]
    Text attaque_2_desc_txt;
    [SerializeField]
    GameObject alerte_attaque_1;
    [SerializeField]
    GameObject alerte_attaque_2;

    [SerializeField]
    Image im_health;
    float hp_max_px;

    //public Text members_txt;
    //public Text votes_restants_txt;


    public void refreshNbJoueur(int nb)
    {
        nb_joueur = nb;
        wait_txt.text = str_wait + nb_joueur;
    }

    void Awake()
    {
        //set up des Boutons
        bt_start_start.onClick.AddListener(clickStart);
        bt_start_quit.onClick.AddListener(clickQuit);

        bt_select_green.onClick.AddListener(() => { clickColor(bt_select_green.name);});
        bt_select_orange.onClick.AddListener(() => { clickColor(bt_select_orange.name); });
        bt_select_blue.onClick.AddListener(() => { clickColor(bt_select_blue.name); });
        bt_select_violet.onClick.AddListener(() => { clickColor(bt_select_violet.name); });
        bt_select_black.onClick.AddListener(() => { clickColor(bt_select_black.name); });
        bt_select_pink.onClick.AddListener(() => { clickColor(bt_select_pink.name); });
        bt_select_quit.onClick.AddListener(clickQuit);
        bt_attaque_1.onClick.AddListener(() => { addVotes(0); });
        bt_attaque_2.onClick.AddListener(() => { addVotes(1); });
        /*bt_attaque_1_m.onClick.AddListener(() => { addVotes(0, -1); });
        bt_attaque_2_m.onClick.AddListener(() => { addVotes(1, -1); });
        bt_validate.onClick.AddListener(send);

        bt_members_plus.onClick.AddListener(() => { setMembers(1); });
        bt_members_minus.onClick.AddListener(() => { setMembers(-1); });*/
        hp_max_px = im_health.rectTransform.rect.width;

        wait_txt.text = str_wait + nb_joueur;
    }

    public void send()
    {
        //ici send et compléter en local si pas tout les votes, avec votes aléa
        photon_manager.Sending();
        photon_manager.setRefreshMode(false);
    }

    public void updateColorBackground()
    {
        Image im = WaitingMenu.GetComponentInChildren<Image>();
        im.color = color;
        im = GameChoice.GetComponentInChildren<Image>();
        im.color = color;
        im = GameTurn.GetComponentInChildren<Image>();
        im.color = color;
    }

    public void updateActionChoice(string [] action_names,bool [] alerte)
    {
        attaque_1_title_txt.text = action_names[0];
        attaque_1_desc_txt.text = photon_manager.description_actions[action_names[0]];
        alerte_attaque_1.SetActive(alerte[0]);
        if(action_names.Length >= 2)
        {
            bt_attaque_2.enabled = true;
            attaque_2_title_txt.text = action_names[1];
            attaque_2_desc_txt.text = photon_manager.description_actions[action_names[1]];
            alerte_attaque_2.SetActive(alerte[1]);
        }
        else
        {
            bt_attaque_2.enabled = false;
        }
    }

    public void addVotes(int num)
    {
        bool act_1, act_2;
        act_1 = act_2 = false;
        switch (num)
        {
            case 0:
                act_1 = true;
                photon_manager.setVotes(0);
                break;
            case 1:
                act_2 = true;
                photon_manager.setVotes(1);
                break;
            default:
                break;
        }
        im_attaque1.enabled = act_1;
        im_attaque2.enabled = act_2;
    }

    /*public void addVotes(int num,int nb)
    {
        if (photon_manager.setVotes(num, nb))
        {
            votes_restants_txt.text = str_votes_restants + photon_manager.nb_votes;
        }
    }*/

    /*public void setMembers(int n)
    {
        if (photon_manager.setMembers(n))
            members_txt.text = str_members + photon_manager.getMembers();
    }*/

    public void clickStart()
    {
        //join or create
        bool state = photon_manager.isConnecting;
        if (state)
        {
#if UNITY_ANDROID
            activateMenu("select");
#elif UNITY_STANDALONE_WIN
            photon_manager.connectMasterClient();
#endif
        }
    }
    public void clickColor(string name)
    {
        bool state = photon_manager.verifySelectColor(name);
        if (state)
        {
            switch (name)
            {
                case "Green":
                    color = new Color(0.384f, 0.8f, 0.251f);
                    break;
                case "Orange":
                    color = new Color(0.886f, 0.624f, 0.239f);
                    break;
                case "Blue":
                    color = new Color(0.035f, 0.667f, 0.859f);
                    break;
                case "Black":
                    color = new Color(0.184f, 0.196f, 0.2f);
                    break;
                case "Violet":
                    color = new Color(0.533f, 0.298f, 0.659f);
                    break;
                case "Pink":
                    color = new Color(0.918f, 0.451f, 0.886f);
                    break;
                default:
                    break;
            }
            updateColorBackground();
            team_txt.text = name;
            activateMenu("wait");
        }
    }
    public void clickQuit()
    {
        photon_manager.disconnected();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
              Application.Quit();
#endif
    }

    public void updateHp(float poucent)
    {
        im_health.rectTransform.sizeDelta = new Vector2(hp_max_px * poucent,im_health.rectTransform.sizeDelta.y);
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
}
