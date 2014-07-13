using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public AudioClip[] hittedAudio;

	public float hitVolume = .9f;

	public void hitted()
	{
		if (this.hittedAudio.Length > 0)
		{
			audio.PlayOneShot(hittedAudio[Random.Range(0, hittedAudio.Length)], hitVolume);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("Bullet HIT " + collider.gameObject.tag);
		if (collider.gameObject.tag == "enemy")

		{
			Debug.Log("Bullet HIT" + collider.gameObject.layer);
			//hitted();
		}

	}
}
