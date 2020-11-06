using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Character 
{

    public float ambushDamage;
    public float approximateShotDamageOnBoss;
    public float approximateShotDamageOnTeammate;
    public float swarmOfArrowDamageOnBoss;
    public float swarmOfArrowDamageOnTeammate;

    public override void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Debug.Log("Action0");
                Ambush();
                DropAction(0);
                break;
            case 1:
                Debug.Log("Action1");
                ApproximateShot();
                DropAction(1);
                break;
            case 2:
                Debug.Log("Action2");
                SwarmOfArrows();
                DropAction(2);
                break;
            case 3:
                Debug.Log("Action3");
                PomPomGirl();
                DropAction(3);
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
        
        anim.SetTrigger("Ambush");
        Exposed = true;
        StartCoroutine(GoToTarget(meleePosition.position, transform));
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.name == "Boss")
            {
                character.ReceiveDamage((Encouraged) ? ambushDamage * 2 : ambushDamage);
                character.CanBeHealed = false;
            }
        }

    }

    private void ApproximateShot()
    {
        anim.SetTrigger("ApproximateShot");
        if (Random.Range(0, 5) > 0)
        {
            foreach (Character character in CharacterManager.sharedInstance.characters)
            {
                if (character.name == "Boss")
                {
                    character.ReceiveDamage((Encouraged) ? approximateShotDamageOnBoss * 2 : approximateShotDamageOnBoss);
                    character.CanBeHealed = false;
                }
            }
        }
        else
        {
            //Choose a random heroes for be hitten
            Character character = CharacterManager.sharedInstance.characters[Random.Range(0, CharacterManager.sharedInstance.characters.Count)];
            while (character.gameObject.name == "Boss")
            {
                character = CharacterManager.sharedInstance.characters[Random.Range(0, CharacterManager.sharedInstance.characters.Count)];
            }

            character.ReceiveDamage((Encouraged) ? approximateShotDamageOnTeammate * 2 : approximateShotDamageOnTeammate);
        }
        
    }

    private void SwarmOfArrows()
    {
        anim.SetTrigger("SwarmOfArrows");

        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            if (character.gameObject.name == "Boss")
            {
                character.ReceiveDamage((Encouraged) ? swarmOfArrowDamageOnBoss * 2 : swarmOfArrowDamageOnBoss);
            }
            if (character.Exposed)
            {
                character.ReceiveDamage((Encouraged) ? swarmOfArrowDamageOnTeammate * 2 : swarmOfArrowDamageOnTeammate);
            }
        }

    }

    private void PomPomGirl()
    {
        anim.SetTrigger("PomPomGirl");
        //for (int i = 0; i < GameManager.sharedInstance.orderedPlayers.Count; i++)
        //{
        //    if (GameManager.sharedInstance.orderedPlayers[i].gameObject.name == "Archer")
        //    {
        //        GameManager.sharedInstance.orderedPlayers[i + 1].Encouraged = true;
        //        break;
        //    }
        //}
      
    }

}
