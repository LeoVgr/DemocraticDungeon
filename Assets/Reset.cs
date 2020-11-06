using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach(Character character in CharacterManager.sharedInstance.characters)
            {
                character.Reset();
            }
            OrderUI.sharedInstance.SortOrder();
        }
    }
}
