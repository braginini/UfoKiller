using UnityEngine;
using System.Collections;

public class LookAtJoystick2 : MonoBehaviour
{

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    public int invertPitch = 1;

    public float minX = -70;
    public float maxX = 70;
    public float minY = -50f;
    public float maxY = 30f;

    public Transform mouseLocationIndicator;

    public float RotationSensivity = 1;

    public Vector2 OLdJoystickValue = new Vector2(.5f, .5f);
    
    void OnEnable()
    {
        EasyJoystick.On_JoystickMove += On_JoystickMove;
    }

    void Update()
    {

        
    }


    void OnDisable()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
    }

    void OnDestroy()
    {
        EasyJoystick.On_JoystickMove -= On_JoystickMove;
    }

    void On_JoystickMove(MovingJoystick move)
    {

        Vector2 delta = move.joystickValue - OLdJoystickValue;

        OLdJoystickValue = move.joystickValue;

        pitch -= delta.y * RotationSensivity;
        yaw += delta.x * RotationSensivity;

        //limit so we dont do backflips
        pitch = Mathf.Clamp(pitch, minY, maxY);
        yaw = Mathf.Clamp(yaw, minX, maxX);

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        //if (mouseLocationIndicator && camera)
        //{
            RaycastHit hit;

            LayerMask mask = 1 << 10;
            Ray ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, mask))
            {
                Debug.Log(hit.point + " " + hit.collider.gameObject.name);

                if (mouseLocationIndicator)
                    mouseLocationIndicator.position = camera.WorldToViewportPoint(hit.point);
            }
        //}

    }
}

