using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character 
{

    public float ambushDamage;

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Debug.Log("Action0");
                Ambush();
                break;
            case 1:
                Debug.Log("Action1");
                Action1();
                break;
            case 2:
                Debug.Log("Action2");
                Action2();
                break;
            case 3:
                Debug.Log("Action3");
                Action3();
                break;
            default:
                Busy = false;
                Debug.Log("Wrong index " + index);
                break;
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void Ambush()
    {
        // Busy = false;
        anim.SetTrigger("Ambush");
        Exposed = true;

        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.name == "Boss")
            {
                character.ReceiveDamage(ambushDamage);
                character.CanBeHealed = false;
            }
        }

    }

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
