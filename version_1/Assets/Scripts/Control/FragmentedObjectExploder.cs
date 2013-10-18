using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class FragmentedObjectExploder : MonoBehaviour
{

    public GameObject toExplode; //actual object to explode (which consists of fragments)

    public GameObject[] toDestroyFirst; //objects to destroy before explosion

    public float explosionForce = 900;

    public GameObject explosion; //explosion effect

    public ForceMode forceMode = ForceMode.Acceleration;

    public bool disableFragments = false;

    public float multiplyFragmentMass = 1f;

    public float timeToLive = 2f;

    private bool exploded = false;
    
	
	void Start ()
    {

	}
	
	
	void Update ()
	{

	}

    public IEnumerator Explode()
    {

        if (!exploded)
        {
            exploded = true;

            Instantiate(explosion, new Vector3(this.transform.position.x, this.transform.position.y + 4.23769f, this.transform.position.z), this.transform.rotation);
         
            beforeExplode();

            if (toExplode && !disableFragments)
            {
                foreach (Transform child in toExplode.transform)
                {

                    GameObject GO = child.gameObject;

                    var rb = GO.GetComponent<Rigidbody>();

                    if (!rb)
                        rb = GO.AddComponent<Rigidbody>();
                    rb.mass = rb.mass*multiplyFragmentMass;

                    Vector3 direction = new Vector3(
                        Random.Range(-180, 180),
                        Random.Range(-180, 180),
                        Random.Range(-180, 180));

                    //rb.AddTorque(direction * explosionForce, forceMode);
                    rb.AddForceAtPosition(direction * explosionForce, child.position, forceMode);
                    //rb.AddForce(direction * explosionForce, forceMode);

                    Destroy(GO, timeToLive + Random.Range(0.0f, 5.0f));
                }

                yield return new WaitForSeconds(timeToLive);
            }

            Destroy(gameObject);
            
        }
        
    }

    private void beforeExplode()
    {
        foreach (GameObject g in toDestroyFirst)
        {
            Destroy(g);
        }
    }


}
