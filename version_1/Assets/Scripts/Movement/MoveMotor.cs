using UnityEngine;
using System.Collections;

public class MoveMotor : MonoBehaviour {

    public float speed = 1f;
    public float rotationSpeed = 90.0f;
    private bool rotating = false;
    private float angle = 0;
    private float timer = 0;
    private float nextStop;
    private float timeToStay;


    public GameObject sheepModel;
    public GameObject hitParticles;
	
	private bool cupturingAnimation = false;

    //private Transform contactRadius

	// Use this for initialization
	void Start () {

        if (!sheepModel)
            Debug.LogError("Sheep model not found");

        //rigidbody.centerOfMass = new Vector3(0f, 0f, 0f);
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        timeToStay = getTimeToStay();
        nextStop = getNextStop();
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<TargetedSheep>().capturing)
		{
			if (!cupturingAnimation)
			{
				cupturingAnimation = true;
				Debug.Log("Help!!! Ufo is capturing me");
                sheepModel.animation.Play("sheep_shake");
			}
			return;
		}
		
		if (cupturingAnimation)
			// Sheep was freed
            sheepModel.animation.Stop("sheep_shake");
		
        //goToWayPoint();
        move();
	}

    private void move()
    {
        checkStops();
        {
            if (speed != 0) {
                if (!rotating)
                    transform.Translate(0, 0, speed * Time.deltaTime);
                else
                {
                    var sign = Mathf.Sign(angle);
                    var a = rotationSpeed * Time.deltaTime;

                    if (Mathf.Abs(angle) - a <= 0 || sign == 0)
                    {
                        rotating = false;
                        angle = 0;
                    }
                    else
                    {
                        transform.RotateAround(transform.position, Vector3.up, sign * a);
                        angle -= sign * a;
                    }
                }
            }
        }
    }

    private void checkStops()
    {
        if (timer >= nextStop && !rotating)
        {
            speed = 0;
            sheepModel.animation.Stop("sheep_walk");
        }

        if (timer >= (nextStop + timeToStay))
        {
            speed = 1f;
            nextStop = getNextStop();
            timeToStay = getTimeToStay();
            sheepModel.animation.Play("sheep_walk");
        }

        timer++;

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
            angle = Random.Range(-90.0f, 90.0f);
            if (collision.gameObject.name.Equals("WoodFence"))
                angle = 90.0f;
            if (angle != 0)
                rotating = true;
        }
    }

    private float getTimeToStay()
    {
        return Random.Range(500f, 3000f);
    }

    private float getNextStop()
    {
        return timer + Random.Range(100f, 1000f);
    }

    public void hitted(RaycastHit hit, Ray ray)
    {
        if (hitParticles)
        {
            GameObject particles = Instantiate(hitParticles, hit.point, Quaternion.LookRotation(-ray.direction)) as GameObject;
            particles.transform.parent = transform;
        }
    }
}
