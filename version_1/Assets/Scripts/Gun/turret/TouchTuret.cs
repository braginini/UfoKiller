using UnityEngine;
using System.Collections;

public class TouchTuret : Turret
{

    void OnEnable()
    {
        EasyButton.On_ButtonPress += On_ButtonPress;
        EasyButton.On_ButtonUp += On_ButtonUp;
    }


    void OnDisable()
    {
        EasyButton.On_ButtonPress -= On_ButtonPress;
        EasyButton.On_ButtonUp -= On_ButtonUp;
    }

    void On_ButtonPress(string buttonName)
    {
        if (buttonName == "TurretButton")
        {
            base.Fire();    
        }
    }

    void On_ButtonUp(string buttonName)
    {
        if (buttonName == "Pause")
        {
            
        }
    }
}
