using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float _initialLife;

    [SerializeField]
    protected List<string> actions;

    public int CountActions { get => actions.Count; }
    public bool Busy { get; protected set; }
    public float Life { get; protected set; }

    public string GetActionName(int index)
    {
        if (index < CountActions)
        {
            return actions[index];
        }
        return "";
    }

    public abstract void PlayAction(int index);

    public void ReceiveDamage(float amount)
    {
        Life -= amount;
        if (Life < 0)
        {
            Die();
        }
    }

    public void ReceiveHeal(float amount)
    {
        if (Life + amount > _initialLife)
        {
            Life = _initialLife;
            return;
        }

        Life += amount;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " dead");
        return;
    }
}
