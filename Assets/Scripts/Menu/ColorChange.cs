using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///<summary> User clicks on texture, gets pixel colour, updates team text colour </summary>
///<author> Lewis King </author>

public class ColorChange : MonoBehaviour
{
    // Variable declaration / some initialised here
    //private Rect textureRect1 = new Rect(300, 285, 175, 175);
    //private Rect textureRect2 = new Rect(575, 285, 175, 175);
    //private Rect textureRect1 = new Rect(Screen.width / 3.75f, Screen.width / 2.75f, Screen.width / 5f, Screen.height / 5f);
    //private Rect textureRect2 = new Rect(575, 285, 175, 175);
    private Text text;
    private Image image;
    private bool entered = false;
    private float height = Screen.height;
    private float width = Screen.width;

    public Texture2D colourTexture;
    public int team;


    Rect ResizeGUI(Rect _rect)
    {
        float FilScreenWidth = _rect.width / 800;
        float rectWidth = FilScreenWidth * width;
        float FilScreenHeight = _rect.height / 600;
        float rectHeight = FilScreenHeight * height;
        float rectX = (_rect.x / 800) * width;
        float rectY = (_rect.y / 600) * height;

        return new Rect(rectX, rectY, rectWidth, rectHeight);
    }

    void OnMouseEnter()
    {
        entered = true;
    }

    void OnMouseExit()
    {
        entered = false;
    }

    /// <summary> Draws textures, calls method to update colour, passes over colour/team </summary>
    void OnGUI()
    {
        // Draw textures
        //GUI.DrawTexture(textureRect1, colourTexture);
        float aspectRatio = Camera.main.aspect;
        float yPos = 0.0f;
        // Get aspect ratio value for the rest in the unity editor
        if (aspectRatio == 1.777778f) //16:9
        {
            yPos = 1.685f;
        }
        else if (aspectRatio == 1.5f) //3:2
        {

        }
        else if (aspectRatio == 1.3333f) //4:3
        {

        }
        else if (aspectRatio == 1.6f) //16:10
        {

        }
        else if (aspectRatio == 1.25f) //5:4
        {

        }
      
        Rect textureRect1 = new Rect(Screen.width / 4.35f, Screen.height / 1.685f, Screen.width / 5f, Screen.height / 4.5f);
        Rect textureRect2 = new Rect(Screen.width / 1.725f, Screen.height / 1.685f, Screen.width / 5f, Screen.height / 4.5f);
        GUI.DrawTexture(textureRect1, colourTexture);
        GUI.DrawTexture(textureRect2, colourTexture);//, ScaleMode.ScaleToFit);

        // If mouse was clicked
        if(EventSystem.current.IsPointerOverGameObject())
        {
            // Set position to current mouse position
            Vector2 mousePosition = Event.current.mousePosition;
            if (team == 1)
            {
                // if outside of the texture boundaries
                if ((mousePosition.x > textureRect1.xMax) || (mousePosition.x < textureRect1.x)
                    || (mousePosition.y > textureRect1.yMax) || (mousePosition.y < textureRect1.y))
                {
                    return;
                }

                // gets the u/v co-ordinates
                float textureUPosition = (mousePosition.x - textureRect1.x) / textureRect1.width;
                float textureVPosition = 1.0f - ((mousePosition.y - textureRect1.y) / textureRect1.height);

                // calls getPixelBilinear to retrieve the colour, calls NewColour(...) method
                Color textureColour = colourTexture.GetPixelBilinear(textureUPosition, textureVPosition);
                NewColour(1, textureColour, 1);
            }
            else if (team == 2)
            {
                if ((mousePosition.x > textureRect2.xMax) || (mousePosition.x < textureRect2.x)
                    || (mousePosition.y > textureRect2.yMax) || (mousePosition.y < textureRect2.y))
                {
                    return;
                }

                float textureUPosition = (mousePosition.x - textureRect2.x) / textureRect2.width;
                float textureVPosition = 1.0f - ((mousePosition.y - textureRect2.y) / textureRect2.height);

                Color textureColour = colourTexture.GetPixelBilinear(textureUPosition, textureVPosition);
                NewColour(2, textureColour, 1);
            }
        }
        else
        {
            ResetColour();
        }

        if (Event.current.type == EventType.MouseDown)
        {
            // Set position to current mouse position
            Vector2 mousePosition = Event.current.mousePosition;
            if (team == 1)
            {
                // if outside of the texture boundaries
                if ((mousePosition.x > textureRect1.xMax) || (mousePosition.x < textureRect1.x) 
                    || (mousePosition.y > textureRect1.yMax) || (mousePosition.y < textureRect1.y))
                {
                    return;
                }

                // gets the u/v co-ordinates
                float textureUPosition = (mousePosition.x - textureRect1.x) / textureRect1.width;
                float textureVPosition = 1.0f - ((mousePosition.y - textureRect1.y) / textureRect1.height);

                // calls getPixelBilinear to retrieve the colour, calls NewColour(...) method
                Color textureColour = colourTexture.GetPixelBilinear(textureUPosition, textureVPosition);
                NewColour(1, textureColour, 0);

                print(Camera.main.aspect);
                print(Camera.main.pixelWidth);
                print(Camera.main.pixelHeight);
            }
            else if (team == 2)
            {
                if ((mousePosition.x > textureRect2.xMax) || (mousePosition.x < textureRect2.x) 
                    || (mousePosition.y > textureRect2.yMax) || (mousePosition.y < textureRect2.y))
                {
                    return;
                }

                float textureUPosition = (mousePosition.x - textureRect2.x) / textureRect2.width;
                float textureVPosition = 1.0f - ((mousePosition.y - textureRect2.y) / textureRect2.height);
                
                Color textureColour = colourTexture.GetPixelBilinear(textureUPosition, textureVPosition);
                NewColour(2, textureColour, 0);
                //print(textureColour);                
            }
        }
    }

    /// <summary>changes team text colour</summary>
    /// <param name="team">team number</param>
    /// <param name="color">colour to change the text to</param>
    void NewColour(int team, Color color, int item)
    {
        if (item == 0)
        {
            text = GameObject.Find("Player" + team + "Text").GetComponent<Text>();
            color.a = 1;
            text.color = color;
        }
        else if (item == 1)
        {
            image = GameObject.Find("ColourPreview" + team).GetComponent<Image>();
            color.a = 1;
            image.color = color;
        }
    }

    void ResetColour()
    {
        image = GameObject.Find("ColourPreview" + team).GetComponent<Image>();
        text = GameObject.Find("Player" + team + "Text").GetComponent<Text>();
        image.color = text.color;
    }
}