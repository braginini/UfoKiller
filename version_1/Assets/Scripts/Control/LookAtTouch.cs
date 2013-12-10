using UnityEngine;
using System.Collections;

public class LookAtTouch : MonoBehaviour
{

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    public float rotateSpeed = 50;
    public int invertPitch = 1;
    public Transform player;
    

    void OnEnable()
    {
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
        EasyTouch.On_Swipe += On_Swipe;
    }


    void OnDisable()
    {
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        EasyTouch.On_Swipe -= On_Swipe;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        EasyTouch.On_Swipe -= On_Swipe;
    }

    private Vector3 JoystickToViewPort(Vector2 joystickVector)
    {
        float x = .5f*joystickVector.x + .5f;
        float y = .5f*joystickVector.y + .5f;

        return new Vector3(x, y, 0f);
    }

    void On_JoystickMoveEnd(MovingJoystick move)
    {

    }

    void On_Swipe(Gesture gesture)
    {
        //handle only touchpad swipes
        if (this.guiTexture && this.guiTexture.HitTest(gesture.position))
        {
            pitch -= gesture.deltaPosition.y * rotateSpeed * invertPitch * Time.deltaTime;
            yaw += gesture.deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;

            //limit so we dont do backflips
            pitch = Mathf.Clamp(pitch, -50, 30);
            yaw = Mathf.Clamp(yaw, -70, 70);


            //do the rotations of our camera 
            player.eulerAngles = new Vector3(pitch, yaw, 0.0f); 
        }

          
    }
}