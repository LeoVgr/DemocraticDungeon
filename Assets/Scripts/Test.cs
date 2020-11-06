using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            character.ReceiveDamage(500);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            character.PlayAction(0);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            character.PlayAction(2);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            character.PlayAction(3);
        }
    }
}
