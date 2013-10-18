using UnityEngine;
using System.Collections;

public class BulletShellExit : MonoBehaviour {
	
	public Rigidbody shell;
	
	public AudioClip bulletAndShellAudio;	
	
	//how many shells should exit at one time
	//usually it is a number of barrels
	public int shellCount = 1;

	void Start () {
		
		if(!shell)
			Debug.LogError("No bullet shell attached");
	
	}
	
	void Update () {		
	}
	
	public void ExitShell() {

	    if (shell)
	    {

	        for (int i = 0; i < shellCount; i++)
	        {

	            Rigidbody shellClone = Instantiate(shell, transform.position, transform.rotation) as Rigidbody;

	            shellClone.velocity = transform.TransformDirection(-1.5f, Random.Range(1.0f, 2.0f),
	                Random.Range(-0.1f, 0.1f));
	            shellClone.AddRelativeTorque(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
	            Destroy(shellClone.gameObject, 0.5f);

	            if (bulletAndShellAudio)
	            {

	                shellClone.audio.clip = bulletAndShellAudio;
	                shellClone.audio.pitch = Random.Range(0.5f, 1.5f);
	                shellClone.audio.volume = 0.15f;
	                shellClone.audio.Play();

	            }

	        }
	    }
	}
}
