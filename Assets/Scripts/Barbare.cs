using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbare : Character
{
    public float magicKissPower;
    public float headButtDamage;
    public float headButtSelfDamage;

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                MagicKiss();
                break;
            case 1:
                KnockOut();
                break;
            case 2:
                HeadButt();
                break;
            case 3:
                BeastlyStrike();
                break;
            default:
                Busy = false;
                Debug.Log("Wrong index " + index);
                break;
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void MagicKiss()
    {
        anim.SetTrigger("MagicKiss");
        ReceiveHeal(magicKissPower);
    }

    private void KnockOut()
    {
        anim.SetTrigger("KnockOut");

        Character boss = CharacterManager.sharedInstance.characters[0];

        foreach (var item in CharacterManager.sharedInstance.characters)
        {
            if(item.gameObject.name == "Boss")
            {
                boss = item;
            }
        }
        
        if(Life <=400 && Life >300)
        {
            boss.ReceiveDamage((Encouraged) ? 100 * 2 : 100);
        }

        if (Life <= 300 && Life > 200)
        {
            boss.ReceiveDamage((Encouraged) ? 150 * 2 : 150);
        }

        if (Life <= 200 && Life > 100)
        {
            boss.ReceiveDamage((Encouraged) ? 200 * 2 : 200);
        }

        if (Life <= 100 && Life > 20)
        {
            boss.ReceiveDamage((Encouraged) ? 300 * 2 : 300);
        }

        if (Life <= 50)
        {
            boss.ReceiveDamage((Encouraged) ? 500 * 2 : 500);
        }


    }

    private void HeadButt()
    {
        anim.SetTrigger("HeadButt");
        ReceiveDamage((Encouraged) ? headButtSelfDamage * 2 : headButtSelfDamage);

        foreach (var item in CharacterManager.sharedInstance.characters)
        {
            if (item.gameObject.name == "Boss")
            {
                item.ReceiveDamage((Encouraged) ? headButtDamage * 2 : headButtDamage);
            }
        }

    }
    private void BeastlyStrike()
    {
        anim.SetTrigger("BeastlyStrike");
        //TODO
    }

}
