using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {
	
	private AudioSource shootAudio;
	
	private AudioSource stopShootAudio;
	
	private float lastShootTime = 0f;
	
	private float lastShootAnimTime = 0f;
	
	private GameObject lBarrel;
	
	private GameObject rBarrel;
	
	private GameObject crossHair;
	
	public float roationSpeed = 1f;
	
	public Camera camera;
	
	public Rigidbody bulletPrefab;
	
	public GameObject[] bulletSpawns;
	
	void Start () {	
		
		AudioSource[] aSources = GetComponents<AudioSource>();
		
		if (aSources.Length < 2) {
			print("no audio sorce attached");
		}
			
    	shootAudio = aSources[0];
		stopShootAudio = aSources[1];
		
		rBarrel = GameObject.FindWithTag("rBarrel");
		lBarrel = GameObject.FindWithTag("lBarrel");
		
		crossHair = GameObject.FindWithTag("crosshair");
		
		bulletSpawns = GameObject.FindGameObjectsWithTag("bullet_spawn");	
	}
	
	void Update () {
		
		//crossHair.transform.position = Input.mousePosition;
		followCrosshair();	
		
						
	}
	
	private void followCrosshair() {
		
		Ray ray = camera.ViewportPointToRay(crossHair.transform.position);
		
		Vector3 dir;
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
			
			dir = hit.point - transform.position;
			transform.LookAt(hit.point);
			} else {
				dir = ray.direction;
		}
		
		
		Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, 
			Quaternion.LookRotation(dir, Vector3.up), 
			roationSpeed * Time.deltaTime);
		
		transform.rotation = newRotation;
		
		foreach (GameObject g in bulletSpawns) { 		
			g.transform.rotation = newRotation;			
		}
	}
	
	public void shoot() {
		
		launchBullets();
		
		Ray ray = camera.ViewportPointToRay(crossHair.transform.position);
		RaycastHit hit;	
		
		if (Physics.Raycast(ray, out hit , Mathf.Infinity) && hit.collider.gameObject.tag.Equals("ufo")) {			
            hit.collider.gameObject.GetComponent<UfoMotor>().hitted();
		}
		
		float currTime = Time.time;
		
		//-0.5 just to make the sound 100% loopable
		if (currTime - lastShootTime > shootAudio.clip.length - 0.5) { 
			shootAudio.time = 0.6f;
			shootAudio.Play();
			lastShootTime = currTime;
		    
		}
		
		//0.75 the length of the shooting animation part
		if (currTime - lastShootAnimTime > 0.75f) {
			rBarrel.animation["right_barrel_shoot"].time = 0.45f; //0.45 - shooting start time animation
			lBarrel.animation["left_barrel_shoot"].time = 0.45f;
			rBarrel.animation.Play();
			lBarrel.animation.Play();
			lastShootAnimTime = currTime;
		}
		
	}
	
	public void stopShoot() {
		
		stopShootAudio.Play();
	}
	
	private void launchBullets() {
		
		foreach (GameObject g in bulletSpawns) {
			Rigidbody bullet = Instantiate(bulletPrefab, g.transform.position, g.transform.rotation) as Rigidbody;
			bullet.velocity = g.transform.forward * 300;
			Destroy(bullet.gameObject, 2f);
		}
		
	}
	
}
