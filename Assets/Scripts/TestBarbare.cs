using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBarbare : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.Life < 0) return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            character.PlayAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            character.PlayAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            character.PlayAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            character.PlayAction(3);
        }
    }
}
