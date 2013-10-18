using UnityEngine;
using System.Collections;

/**
 * Class used as to control turret-like weapons 
 * 
**/
public class LookAtMouse : LookAtBase {

    public override void UpdateControlRay()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
    }
}
