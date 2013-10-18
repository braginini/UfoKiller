using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BazookaScript : MonoBehaviour
{
	public AudioClip shootSound;
	public Rigidbody bulletPrefab;
	public float bulletSpeed = 100.0f;
	
	void Update ()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			audio.PlayOneShot(shootSound);
			var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as Rigidbody;
			bullet.velocity = transform.forward * bulletSpeed;
			Destroy(bullet.gameObject, 3f);
			//Physics.IgnoreCollision(transform.root.collider, bullet.collider, true);
		}
	}
}
