using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float _initialLife;

    public List<string> Actions { get; protected set; }
    public List<string> RemainingActions { get; protected set; }
    public List<string> ProposalActions { get; protected set; }

    protected Animator anim;
    public int CountActions { get => Actions.Count; }
    public bool Busy { get; protected set; }
    public float Life { get; protected set; }
    public bool Exposed { get; protected set; }
    public bool Protected { get; set; }
    public bool CanBeHealed { get; set; }
    public bool Encouraged { get; set; }
    public bool Invulnerable { get; set; }

    [NonSerialized]
    public Character protector = null;

    private void Awake()
    {
        RemainingActions = Actions;
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
        CanBeHealed = true;
        Protected = false;
        protector = null;
        Exposed = false;
        Encouraged = false;
        Invulnerable = false;
    }

    public void PickRandomActions()
    {
        if(RemainingActions.Count <= 0)
        {
            RemainingActions = Actions;
        }

        for (int i = 0; i < 2; i++)
        {
            if(RemainingActions.Count > 0)
            {
                int randomNumber = UnityEngine.Random.Range(0, RemainingActions.Count);
                ProposalActions.Add(RemainingActions[randomNumber]);
                RemainingActions.RemoveAt(randomNumber);
            }
        }
      

    }
}
