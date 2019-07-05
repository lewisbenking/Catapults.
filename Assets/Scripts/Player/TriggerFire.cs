using UnityEngine;
using System.Collections;

///<summary> Function which triggers fire sequence in the Character Controller </summary>
///<remarks> Used by fire.anim at apex of fire animation </remarks>
///<remarks> Attatched to Player GameObject </remarks>
///<author> Charlie Postgate </author>

public class TriggerFire : MonoBehaviour {

    ///<summary> Calls triggerFire method in CharacterController </summary>
    void triggerFire()
    {
        gameObject.transform.parent.GetComponent<CharacterController>().triggerFire();
    }
}
