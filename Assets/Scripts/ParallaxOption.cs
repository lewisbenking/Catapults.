using System.Collections;
using UnityEngine;

/// <summary>Save and restore the positions</summary>
/// <author>Marks Paskannijs</author>
public class ParallaxOption : MonoBehaviour {

    public bool moveParallax;

    [SerializeField]
    [HideInInspector]
    private Vector3 storedPosition;

    public void SavePosition()
    {
        storedPosition = transform.position;
    }

    public void RestorePosition()
    {
        transform.position = storedPosition;
    }
}
