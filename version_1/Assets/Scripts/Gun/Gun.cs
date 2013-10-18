using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private GameObject gun;

    void Start()
    {
        gun = GameObject.FindWithTag("gun");
    }
	
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (gun)
            {
                gun.audio.Play();
                gun.animation.Play("shoot");
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.gameObject.tag == "target")
                StartCoroutine(KillTarget(hit.collider.gameObject));
        }
    }

    IEnumerator KillTarget(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer)
            renderer.enabled = false;

        var pe = obj.GetComponentInChildren<ParticleSystem>();            
        if (pe)
            pe.Play();

        var audio = obj.GetComponent<AudioSource>();
        if (audio)
        {
            yield return new WaitForSeconds(0.3f);
            audio.Play();
            yield return new WaitForSeconds(0.5f);
        }

        GameObject.Destroy(obj);
    }
}
