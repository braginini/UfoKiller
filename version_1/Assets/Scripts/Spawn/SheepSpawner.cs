using UnityEngine;

public class SheepSpawner : MonoBehaviour
{
    public int count = 10;
    public float minX = -32;
    public float maxX = 36;
    public float minZ = 88;
    public float maxZ = 166;
    public GameObject sheep;

    void Start()
    {
		GameObject.Find("Inventory").BroadcastMessage("updateSheepCount", count);
		
        for (int i = 0; i < count; ++i)
        {
            var pos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
            Instantiate(sheep, pos, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        }
    }
}
