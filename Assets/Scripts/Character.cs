using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float _initialLife;
    public bool Busy { get; protected set; }

    public float Life { get; protected set; }

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
