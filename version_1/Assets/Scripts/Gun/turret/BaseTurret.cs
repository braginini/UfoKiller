using UnityEngine;
using System.Collections;

/**
 * Class used as a base for turret-like weapons 
 * 
**/
public abstract class BaseTurret : BaseGun {
	
	public float fireRate = 0.1f;
	private float nextFire = 0f;
	
	public AudioClip shotSound;
	
	public Rigidbody bulletPrefab;
	
	public GameObject[] bulletSpawns;
	
	public GameObject[] shellExits;
	
	public ParticleSystem[] barrelSmokes;
	
	public GameObject muzzleFlash;
	
	public GameObject[] muzzlePoints;
	
	public bool launchBullets = false;

    public bool isTouch = false;
	
	public virtual void Start ()
	{

	    base.Start();
		
		if (!shotSound)
			Debug.LogWarning("No shot sound attached");
		
		if (!bulletPrefab)
			Debug.LogWarning("No bullet prefab attached");
		
		if (bulletSpawns.Length == 0)
			bulletSpawns = GameObject.FindGameObjectsWithTag("bullet_spawn");
		
		if (shellExits.Length == 0)
			Debug.LogWarning("No bullet shell exits attached");
		
		if (barrelSmokes.Length == 0)
			Debug.LogWarning("No barrel smoke particle systems attached");
		
		if (!muzzleFlash)
			Debug.LogWarning("No muzzle flash prefab attached");
		
		if (muzzlePoints.Length == 0) {
			Debug.LogWarning("No muzzle flash points attached");
			
			if (bulletSpawns.Length > 0) {
				muzzlePoints = bulletSpawns; //use bullet spawns as muzzle flush points
			}
		}
	
	}	
	
	public virtual void Update ()
	{
	    base.Update();

		if (Input.GetButton("Fire1") && !isTouch) {
			Fire();
		}
	}
	
	public virtual void Fire()
	{
	    if (Time.time > nextFire && !isEmpty)
	    {

	        ReduceBullets(6);

	        Vector3 fwd = transform.TransformDirection(transform.forward);

	        //RaycastHit hit;

	        //Debug.DrawRay(transform.position, fwd * 10, Color.green);	

	        PlaySound();

	        PlayAnimation();

	        if (launchBullets)
	            LaunchBullets();

	        ExitBulletShells();

	        Smoke();

	        MuzzleFlash();

	        HitTraget();

            nextFire = Time.time + fireRate;
	    }

	}
	
	public virtual void PlaySound() {
		if (shotSound) {
			AudioSource.PlayClipAtPoint(shotSound, transform.position, 0.5f);			
		}
	}
	
	public virtual void HitTraget() {
					
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

        //deal only with layer 8 - enemy and layer 9 - friend
	    int enemyLayer = 8;
	    int friendLayer = 9;
	    LayerMask layerMask = (1 << enemyLayer) | (1 << friendLayer); 
			
		if (Physics.Raycast(ray, out hit , Mathf.Infinity, layerMask)) {

		    if (hit.collider.gameObject.tag.Equals("enemy"))
		    {

		        UfoMotor ufoMotorScript = hit.collider.gameObject.GetComponent<UfoMotor>();

		        if (ufoMotorScript)
		        {
		            ufoMotorScript.hitted(hit, ray);
		        }
		    } else if (hit.collider.gameObject.tag.Equals("animal"))
		    {
                Debug.Log(">>>>>>>>>>>>>>> Hitted animal");
		        MoveMotor moveMotorScript = hit.collider.gameObject.GetComponent<MoveMotor>();

		        if (moveMotorScript)
		        {
		            moveMotorScript.hitted(hit, ray);
		        }
		    }
		    

		}
				
	}
	
	public virtual void LaunchBullets() {

	    if (bulletPrefab)
	    {
	        for (int i = 0; i < 2; i++)
	        {
	            //here we reduce the number of actual bullets launched to 2
	            GameObject g = bulletSpawns[Random.Range(0, bulletSpawns.Length)];
	            Rigidbody bullet = Instantiate(bulletPrefab, g.transform.position, g.transform.rotation) as Rigidbody;
	            bullet.velocity = g.transform.forward*500;
	            Destroy(bullet.gameObject, 1f);
	        }
	        /*foreach (GameObject g in bulletSpawns) {
			Rigidbody bullet = Instantiate(bulletPrefab, g.transform.position, g.transform.rotation) as Rigidbody;
			bullet.velocity = g.transform.forward * 500;
			Destroy(bullet.gameObject, 1f);
	    }*/
	    }

	}
	
	public virtual void ExitBulletShells() {
		
		foreach (GameObject g in shellExits) {
			
			BulletShellExit shellExitScript = g.GetComponent<BulletShellExit>();
			
			if (shellExitScript)
				shellExitScript.ExitShell();
			
		}
	}
	
	public virtual IEnumerator Smoke() {
		
		foreach (ParticleSystem p in barrelSmokes) {
			
			p.enableEmission = true;
		}
		
		yield return new WaitForEndOfFrame();
		
		foreach (ParticleSystem p in barrelSmokes) {
			
			p.enableEmission = false;
		}
	}
	
	public virtual void MuzzleFlash() {
		
		if (muzzleFlash && bulletSpawns.Length > 0) {
			
			foreach (GameObject muzzlePoint in muzzlePoints) {
				
				GameObject muzzleFlashClone = Instantiate(muzzleFlash, muzzlePoint.transform.position, muzzlePoint.transform.rotation) as GameObject;
				muzzleFlashClone.transform.parent = transform; //here we hold muzzle flash with the gun
				GameObject.Destroy(muzzleFlashClone, 0.3f);
				
			}
		}
		
	}
	
	public abstract void PlayAnimation();
	
}
