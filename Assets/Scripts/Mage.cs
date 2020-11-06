using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Character
{

    public float fireballDamageOnBoss;
    public float fireballDamageOnTeammate;
    public float healAmount;
    public float mortalContactDamage;
    public int numberOfPeopleHealed = 2;

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Fireball();
                DropAction(0);
                break;
            case 1:
                Chloroquine();
                DropAction(1);
                break;
            case 2:
                ImmediateMemory();
                DropAction(2);
                break;
            case 3:
                MortalContact();
                DropAction(3);
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
            if(character.gameObject.name == "Boss")
            {
                character.ReceiveDamage((Encouraged) ? fireballDamageOnBoss * 2 : fireballDamageOnBoss);
            }
            if (character.Exposed)
            {
                character.ReceiveDamage((Encouraged) ? fireballDamageOnTeammate * 2 : fireballDamageOnTeammate);
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
        
        //Heal the ones who get choosen
        for (int i = 0; i < numberOfPeopleHealed; i++)
        {
            foreach (Character character in CharacterManager.sharedInstance.characters)
            {
                if (character.gameObject.name == classesHealed[i]) //If the current character is selected, inflict him damage and remove him from the list for optimization
                {
                    character.ReceiveHeal(healAmount);
                    break;
                }
            }
        }
    }

    private void ImmediateMemory()
    {
        anim.SetTrigger("ImmediateMemory");
        //foreach (var heroe in CharacterManager.sharedInstance.characters)
        //{
        //    heroe.Memoried = true;
        //}
    }

    private void MortalContact()
    {
        anim.SetTrigger("MortalContact");
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name == "Boss")
            {
                character.ReceiveDamage((Encouraged) ? mortalContactDamage * 2 : mortalContactDamage);
            }
            break;
        }
        Exposed = true;
        StartCoroutine(GoToTarget(meleePosition.position, transform));
    }

}
