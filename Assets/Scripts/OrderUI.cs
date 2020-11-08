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
    private Transform marker;
    [SerializeField]
    private List<Transform> characterIcons;
    public List<Character> orderedPlayers = new List<Character>();
    [SerializeField]
    private float offsetYMarker = -0.12f;
    [SerializeField]
    private float startXMarker = 2.9f;
    [SerializeField]
    private float offsetY = 0.12f;
    [SerializeField]
    private float startX = 3;
    [SerializeField]
    private float stepX = 1;
    [SerializeField]
    public int index = 0;

    private void Start()
    {
        marker.localPosition = new Vector3(startXMarker, offsetYMarker, 0);
        SortOrder();
    }

    public Character Peek()
    {
        return orderedPlayers[index];
    }

    public void Next()
    {
        if (index < orderedPlayers.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
            Reset();
        }
        marker.localPosition = new Vector3(startXMarker - stepX * index, offsetYMarker, 0);
        if (orderedPlayers[index].Life < 0)
        {
            Next();
        }
    }

    public void SortOrder()
    {
        RandomizePositions();
        foreach (Transform characterIcon in characterIcons)
        {
            characterIcon.gameObject.SetActive(false);
        }
        for (int i = 0; i < orderedPlayers.Count; i++)
        {
            PlaceIcon(orderedPlayers[i].gameObject.name, i);
        }
    }

    public void PlaceAllIcons()
    {
        for (int i = 0; i < orderedPlayers.Count; i++)
        {
            PlaceIcon(orderedPlayers[i].gameObject.name, i);
        }
    }

    private void PlaceIcon(string name, int index)
    {
        foreach (Transform characterIcon in characterIcons)
        {
            if (characterIcon.gameObject.name == name)
            {
                characterIcon.gameObject.SetActive(true);
                characterIcon.localPosition = new Vector3(startX - stepX * index, offsetY, 0);
                return;
            }
        }
    }
    private void RandomizePositions()
    {
        orderedPlayers.Clear();

        while (orderedPlayers.Count < CharacterManager.sharedInstance.characters.Count)
        {
            int randomIndex = Random.Range(0, CharacterManager.sharedInstance.characters.Count);

            while (orderedPlayers.Contains(CharacterManager.sharedInstance.characters[randomIndex]))
            {
                randomIndex = Random.Range(0, CharacterManager.sharedInstance.characters.Count);
            }

            orderedPlayers.Add(CharacterManager.sharedInstance.characters[randomIndex]);
        }
    }

    private void Reset()
    {
        Debug.Log("Reset");
        foreach (Character character in CharacterManager.sharedInstance.characters)
        {
            character.Reset();
        }
        SortOrder();
    }

    public int TurnLeft()
    {
        return orderedPlayers.Count - index;
    }
}
