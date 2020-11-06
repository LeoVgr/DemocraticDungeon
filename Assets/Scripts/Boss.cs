using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{
    private Character target = null;
    [SerializeField]
    private float meleeHitDamage;
    protected override void Start()
    {
        base.Start();
        Exposed = true;
    }

    private void Update()
    {
        anim.SetFloat("Life",Life);
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
                Action2();
                DropAction(2);
                break;
            case 3:
                Action3();
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
        if (GameManager.sharedInstance.orderedPlayers.Count == 1) return;
        for (int i = 1; i < GameManager.sharedInstance.orderedPlayers.Count; i ++)
        {
            if (Random.Range(0,2) == 0)
            {
                //Play animation cancel
                GameManager.sharedInstance.orderedPlayers.RemoveAt(i);
            }
        }
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

    private void Action2()
    {
        Busy = false;
    }
    private void Action3()
    {
        Busy = false;
    }

    public void Taunt(Character character)
    {
        target = character;
    }

}
