﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Character
{

    public float healPotion;

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                HealPotion();
                break;
            case 1:
                BackStab();
                break;
            case 2:
                Action2();
                break;
            case 3:
                Action3();
                break;
            default:
                Busy = false;
                Debug.Log("Wrong index " + index);
                break;
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void HealPotion()
    {
        anim.SetTrigger("HealPotion");
        ReceiveHeal(healPotion);
    }

    private void BackStab()
    {
        anim.SetTrigger("BackStab");
        int numberOfPeopleNearTheBoss = 0;
        foreach(Character character in CharacterManager.sharedInstance.characters)
        {
            if( character.gameObject.name != "Boss" && character.Exposed)
            {
                numberOfPeopleNearTheBoss++;
            }
        }

        int damage = 0;
        if (numberOfPeopleNearTheBoss < 4)
        {
            damage = 50 + 50 * numberOfPeopleNearTheBoss;
        }
        else
        {
            damage = 500;
        }

        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name == "Boss")
            {
                character.ReceiveDamage(damage);
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


}
