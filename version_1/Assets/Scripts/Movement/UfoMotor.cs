using UnityEngine;
using System.Collections;

public class UfoMotor : MonoBehaviour
{
	public AudioClip ufoLeaving;
	public AudioClip ufoArriving;
	public AudioClip ufoDying;
	public AudioClip[] hittedAudio;
 	
	public float stealHeight = 1f;
	public int lives = 3;
	private TargetedSheep target;
	private bool steal = false;
	private delegate void UpdateCurrentState();
	private UpdateCurrentState updateState;
	private bool killed = false;

    public GameObject hitParticles;
    public GameObject lightBeam;

    private FragmentedObjectExploder exploder;
	
	public void hitted()
	{
		Debug.Log("Bullet is hit me. Lives left " + lives);
		--lives;
		if (lives == 0)
		{
			killed = true;
			
			var explosion = gameObject.GetComponentInChildren<ParticleSystem>();
			if (explosion)
			{
				Debug.Log("Expolosion is running");
        		explosion.Play();
			}
			else
				Debug.LogWarning("Expolosion is not found");
			
			audio.PlayOneShot(ufoDying);
			GameObject.Find("Inventory").BroadcastMessage("incrementUfoKilled", 0);

            BeforeDestroy();
            if (exploder)
		        StartCoroutine(this.exploder.Explode());

		}
		else if (lives > 0 && !this.audio.isPlaying) {

            Debug.Log("");
			this.audio.clip = hittedAudio[Random.Range(0, hittedAudio.Length)];
		    this.audio.volume = 1f;
			this.audio.Play();
		}
	}

    public void hitted(RaycastHit hit, Ray ray)
    {
        Debug.Log("Bullet is hit me. Lives left " + lives);
        --lives;
        if (lives == 0)
        {
            killed = true;

            audio.PlayOneShot(ufoDying);

            GameObject.Find("Inventory").BroadcastMessage("incrementUfoKilled", 0);

            BeforeDestroy();
            if (exploder)
                StartCoroutine(this.exploder.Explode());
                
        }
        else if (lives > 0 && !this.audio.isPlaying)
        {
            this.audio.clip = hittedAudio[Random.Range(0, hittedAudio.Length)];
            this.audio.volume = 1f;
            this.audio.Play();
        }

        if (hitParticles)
        {
            GameObject particles = Instantiate(hitParticles, hit.point, Quaternion.LookRotation(-ray.direction)) as GameObject;
            //particles.transform.parent = transform;
        }
    }

	void Start()
	{
		updateState = WithoutTargetState;
		//this.animation.Play("Swing");

	    this.exploder = GetComponent<FragmentedObjectExploder>();
        if (!exploder)
            Debug.LogWarning("Object Exploder is not attached");

        if (!lightBeam)
            Debug.LogWarning("Object LightBeam is not attached");
	}
	
	void Update()
	{
	 	if (!killed && updateState != null)
			updateState();
	}
	
	void OnDisable()
	{
		if (target)
		{
			//Debug.Log("Freeing target");
			target.capturing = false;
			target.targeted = false;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (killed) // Ignore all bullets if ufo was aleady killed but not dissapeared yet
			// (or it could count one ufo as several killed)
			return;

		if (other.gameObject.tag == "bullet")
		{
			Destroy(other.gameObject);
			hitted();
		}
    }
	
	private void findTarget()
	{
		var sheep = FindObjectsOfType(typeof(TargetedSheep)) as TargetedSheep[];
		foreach (var s in sheep)
		{
			if (!s.targeted)
			{
				s.targeted = true;
				target = s;
				break;
			}
		}
		//Debug.Log("Target " + (target ? "" : "not ") + "found");
	}
	
	private IEnumerator Steal()
	{
		//Debug.Log("Arrived to height " + transform.position.y);
        if (lightBeam)
		    lightBeam.animation.Play("Open");

		yield return new WaitForSeconds(10f);
		Destroy(target.gameObject);
		GameObject.Find("Inventory").BroadcastMessage("decrementSheep", 0);

		if (lightBeam)
		    lightBeam.animation.Play("Close");

		 audio.PlayOneShot(ufoLeaving, 0.3f);

		yield return new WaitForSeconds(2f);

        if (lightBeam)
            Destroy(lightBeam);

		//Debug.Log("Switching to EscapeState");
		updateState = EscapeState;
	}
	
	/// <summary>
	/// In this state ufo does not have any target, so it tryes to find any free target.
	/// </summary>
	private void WithoutTargetState()
	{
		if (target)
		{
			Debug.LogError("In state WithoutTargetState but target is not null");
			updateState = GoToTargetState;
			return;
		}

		// Could happen if there were initially more ufos then sheep. Trying to capture new sheep because
		// some ufos could have been killed
		findTarget();		
		if (target)
		{
			Debug.Log("Switching to GoToTargetState");
			updateState = GoToTargetState;
		}
		else
		{
			//TODO: do some random movements while waiting free target
			return;
		}
	}
	
	/// <summary>
	/// In this state ufo is moving toward targeted sheep.
	/// </summary>
	private void GoToTargetState()
	{
		var to = target.transform.position + stealHeight * Vector3.up;
		var distance = Vector3.Distance(transform.position, to);
		if (distance < 4f)
		{
			// Starting capturing a little bit earlier to stop the sheep and arrive to correct position
			if (!target.capturing)
			{
				target.capturing = true;

				audio.PlayOneShot(ufoArriving, 0.3f);
				//Debug.Log("Capturing");
			}
		}
		if (distance < 0.1f)
		{
			//Debug.Log("Setting steal state");
			updateState = StealState;
			return; 
		}

		var speed = 5f + distance * distance / 500;
		var nextPosition = Vector3.Lerp(transform.position, to, Time.deltaTime * speed / distance);
		
		// This cycle here is for handling situations when speed of the ufo is too high to get to the capturing area
		// and ufo passes this area without stopping in it
		while (distance < Vector3.Distance(transform.position, nextPosition) && speed > 0.001)
		{
			var newSpeed = speed / 2f;
			Debug.LogWarning("Reducing speed from " + speed + " to " + newSpeed);
			speed = newSpeed;
			nextPosition = Vector3.Lerp(transform.position, to, Time.deltaTime * speed / distance);
		}
		
		//Debug.Log("Speed " + speed + ", height " + transform.position.y);
    	transform.position = nextPosition;
	}
	
	/// <summary>
	/// In this state ufo is stealing sheep
	/// </summary>
	private void StealState()
	{
		if (!steal)
		{
			steal = true;
			transform.rotation = new Quaternion(0, 0, 0, 0);
			target.audio.volume = 0.5f;
			target.audio.Play();			
			StartCoroutine(Steal());
		}
	}
	
	/// <summary>
	/// In this state ufo escapes with stolen sheep
	/// </summary>
	private void EscapeState()
	{
		var escapeHeight = 500f;
		
		if (transform.position.y >= escapeHeight)
		{
			Destroy(transform.root.gameObject);
			return;
		}
		
		var to = new Vector3(0, 2f * escapeHeight, 0); // Somewhere very high
		var distance = Vector3.Distance(transform.position, to);
		var speed = 50f;
		transform.position = Vector3.Lerp(transform.position, to, Time.deltaTime * speed / distance);
	}

    //some opereations that should be done before destroying/exploding object
    private void BeforeDestroy()
    {
        if (collider)
            collider.enabled = false;
    }
}
