using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.Life < 0) return;
        if (Input.GetKeyDown(KeyCode.O))
        {
            character.PlayAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            character.PlayAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            character.PlayAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            character.PlayAction(3);
        }
    }
}
