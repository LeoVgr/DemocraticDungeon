using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Paladin : Character
{
    [Header("Exemple : 5 = 1/5 probability")]
    public int badDivineStrikeProbability = 5;
    public float healAmount;
    public float divineStrikeDamage;
    public float divineStrikeFriendlyDamage;

    public override sealed void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Heal();
                DropAction(0);
                break;
            case 1:
                DivineStrike();
                DropAction(1);
                break;
            case 2:
                Protect();
                DropAction(2);
                break;
            case 3:
                Taunt();
                DropAction(3);
                break;
            default:
                Busy = false;
                Debug.Log("Wrong index " + index);
                break;
        }
    }

    //Here busy = false because we don't an animation yet, but we should do it as an key event during the animation

    private void Heal()
    {
        anim.SetTrigger("Heal");
        foreach(Character character in CharacterManager.sharedInstance.characters)
        {
            if(character.gameObject.name == "Boss")
            {
                continue;
            }
            character.ReceiveHeal(healAmount);
        }
    }

    private void DivineStrike()
    {
        if (Random.Range(0, badDivineStrikeProbability) == 0) // Chance to get bad result
        {
            anim.SetTrigger("BadDivineStrike");
            //Damage the paladin
            ReceiveDamage(50);
            //Calculate how many people are going to get damaged
            int numberOfPeopleDamaged = Random.Range(2, 5);
            float damage = divineStrikeFriendlyDamage / (float)numberOfPeopleDamaged;
            //Choose who is going to get hurt
            List<string> classesDamaged = new List<string>(numberOfPeopleDamaged);
            List<string> classes = new List<string>(4);
            classes.Add("Archer");
            classes.Add("Assassin");
            classes.Add("Barbare");
            classes.Add("Mage");
            
            for(int i = 0; i < numberOfPeopleDamaged; i++)
            {
                int rand = Random.Range(0, 4 - i);
                classesDamaged.Add(classes[rand]);
                classes.RemoveAt(rand);
            }

            //Hurt the ones who get choosen
            foreach(Character character in CharacterManager.sharedInstance.characters)
            {
                for(int i = 0; i < numberOfPeopleDamaged; i++)
                {
                    if (character.gameObject.name == classesDamaged[i]) //If the current character is selected, inflict him damage and remove him from the list for optimization
                    {
                        character.ReceiveDamage((Encouraged) ? divineStrikeFriendlyDamage * 2 : divineStrikeFriendlyDamage);
                        classesDamaged.RemoveAt(i);
                        numberOfPeopleDamaged--;
                        break;
                    }
                }
                if (numberOfPeopleDamaged == 0)
                {
                    break;
                }
            }
            return;
        }
        //
        anim.SetTrigger("GoodDivineStrike");
        foreach(Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name == "Boss")
            {
                character.ReceiveDamage((Encouraged)?divineStrikeDamage*2:divineStrikeDamage);
                break;
            }        
        }
    }

    private void Protect()
    {
        anim.SetTrigger("Protect");
        Character protect = null;
        float life = Mathf.Infinity;
        foreach(Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name != "Boss" && character.gameObject.name != "Paladin" && character.Life < life)
            {
                life = character.Life;
                protect = character;
            }
        }

        if (protect == null)
        {
            Debug.LogError("Error in Protect, Paladin.cs");
            return;
        }
        
        protect.Protected = true;
        protect.protector = this;
    }

    private void Taunt()
    {
        anim.SetTrigger("Taunt");
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name == "Boss")
            {
                ((Boss)character).Taunt(this);
            }
            break;
        }

    }


}
