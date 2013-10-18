using UnityEngine;
using System.Collections;

public class UfoSpawner : MonoBehaviour
{

    public GameObject ufoObj;

    public Vector3 position;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
        GameObject ufo = GameObject.FindGameObjectWithTag("enemy");

	    if (!ufo)
	    {
	        GameObject clone = Instantiate(ufoObj, position, transform.rotation) as GameObject;
	        clone.tag = "enemy";
	    }
	}
}
