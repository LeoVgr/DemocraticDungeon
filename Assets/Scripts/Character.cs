using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float _initialLife;

    public static string[] teams = { "Green", "Orange", "Blue", "Violet", "Black", "Pink" };
    public List<string> Actions { get; protected set; }

    public List<string> nomActions;
    public List<string> descActions; 

    public static Dictionary<string, string> ActionsDescription { get; protected set; }
    public Dictionary<string, string> RemainingActions { get; protected set; }
    public Dictionary<string, string> ProposalActions { get; protected set; }

    public int actionToPlay = -1;

    protected Animator anim;
    public int CountActions { get => Actions.Count; }
    public bool Busy { get; protected set; }
    public float Life { get; protected set; }
    public bool Exposed { get; protected set; }
    public bool Protected { get; set; }
    public bool CanBeHealed { get; set; }
    public bool Encouraged { get; set; }
    public bool Invulnerable { get; set; }
    public bool Memoried { get; set; }
    public string Team { get; set; }
    public int TurnPoisoned { get; set; }

    [NonSerialized]
    public Character protector = null;

    protected virtual void Awake()
    {
        ActionsDescription = new Dictionary<string, string>();
        ProposalActions = new Dictionary<string, string>();
        RemainingActions = new Dictionary<string, string>();
        GameManager.sharedInstance.AllActions = new Dictionary<string, string>();
        Actions = new List<string>();

        for (int i=0; i< descActions.Count; i++)
        {
            ActionsDescription.Add(nomActions[i], descActions[i]);
            Actions.Add(nomActions[i]);
           
        }
        RemainingActions = ActionsDescription;
    }

    protected virtual void Start()
    {
        Busy = false;
        Life = _initialLife;
        Exposed = false;
        Protected = false;
        CanBeHealed = false;
        Encouraged = false;
        Invulnerable = false;
        Memoried = false;
        TurnPoisoned = 0;
        anim = GetComponent<Animator>();
    }

    public abstract void PlayAction(int index);

    public void ReceiveDamage(float amount)
    {
        if (Protected && protector != null)
        {
            protector.ReceiveDamage(amount / 2);
            return;
        }

        Life -= amount;
        if (Life < 0)
        {
            Die();
        }

        List<float> lifes = new List<float>();
        foreach (var character in CharacterManager.sharedInstance.characters)
        {
            lifes.Add(character.Life);
        }
        GameManager.sharedInstance.photonManager.updateHp(lifes.ToArray());
    }

    public void ReceiveHeal(float amount)
    {
        if (CanBeHealed)
        {
            if (Life + amount > _initialLife)
            {
                Life = _initialLife;
                return;
            }

            Life += amount;
        }
        
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " dead");
        return;
    }

    public void EndAction()
    {
        Busy = false;
    }

    public void Reset()
    {
        if (TurnPoisoned > 0)
        {
            TurnPoisoned--;
            switch(UnityEngine.Random.Range(0,3))
            {
                case 0:
                    ReceiveDamage(0);
                    break;
                case 1:
                    ReceiveDamage(50);
                    break;
                case 2:
                    ReceiveDamage(100);
                    break;
            }
        }
        CanBeHealed = true;
        Protected = false;
        protector = null;
        Exposed = false;
        Encouraged = false;
        Invulnerable = false;
        Memoried = false;
    }

    public void PickRandomActions()
    {
        ProposalActions.Clear();

        if (RemainingActions.Count <= 0)
        {
            RemainingActions = ActionsDescription;
        }

        int previous = -1;
        if(RemainingActions.Count >=2)
        {
            for (int i = 0; i < 2; i++)
            {
                if (RemainingActions.Count > 0)
                {
                    int randomNumber = UnityEngine.Random.Range(0, RemainingActions.Count);
                    while(randomNumber == previous)
                    {
                        randomNumber = UnityEngine.Random.Range(0, RemainingActions.Count);
                    }

                    previous = randomNumber;
                    ProposalActions.Add(Actions[randomNumber], RemainingActions[Actions[randomNumber]]);

                }
            }
        }
        else
        {
            for (int i = 0; i < 1; i++)
            {
                if (RemainingActions.Count > 0)
                {
                    int randomNumber = UnityEngine.Random.Range(0, RemainingActions.Count);
                    while (randomNumber == previous)
                    {
                        randomNumber = UnityEngine.Random.Range(0, RemainingActions.Count);
                    }
             
                    print("RANDOM : " + Actions.Count);
                    ProposalActions.Add(Actions[randomNumber], RemainingActions[Actions[randomNumber]]);

                }
            }
        }
        
      

    }

    public void DropAction(int index)
    {
        if (!Memoried)
        {
            RemainingActions.Remove(Actions[index]);
        }
        else
        {
            if(UnityEngine.Random.Range(0,1) == 0)
            {
                RemainingActions.Remove(Actions[index]);
            }
        }
    }
}
