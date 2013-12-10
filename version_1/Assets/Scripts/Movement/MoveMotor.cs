using UnityEngine;
using System.Collections;

public class MoveMotor : MonoBehaviour {

    public float speed = 0.1f;
	private float currSpeed;
    public float rotationSpeed = 90.0f;
    private bool rotating = false;
    private float angle = 0;
    private float timer = 0;
    private float nextStop;
    private float timeToStay;
    private bool stop = false;


    public GameObject sheepModel;
    public GameObject hitParticles;
	
	private bool cupturingAnimation = false;

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


        StartCoroutine(Move1());
	}

    IEnumerator Move1()
    {
        if (!stop)
        {
            sheepModel.animation.Play("sheep_walk");
            transform.Translate(0, 0, speed * Time.deltaTime);
            yield return new WaitForSeconds(Random.Range(5, 5));
        }
        else
        {
            sheepModel.animation.Stop("sheep_walk");
            stop = true;
            yield return new WaitForSeconds(Random.Range(5, 5));
            stop = false;
        }
        

        
    }

    IEnumerator Move()
    {
        if (!rotating)
        {
            sheepModel.animation.Play("sheep_walk");
            transform.Translate(0, 0, speed * Time.deltaTime);
            yield return new WaitForSeconds(Random.Range(500, 5000));
            sheepModel.animation.Stop("sheep_walk");
            yield return new WaitForSeconds(Random.Range(500, 5000));
        }
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
            if (collision.gameObject.tag == "wall")
            {
                Debug.Log("WAAAAL");
                angle = 90.0f;
            }
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
        return timer + Random.Range(500f, 1000f);
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
