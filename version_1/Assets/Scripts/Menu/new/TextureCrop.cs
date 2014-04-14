using UnityEngine;
using System.Collections;

public class TextureCrop : MonoBehaviour {

	public Texture2D texture;
	private Renderer renderer;
	public Rect textureCrop = new Rect( 1f, 1f, 1f, 1f );
	public Vector2 position = new Vector2( 10, 10 );

	void Start() 
	{
		texture = (Texture2D)gameObject.GetComponent<GUITexture>().texture;
		Debug.Log("Texture size=" + texture.width + "x" + texture.height);
		Debug.Log("Screen size=" + Screen.width + "x" + Screen.height);
		transform.localScale = new Vector3(1f,1f,0f);
	}
	
	void OnGUI()
	{
		//GUI.BeginGroup( new Rect( position.x, position.y, texture.width * textureCrop.width, texture.height * textureCrop.height ) );
		//GUI.DrawTexture( new Rect( -texture.width * textureCrop.x, -texture.height * textureCrop.y, texture.width, texture.height ), texture );
		//GUI.EndGroup();
	}
}
