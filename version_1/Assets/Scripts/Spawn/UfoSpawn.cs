using UnityEngine;
using System.Collections;

public class UfoSpawn : MonoBehaviour
{
	public GameObject ufo;
	public float spawnTime = 10.0f;
	
	public float minX = -30;
	public float maxX = 0;
	public float minZ = 20;
 	public float maxZ = 35;
	public float minY = 100;
	public float maxY = 200;

    private float timeSinceSpawn = 0.0f;

    void Update () 
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn >= spawnTime)
        {
            timeSinceSpawn = 0.0f;
			var pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            Instantiate(ufo, pos, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        }
    }
}
