using UnityEngine;
using System.Collections;

public class SheepAI : MonoBehaviour {

    public float Speed = 0.1f;
    public float MaxHeadingChange = 30;
    public float MinHeadingAngle = -180;
    public float MaxHeadingAngle = 180;
    public float MinDirectionChangeInterval = 500;
    public float MaxDirectionChangeInterval = 10000;

    private float _directionChangeInterval = 5f;
    private float _heading;
    private Vector3 _targetRotation;
    private float _rotationSpeed;
    private CharacterController _controller;

	// Use this for initialization
	void Start () {
        _heading = RandomHeading();
        _directionChangeInterval = RadomDirectionChangeInterval();
        _controller = GetComponent<CharacterController>();
	}

    void Awake()
    {
        transform.eulerAngles = new Vector3(0, _heading, 0);
        StartCoroutine(NewHeading());
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(0, 0, Speed * Time.deltaTime);
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, _targetRotation, Time.deltaTime * _directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        _controller.SimpleMove(forward * Speed);
	}

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(_directionChangeInterval);
        }
    }

    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(_heading - MaxHeadingChange, MinHeadingAngle, MaxHeadingAngle);
        var ceil = Mathf.Clamp(_heading + MaxHeadingChange, MinHeadingAngle, MaxHeadingAngle);
        _heading = Random.Range(floor, ceil);
        _targetRotation = new Vector3(0, _heading, 0);
    }

    void Rotate()
    {
        while (_heading != 0)
        {

            var sign = Mathf.Sign(_heading);
            var a = _rotationSpeed * Time.deltaTime;

            if (Mathf.Abs(_heading) - a <= 0 || sign == 0)
            {
                _heading = 0;
            }
            else
            {
                transform.RotateAround(transform.position, Vector3.up, sign * a);
                _heading -= sign * a;
            }
        }
    }

    float RandomHeading()
    {
        return Random.Range(MinHeadingAngle, MaxHeadingAngle);
    }

    float RadomDirectionChangeInterval()
    {
        return Random.Range(MinDirectionChangeInterval, MaxDirectionChangeInterval);
    }

}
