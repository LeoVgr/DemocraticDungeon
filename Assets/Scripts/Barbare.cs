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
                DropAction(0);
                break;
            case 1:
                KnockOut();
                DropAction(1);
                break;
            case 2:
                HeadButt();
                DropAction(2);
                break;
            case 3:
                BeastlyStrike();
                DropAction(3);
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
        Exposed = true;
        StartCoroutine(GoToTarget(meleePosition.position, transform));

        foreach (var boss in CharacterManager.sharedInstance.characters)
        {
            if(boss.gameObject.name == "Boss")
            {
                if (Life > 300)
                {
                    boss.ReceiveDamage((Encouraged) ? 100 * 2 : 100);
                }
                else if (Life <= 300 && Life > 200)
                {
                    boss.ReceiveDamage((Encouraged) ? 150 * 2 : 150);
                }
                else if (Life <= 200 && Life > 100)
                {
                    boss.ReceiveDamage((Encouraged) ? 200 * 2 : 200);
                }
                else if (Life <= 100 && Life > 20)
                {
                    boss.ReceiveDamage((Encouraged) ? 300 * 2 : 300);
                }
                else if (Life <= 50)
                {
                    boss.ReceiveDamage((Encouraged) ? 500 * 2 : 500);
                }
            }
        }
        

    }

    private void HeadButt()
    {
        anim.SetTrigger("HeadButt");
        Exposed = true;
        StartCoroutine(GoToTarget(meleePosition.position, transform));
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
        Exposed = true;
        //Character boss;
        //Exposed = true;
        //foreach (var heroe in GameManager.sharedInstance.orderedPlayers)
        //{
        //    if(heroe.gameObject.name == "Boss")
        //    {
        //        boss = heroe;
        //        GameManager.sharedInstance.orderedPlayers.Remove(boss);
        //        GameManager.sharedInstance.orderedPlayers.Add(boss);

                

        //    }
        //}
        //RemainingActions.Remove(Actions[Random.Range(0,RemainingActions.Count)]);
    }

}
