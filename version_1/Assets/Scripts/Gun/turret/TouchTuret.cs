using UnityEngine;
using System.Collections;

public class TouchTuret : Turret
{
	public float angleThreshold = .3f;
	public string fireButtonName = "Turret Button";
	private Vector2 prevJoysticValue = new Vector2(0f, 0f);
	public float clampXMin = -60f;
	public float clampXMax = 30f;
	public float clampYMin = -70f;
	public float clampYMax = 70f;

    void OnEnable()
    {
        EasyButton.On_ButtonPress += On_ButtonPress;
        EasyButton.On_ButtonUp += On_ButtonUp;
		EasyJoystick.On_JoystickMove += On_JoystickMove;
    }


    void OnDisable()
    {
        EasyButton.On_ButtonPress -= On_ButtonPress;
        EasyButton.On_ButtonUp -= On_ButtonUp;
		EasyJoystick.On_JoystickMove -= On_JoystickMove;
    }

	void OnDestroy()
	{
		EasyJoystick.On_JoystickMoveEnd -= On_JoystickMove;
		EasyButton.On_ButtonPress -= On_ButtonPress;
		EasyButton.On_ButtonUp -= On_ButtonUp;
	}

    void On_ButtonPress(string buttonName)
    {
		if (buttonName == fireButtonName)
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

	/**
	 * handles EasyTouch joystic move event
	**/
	void On_JoystickMove(MovingJoystick move)
	{
		float angle = Mathf.Abs(Vector2.Angle(move.joystickValue, prevJoysticValue));
		if (angle < angleThreshold) {
			return;
		}
		
		Debug.Log("MOVE" + move.joystickValue + " angle=" + angle);
		prevJoysticValue = move.joystickValue;

		transform.eulerAngles = new Vector3(Mathf.Clamp(move.joystickValue.y, clampXMin, clampXMax), 
		                                    Mathf.Clamp(move.joystickValue.x, clampYMin, clampYMax), 
		                                    0.0f);
	}
}
