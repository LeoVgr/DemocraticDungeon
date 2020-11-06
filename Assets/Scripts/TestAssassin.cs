using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAssassin : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.Life < 0) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            character.PlayAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            character.PlayAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            character.PlayAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            character.PlayAction(3);
        }
    }
}
