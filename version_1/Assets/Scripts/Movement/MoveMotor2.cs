using UnityEngine;
using System.Collections;

public class MoveMotor2 : MonoBehaviour
{

    CharacterController _controller;

    public float speed = 1f;
    public float rotationSpeed = 90.0f;
    public float minTime = 1f;
    private bool rotating = false;
    private float gravity = 20f;
    private float velocity;
    private Vector3 moveDirection;
    private Quaternion newRotation;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        rigidbody.centerOfMass = new Vector3(0f, 0f, 0f);
        
    }

    void Update()
    {
        move();
    }

    private void move()
    {
            moveDirection = getMoveDirection();
            _controller.Move(moveDirection * Time.deltaTime);  
    }

    private Vector3 getMoveDirection()
    {
        Vector3 moveDirection = transform.right;
        moveDirection *= speed;
        moveDirection.y -= gravity;
        return moveDirection;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag != "terrain")
        {
            if (rotating)
            {
                Debug.Log("0");
                if (isRotationCompleted())
                {
                    Debug.Log("1");
                    rotating = false;
                }
                else
                    return;
            }

            Debug.Log("2");
            rotating = true;
            Vector3 angles = transform.eulerAngles;            
            newRotation = Quaternion.Euler(angles.x,
                Mathf.SmoothDampAngle(angles.y, Random.Range(-90f, 90f), ref velocity, minTime, rotationSpeed),
                angles.z);

            transform.rotation = newRotation;
            
        }
    }    

    bool isRotationCompleted()
    {
        float angle = (newRotation.eulerAngles - transform.rotation.eulerAngles).sqrMagnitude;
        Debug.Log(">>>>" + angle);
        return angle <= 0.1f;
    }

    void OnCollisionEnter(Collision collision)
    {
        
        processColiision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        processColiision(collision);
    }

    private bool checkValidCollistions(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.tag != "terrain")
            {
                Debug.DrawRay(contact.point, contact.normal, Color.red);
                return true;
            }
        }

        return false;
    }

    private void processColiision(Collision collision)
    {
        if (rotating)
            return;

        if (checkValidCollistions(collision))
        {            
            rotating = true;
            //moveDirection.x = Random.Range(-180, 180);
            moveDirection = new Vector3(Random.Range(0, 300), moveDirection.y, Random.Range(0, 300));
        }
    }

}
