using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float _initialLife;

    protected HP hpBar;

    protected CharacterSound sounds;

    public static string[] teams = { "Green", "Orange", "Blue", "Violet", "Black", "Pink" };
    public List<string> Actions { get; protected set; }

    public List<string> nomActions;
    public List<string> descActions; 

    public static Dictionary<string, string> ActionsDescription { get; protected set; }
    public Dictionary<string, string> RemainingActions { get; protected set; }
    public Dictionary<string, string> ProposalActions { get; protected set; }

    [SerializeField]
    protected Vector3 initialPosition = new Vector3();
    [SerializeField]
    protected Transform meleePosition = null;
    public int actionToPlay = -1;

    protected Animator anim;
    public int CountActions { get => Actions.Count; }
    public bool Busy { get; protected set; }
    public float Life { get; protected set; }
    [SerializeField]
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

    //protected void Awake()
    //{
    //    ActionsDescription = new Dictionary<string, string>();
    //    ProposalActions = new Dictionary<string, string>();
    //    RemainingActions = new Dictionary<string, string>();
    //    GameManager.sharedInstance.AllActions = new Dictionary<string, string>();
    //    Actions = new List<string>();

    //    for (int i=0; i< descActions.Count; i++)
    //    {
    //        ActionsDescription.Add(nomActions[i], descActions[i]);
    //        Actions.Add(nomActions[i]);
           
    //    }
    //    RemainingActions = ActionsDescription;
    //}

    protected virtual void Start()
    {
        Busy = false;
        Life = _initialLife;
        Exposed = false;
        Protected = false;
        CanBeHealed = true;
        Encouraged = false;
        Invulnerable = false;
        Memoried = false;
        TurnPoisoned = 0;
        anim = GetComponent<Animator>();
        anim.SetFloat("Life", Life);
        hpBar = GetComponentInChildren<HP>();
        sounds = GetComponent<CharacterSound>();
        initialPosition = transform.position;
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
        hpBar.CalculatePosition(Life, _initialLife);
        anim.SetFloat("Life", Life);
        //List<float> lifes = new List<float>();
        //foreach (var character in CharacterManager.sharedInstance.characters)
        //{
        //    lifes.Add(character.Life);
        //}
        //GameManager.sharedInstance.photonManager.updateHp(lifes.ToArray());
        if (Life < 0)
        {
            Die();
        }
        else
        {
            sounds.HurtSound();
        }

    }


    public void ReceiveHeal(float amount)
    {
        if (CanBeHealed && Life > 0)
        {

            if (Life + amount > _initialLife)
            {
                Life = _initialLife;
                hpBar.CalculatePosition(Life, _initialLife);
                return;
            }

            Life += amount;
            anim.SetFloat("Life", Life);
            hpBar.CalculatePosition(Life, _initialLife);
        }
        
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " dead");
        sounds.DeathSound();
        return;
    }

    public void EndAction()
    {
        OrderUI.sharedInstance.Next();
        Busy = false;
        Test.canPlay = true;
    }

    public void Reset()
    {
        if (TurnPoisoned > 0)
        {
            TurnPoisoned--;
            Debug.Log(TurnPoisoned);
            switch (UnityEngine.Random.Range(0,3))
            {
                case 0:
                    Debug.Log(0);
                    ReceiveDamage(0);
                    break;
                case 1:
                    Debug.Log(50);
                    ReceiveDamage(50);
                    break;
                case 2:
                    Debug.Log(100);
                    ReceiveDamage(100);
                    break;
            }
        }
        CanBeHealed = true;
        Protected = false;
        protector = null;
        if (Exposed)
        {
            Exposed = false;
            StartCoroutine(GoToTarget(initialPosition, transform));
        }

        Encouraged = false;
        Invulnerable = false;
        Memoried = false;
    }

    public void PickRandomActions()
    {
        if (ProposalActions != null) ProposalActions.Clear();

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
        //if (!Memoried)
        //{
        //    RemainingActions.Remove(Actions[index]);
        //}
        //else
        //{
        //    if(UnityEngine.Random.Range(0,1) == 0)
        //    {
        //        RemainingActions.Remove(Actions[index]);
        //    }
        //}
    }

    protected IEnumerator GoToTarget(Vector3 position, Transform objectToMove)
    {
        float t = 0;
        while (objectToMove.position != position)
        {
            Vector3 newPosition = new Vector3(Mathf.Lerp(objectToMove.position.x, position.x, t),
                                               Mathf.Lerp(objectToMove.position.y, position.y, t),
                                               objectToMove.position.z);
            objectToMove.position = newPosition;
            t += 0.2f;
            yield return new WaitForFixedUpdate();
        }
    }
}
