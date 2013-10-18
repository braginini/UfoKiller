using UnityEngine;
using System.Collections;

public abstract class BaseGun : MonoBehaviour
{

    public int gunClipSize = 1;

    public int bulletsLeft;

    public float damage = 1;

    protected bool isEmpty = false;

    public AudioClip reloadSound;

	public void Start ()
	{

	    bulletsLeft = gunClipSize;

	}
	
	public void Update ()
	{

	    if (Input.GetKeyDown(KeyCode.R))
	    {
	       StartCoroutine(Reload());
	    }

        if (bulletsLeft < 1)
        {
            bulletsLeft = 0;
	        isEmpty = true;
	    }

	}

    public IEnumerator Reload()
    {

        if (bulletsLeft == gunClipSize)
            yield return null;

        if (reloadSound)
        {
            AudioSource.PlayClipAtPoint(reloadSound, transform.position);
            yield return new WaitForSeconds(reloadSound.length);
        }

        bulletsLeft = gunClipSize;
        isEmpty = false;
    }

    public void ReduceBullets(int amount)
    {
        bulletsLeft -= amount;
    }
}
