using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float destroyAfter;
	private float instantiationTime;

	// Use this for initialization
	void Start () {
		instantiationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - instantiationTime > destroyAfter)
			Destroy(transform.root.gameObject);
	}
}
