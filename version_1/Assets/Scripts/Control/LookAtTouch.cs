using UnityEngine;
using System.Collections;

public class LookAtTouch : MonoBehaviour
{

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    public float rotateSpeed = 50;
    public int invertPitch = 1;
    public Transform player;
	public float angleThreshold = .3f;
	private Vector2 prev = new Vector2(0f, 0f);
    

    void OnEnable()
    {
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
		EasyJoystick.On_JoystickMove += On_JoystickMove;
		EasyJoystick.On_JoystickMoveStart += On_JoystickMoveStart;
        EasyTouch.On_Swipe += On_Swipe;
    }


    void OnDisable()
    {
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
		EasyJoystick.On_JoystickMove -= On_JoystickMove;
		EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
        EasyTouch.On_Swipe -= On_Swipe;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
		EasyJoystick.On_JoystickMove -= On_JoystickMove;
		EasyJoystick.On_JoystickMoveStart -= On_JoystickMoveStart;
        EasyTouch.On_Swipe -= On_Swipe;
    }

    private Vector3 JoystickToViewPort(Vector2 joystickVector)
    {
        float x = .5f*joystickVector.x + .5f;
        float y = .5f*joystickVector.y + .5f;

        return new Vector3(x, y, 0f);
    }

	void On_JoystickMoveStart(MovingJoystick move)
	{
		Debug.Log("MOVE START" + move.joystickValue);
	}

	void On_JoystickMove(MovingJoystick move)
	{
		float angle = Mathf.Abs(Vector2.Angle(move.joystickValue, prev));
		if (angle < angleThreshold) {
			return;
		}

		Debug.Log("MOVE" + move.joystickValue + " angle=" + angle);
		prev = move.joystickValue;
		player.eulerAngles = new Vector3(move.joystickValue.y, move.joystickValue.x, 0.0f);
	}

    void On_JoystickMoveEnd(MovingJoystick move)
    {
			/*pitch -= move.;
			yaw += gesture.deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;
			
			//limit so we dont do backflips
			pitch = Mathf.Clamp(pitch, -50, 30);
			yaw = Mathf.Clamp(yaw, -70, 70);*/
			
			
			//do the rotations of our camera
			
			Debug.Log("MOVE END" + move.joystickValue);
			//prev = move.joystickValue;
			//player.eulerAngles = new Vector3(move.joystickValue.x, move.joystickValue.y, 0.0f); 
    }

    void On_Swipe(Gesture gesture)
    {
        //handle only touchpad swipes
        /*if (this.guiTexture && this.guiTexture.HitTest(gesture.position))
        {
            pitch -= gesture.deltaPosition.y * rotateSpeed * invertPitch * Time.deltaTime;
            yaw += gesture.deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;

            //limit so we dont do backflips
            pitch = Mathf.Clamp(pitch, -50, 30);
            yaw = Mathf.Clamp(yaw, -70, 70);


            //do the rotations of our camera 
            player.eulerAngles = new Vector3(pitch, yaw, 0.0f); 
        }*/

          
    }
}