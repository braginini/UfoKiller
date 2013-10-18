using UnityEngine;
using System.Collections;

public abstract class LookAtBase : MonoBehaviour {

    public Transform[] transforms; //objects to look at camera

    public Camera camera;

    public GameObject mousePlane;

    public float planeOffset = 1f;

    public Transform mouseLocationIndicator;

    protected Ray ray;

    protected Ray oldRay;

	protected void Start () {

        if (!camera)
        {

            if (!Camera.main)
            {

                Debug.LogError("No Main Camera found. Please either assign a camera, or tag an existing camera as 'Main Camera'.");
                Destroy(this);
                return;
            }

            camera = Camera.main;
        }

        mousePlane.transform.parent = camera.transform;

        mousePlane.renderer.enabled = false;

        if (planeOffset < 0f)
        {

            planeOffset = camera.nearClipPlane * 2f;

        }

        //mousePlane.transform.position = camera.transform.position + camera.transform.forward * planeOffset;


        mousePlane.transform.up = -camera.transform.forward;
	}
	
	// Update is called once per frame
	void Update ()
	{

	    oldRay = ray;
        UpdateControlRay();

        if (ray.direction.Equals(oldRay.direction)) //to support perceptual and mouse control at the same time
            return;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            foreach (Transform t in transforms)
            {
                t.LookAt(hit.point);
            }

            if (mouseLocationIndicator)
            {
                mouseLocationIndicator.position = camera.WorldToViewportPoint(hit.point);
            }

        }

        
	}

    public abstract void UpdateControlRay();
}
