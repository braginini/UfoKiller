using UnityEngine;
using System.Collections;

public class MoveMotor : MonoBehaviour {

    public float speed = 0.1f;
	private float currSpeed;
    public float rotationSpeed = 90.0f;
    public bool rotating = false;
    private float angle = 0;
    private float timer = 0;
    private float nextStop;
    private float timeToStay;


    public GameObject sheepModel;
    public GameObject hitParticles;
	
	private bool cupturingAnimation = false;
	
	Quaternion newRotation;

    //private Transform contactRadius

	// Use this for initialization
	void Start () {
		this.currSpeed = speed;

        if (!sheepModel)
            Debug.LogError("Sheep model not found");

        //rigidbody.centerOfMass = new Vector3(0f, 0f, 0f);
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        timeToStay = getTimeToStay();
        nextStop = getNextStop();
		
		newRotation = Quaternion.AngleAxis(-90f, Vector3.up);
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<TargetedSheep>().capturing)
		{
			if (!cupturingAnimation)
			{
				cupturingAnimation = true;
				Debug.Log("Help!!! Ufo is capturing me");
				playAnimation("sheep_shake");
			}
			return;
		}
		
		if (cupturingAnimation)
			// Sheep was freed
            stopAnimation("sheep_shake");
		
        //goToWayPoint();
        StartCoroutine(move());
	}

    private IEnumerator move()
    {
		checkCollisionsInFront();
		
		transform.Translate(0, 0, currSpeed * Time.deltaTime);
		
		if (rotating) 
		{
			var sign = Mathf.Sign(angle);
            var a = rotationSpeed * Time.deltaTime;

            if (Mathf.Abs(angle) - a <= 0 || sign == 0)
            {				
				yield return new WaitForSeconds(Random.Range(2, 10)); 
				rotating = false;  
				angle = 0;
				
            } else			
            {
            	transform.RotateAround(transform.position, Vector3.up, sign * a);
                angle -= sign * a;
            }	
		}
			
    }
	
	private void checkCollisionsInFront() 
	{
		Vector3 front = transform.TransformDirection(0f, .1f, .2f);
		RaycastHit hit;
		
		Debug.DrawRay(transform.position, front, Color.green);
		if (Physics.Raycast(transform.position, front, out hit) && !hit.collider.gameObject.tag.Equals("terrain")) 
		{
			
			if (!rotating) 
			{
				rotating = true;
				if (hit.collider.gameObject.tag.Equals("wall"))
					angle = 180f;
				else
					angle = Random.Range(-90, 90);
			}
    		
		}
	}

    private void checkStops()
    {
        if (timer >= nextStop && !rotating)
        {
            currSpeed = 0;
           	stopAnimation("sheep_walk");
        }

        if (timer >= (nextStop + timeToStay))
        {
            currSpeed = speed;
            nextStop = getNextStop();
            timeToStay = getTimeToStay();
            playAnimation("sheep_walk");
        }

        timer++;

    }

    void OnCollisionEnter(Collision collision)
    {
       // processCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        //processCollision(collision);
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

    private void processCollision(Collision collision)
    {      
		if (rotating)
            return;

        if (checkValidCollistions(collision))
        {
            angle = Random.Range(-90.0f, 90.0f);
            if (collision.gameObject.tag.Equals("wall"))
                angle = 90.0f;
            if (angle != 0)
                rotating = true;
        }
    }

    private float getTimeToStay()
    {
        return Random.Range(1000f, 2000f);
    }

    private float getNextStop()
    {
        return timer + Random.Range(1000000f, 500000000f);
    }

    public void hitted(RaycastHit hit, Ray ray)
    {
        if (hitParticles)
        {
            GameObject particles = Instantiate(hitParticles, hit.point, Quaternion.LookRotation(-ray.direction)) as GameObject;
            particles.transform.parent = transform;
        }
    }
	
	private void playAnimation(string name) 
	{
		if (sheepModel) 
		{
			sheepModel.animation.Stop();
			sheepModel.animation.Play(name);
		}
	}
	
	private void stopAnimation(string name) 
	{
		if (sheepModel) 
			sheepModel.animation.Stop(name);
	}
}
