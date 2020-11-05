using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{

    public float fireballDamage;
    public float healAmount;
    public float mortalContactDamage;
    public int numberOfPeopleHealed = 2;

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
                Chloroquine();
                break;
            case 2:
                Action2();
                break;
            case 3:
                MortalContact();
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
            if(character.Exposed || character.gameObject.name == "Boss")
            {
                character.ReceiveDamage(fireballDamage);
            }
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void Chloroquine()
    {
        anim.SetTrigger("Chloroquine");
        //Choose who is going to get healed
        List<string> classesHealed = new List<string>(numberOfPeopleHealed);
        List<string> classes = new List<string>(5);
        classes.Add("Archer");
        classes.Add("Assassin");
        classes.Add("Barbare");
        classes.Add("Mage");
        classes.Add("Paladin");

        for (int i = 0; i < numberOfPeopleHealed; i++)
        {
            int rand = Random.Range(0, 5 - i);
            classesHealed.Add(classes[rand]);
            classes.RemoveAt(rand);
        }

        //Hurt the ones who get choosen
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            for (int i = 0; i < numberOfPeopleHealed; i++)
            {
                if (character.gameObject.name == classesHealed[i]) //If the current character is selected, inflict him damage and remove him from the list for optimization
                {
                    classesHealed.RemoveAt(i);
                    character.ReceiveHeal(healAmount);
                    numberOfPeopleHealed--;
                    break;
                }
            }
            if (numberOfPeopleHealed == 0)
            {
                break;
            }
        }
        return;
    }

    private void Action2()
    {
        Busy = false;
    }

    private void MortalContact()
    {
        anim.SetTrigger("MortalContact");
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name == "Boss")
            {
                character.ReceiveDamage(mortalContactDamage);
            }
            break;
        }
        Exposed = true;
    }

}
