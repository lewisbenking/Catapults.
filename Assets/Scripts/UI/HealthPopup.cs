using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPopup : MonoBehaviour {

    private int lifeTime = 150; // 100 physics ticks
    private int currentLife = 0;
    private bool initiated;

    private RectTransform canvasRectTransform;
    private Text displayText;

    // Use this for initialization
    void Start()
    {
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        displayText = GetComponent<Text>();
    }

    // Update is called once per physics tick
    void FixedUpdate()
    {
        //destroy itself after 100 physics ticks
        if (currentLife >= lifeTime)
        {
            Destroy(gameObject);
        }

        //drift the popup up & to the right
        canvasRectTransform.localPosition = new Vector3(canvasRectTransform.localPosition.x + 0.007f, canvasRectTransform.localPosition.y + 0.01f, 0);
        displayText.CrossFadeAlpha(0f, 1f, false);

        //increment the popup's life
        currentLife++;
    }

    //give the popup it's appropriate text properties
    public void initiatePopup(Color color, string displayVal)
    {
        Start();
        displayText.text = displayVal;
        displayText.color = color;
    }
}
