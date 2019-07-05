using UnityEngine;
using System.Collections;

///<summary> Closes the game in the Editor and Window </summary>
///<remarks> Added in line to close if user is in the Unity Editor, it didn't work without it, but you have to comment it out for the published version </remarks>
///<author> Lewis King </author>

public class QuitOnClick : MonoBehaviour
{
    /// <summary> Sets isPlaying to false, calls Application.Quit() method to close the game </summary>
	public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
