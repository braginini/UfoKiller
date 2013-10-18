using UnityEngine;
using System.Collections;

public class TargetedSheep : MonoBehaviour
{
	/// <summary>
	/// Every ufo marks its sheep as targeted to resolve conflicts between different ufos.
	/// </summary>
	public bool targeted = false;
	
	/// <summary>
	/// This flags shows when sheep is being captured by an ufo.
	/// </summary>
	public bool capturing = false;
}
