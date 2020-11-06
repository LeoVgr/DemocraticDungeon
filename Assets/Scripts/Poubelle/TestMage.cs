using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMage : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.Life < 0) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            character.PlayAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            character.PlayAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            character.PlayAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            character.PlayAction(3);
        }
    }
}
