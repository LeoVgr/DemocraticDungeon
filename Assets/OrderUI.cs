using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{

    private static OrderUI instance = null;

    public static OrderUI sharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<OrderUI>();
            }
            return instance;
        }
    }

    [SerializeField]
    private List<Transform> characterIcons;

    [SerializeField]
    private float offsetY = 0.12f;
    [SerializeField]
    private float startX = 3;
    [SerializeField]
    private float stepX = 1;

    public void SortOrder()
    {
        List<Character> orderedList = GameManager.sharedInstance.orderedPlayers;
        foreach(Transform characterIcon in characterIcons)
        {
            characterIcon.gameObject.SetActive(false);
        }
        for (int i = 0; i < orderedList.Count; i++)
        {
           // PlaceIcon(orderedList[i].Team, i);
        }
    }

    public void PlaceIcon(string name, int index)
    {
        foreach (Transform characterIcon in characterIcons)
        {
            if(characterIcon.gameObject.name == name)
            {
                characterIcon.gameObject.SetActive(true);
                transform.localPosition = new Vector3(startX + stepX * index, offsetY, 0);
                return;
            }
        }
    }
}
