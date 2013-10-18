using UnityEngine;
using System.Collections;

public class UfoDetonator : MonoBehaviour
{

    private GameObject ufo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!ufo)
	        ufo = GameObject.FindWithTag("enemy");

        if (Input.GetButton("Fire1") && ufo)
        {
            StartCoroutine(ufo.GetComponent<FragmentedObjectExploder>().Explode());
        }
	
	}
}
