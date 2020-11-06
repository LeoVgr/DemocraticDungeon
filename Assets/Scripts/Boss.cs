using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{
    private Character target = null;
    [SerializeField]
    private float meleeHitDamage;
    [SerializeField]
    private float distanceAttackDamage;
    [SerializeField]
    private float healAmount;
    protected override void Start()
    {
        base.Start();
        Exposed = true;
    }

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Scream();
                DropAction(0);
                break;
            case 1:
                MeleeHit();
                DropAction(1);
                break;
            case 2:
                DistanceAttack();
                DropAction(2);
                break;
            case 3:
                Heal();
                DropAction(3);
                break;
            default:
                Busy = false;
                Debug.Log("Wrong index " + index);
                break;
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void Scream()
    {
        anim.SetTrigger("Scream");
        //for (int i = 1; i < GameManager.sharedInstance.orderedPlayers.Count; i ++)
        //{
        //    if (Random.Range(0,2) == 0)
        //    {
        //        //Play animation cancel
        //        GameManager.sharedInstance.orderedPlayers.RemoveAt(i);
        //    }
        //}
    }

    private void MeleeHit()
    {
        anim.SetTrigger("MeleeHit");
        if (target != null)
        {
            if (target.Exposed)
            {
                target.ReceiveDamage(meleeHitDamage);
            }
            target = null;
            return;
        }
        foreach(Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name != "Boss" && character.Exposed )
            {               
                character.ReceiveDamage(meleeHitDamage);
            }
        }
    }

    private void DistanceAttack()
    {
        anim.SetTrigger("DistanceAttack");
        if (target != null)
        {
            if (target.Exposed)
            {
                target.ReceiveDamage(distanceAttackDamage);
            }
            target = null;
            return;
        }
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name != "Boss")
            {
                character.ReceiveDamage(distanceAttackDamage);
            }
        }
    }
    private void Heal()
    {
        anim.SetTrigger("Heal");
        ReceiveHeal(healAmount);
        //Need to shuffle the rest of ordered player. Problem: the UI has to be implement, also when the boss uses this action he has to be the first of the list
    }

    public void Taunt(Character character)
    {
        target = character;
    }
}
