﻿using UnityEngine;
using System.Collections;

public class UfoMotor : MonoBehaviour
{
	public AudioClip ufoLeaving;
	public AudioClip ufoArriving;
	public AudioClip ufoDying;
	public float ufoSpeed = 2f;
	public float volume = 1f;
	public float hitVolume = .7f;
	public AudioClip[] hittedAudio;
 	
	public float stealHeight = 1f;
	public int lives = 10;
	private TargetedSheep target;
	private bool steal = false;
	private delegate void UpdateCurrentState();
	private UpdateCurrentState updateState;
	private bool killed = false;

    public GameObject hitParticles;
    public GameObject lightBeam;

	private FragmentedObjectExploder exploder;

    public void hitted(RaycastHit hit, Ray ray)
    {
        //Debug.Log("Bullet is hit me. Lives left " + lives);
        --lives;
        if (lives == 0)
        {
            killed = true;

			AudioSource.PlayClipAtPoint(ufoDying, transform.position, volume);		

            GameObject.Find("Inventory").BroadcastMessage("incrementUfoKilled", 0);

            BeforeDestroy();
            if (exploder)
                StartCoroutine(this.exploder.Explode());
                
        } else 
		{
			//do not create a sound every time, just in 30% of cases
			if (Random.Range(0, 100) > 70) 
				AudioSource.PlayClipAtPoint(hittedAudio[Random.Range(0, hittedAudio.Length)], 
			                            transform.position, hitVolume);	
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

		audio.PlayOneShot(ufoLeaving, volume);

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

				audio.PlayOneShot(ufoArriving, volume);
				//Debug.Log("Capturing");
			}
		}
		if (distance < 0.1f)
		{
			//Debug.Log("Setting steal state");
			updateState = StealState;
			return; 
		}

		var speed = ufoSpeed + distance * distance / 800;
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
		transform.position = Vector3.Lerp(transform.position, to, Time.deltaTime * ufoSpeed * 3 / distance);
	}

    //some opereations that should be done before destroying/exploding object
    private void BeforeDestroy()
    {
        if (collider)
            collider.enabled = false;
    }
}
