using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPaladin : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.Life < 0) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            character.PlayAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            character.PlayAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            character.PlayAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            character.PlayAction(3);
        }
    }
}
