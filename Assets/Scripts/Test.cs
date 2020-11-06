using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static bool canPlay = true;
    Character character;
    
    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(OrderUI.sharedInstance.Peek().name);
            Debug.Log(canPlay);
        }

        if (character.Life < 0 || OrderUI.sharedInstance.Peek() != character || !canPlay) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            character.PlayAction(0);
            canPlay = false;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            character.PlayAction(1);
            canPlay = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            character.PlayAction(2);
            canPlay = false;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            character.PlayAction(3);
            canPlay = false;
        }
    }
}