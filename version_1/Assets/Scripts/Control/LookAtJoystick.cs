using UnityEngine;
using System.Collections;

public class LookAtJoystick : LookAtBase
{

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    public float rotateSpeed = 50;
    public int invertPitch = 1;
    public Transform player;

    public Vector3 JoystickPos = new Vector3(.5f, .5f, 0f);
    
    public override void UpdateControlRay()
    {
        ray = camera.ViewportPointToRay(JoystickPos);
    }

    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
        EasyTouch.On_Swipe += On_Swipe;
    }


    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        EasyTouch.On_Swipe -= On_Swipe;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
        EasyTouch.On_Swipe -= On_Swipe;
    }

    void On_JoystickMove(MovingJoystick move)
    {
        Vector3 newPos = JoystickToViewPort(move.joystickValue);

        float deltaX = Mathf.Abs(newPos.x - JoystickPos.x);
        float deltaY = Mathf.Abs(newPos.y - JoystickPos.y);

        if (deltaX > 0.008)
            JoystickPos.x = newPos.x;
        if (deltaY > 0.008)
            JoystickPos.y = newPos.y;
    }

    private Vector3 JoystickToViewPort(Vector2 joystickVector)
    {
        float x = .5f*joystickVector.x + .5f;
        float y = .5f*joystickVector.y + .5f;

        Debug.Log("x=" + x + " y=" + y);

        return new Vector3(x, y, 0f);
    }

    void On_JoystickMoveEnd(MovingJoystick move)
    {

    }

    void On_Swipe(Gesture gesture)
    {
        /*Debug.Log(gesture.GetSwipeOrDragAngle());
        pitch -= gesture.deltaPosition.y * rotateSpeed * invertPitch * Time.deltaTime;
        yaw += gesture.deltaPosition.x * rotateSpeed * invertPitch * Time.deltaTime;

        //limit so we dont do backflips
        pitch = Mathf.Clamp(pitch, -50, 30);
        yaw = Mathf.Clamp(yaw, -70, 70);


        //do the rotations of our camera 
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f); */  
    }
}
