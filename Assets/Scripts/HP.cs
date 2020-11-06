using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class HP : MonoBehaviour
{
    private float xPositionWhenFull;

    private void Start()
    {
        xPositionWhenFull = transform.localPosition.x;
    }

    public void CalculatePosition(float actualLife, float maxLife)
    {
        transform.localPosition = new Vector3(Mathf.Lerp(0, xPositionWhenFull, actualLife / maxLife),
                                       transform.localPosition.y,
                                       transform.localPosition.z);
    }
}
