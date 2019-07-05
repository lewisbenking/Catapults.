using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

    private Vector3 offset;
    private Text toolTipText;
    private Image image;

    void Start()
    {
        toolTipText = transform.Find("TooltipText").GetComponent<Text>();
        image = GetComponent<Image>();
        image.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.0f, true);
        toolTipText.CrossFadeAlpha(0.0f, 0.0f, true);
    }
	// Update is called once per frame
	void Update () {
	}

    public void setToolTipText(string text)
    {
        toolTipText = transform.Find("TooltipText").GetComponent<Text>();
        toolTipText.text = text;
    }

    public void fadeOut()
    {
        image.GetComponent<Image>().CrossFadeAlpha(0.0f, 0.1f, true);
        toolTipText.CrossFadeAlpha(0.0f, 0.1f, true);
    }

    public void fadeIn()
    {
        image.GetComponent<Image>().CrossFadeAlpha(0.8f, 0.1f, true);
        toolTipText.CrossFadeAlpha(0.8f, 0.1f, true);
    }
}
