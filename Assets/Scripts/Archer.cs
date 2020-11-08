using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class Archer : Character
{

    public float ambushDamage;
    public float approximateShotDamageOnBoss;
    public float approximateShotDamageOnTeammate;
    public float swarmOfArrowDamageOnBoss;
    public float swarmOfArrowDamageOnTeammate;

    public override sealed void PlayAction(int index)
    {
        Busy = true;
        switch (index)
        {
            case 0:
                Ambush();
                DropAction(0);
                break;
            case 1:
                ApproximateShot();
                DropAction(1);
                break;
            case 2:
                SwarmOfArrows();
                DropAction(2);
                break;
            case 3:
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
                }
            }
        }
        else
        {
            //Choose a random heroes for be hitten
            Character character = CharacterManager.sharedInstance.characters[Random.Range(0, CharacterManager.sharedInstance.characters.Count)];
            while (character.gameObject.name == "Boss" || character.gameObject.name == "Archer")
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
        for (int i = 0; i < OrderUI.sharedInstance.orderedPlayers.Count; i++)
        {
            if (OrderUI.sharedInstance.orderedPlayers[i].gameObject.name == "Archer")
            {
                OrderUI.sharedInstance.orderedPlayers[i + 1].Encouraged = true;
                break;
            }
        }

    }

}
