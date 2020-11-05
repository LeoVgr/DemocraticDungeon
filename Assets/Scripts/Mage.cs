using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{

    public float fireballDamage;

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Debug.Log("Play " + actions[0]);
                Fireball();
                break;
            case 1:
                Action1();
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


    private void Fireball()
    {
        anim.SetTrigger("Fireball");
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if(character.Exposed)
            {
                character.ReceiveDamage(fireballDamage);
            }
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void Action1()
    {
        Busy = false;
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
