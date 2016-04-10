using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour {

	Hex hex;
	Material mat;

	// Use this for initialization
	public void init(Hex h){

		hex = h;

		transform.parent = hex.transform;				
		transform.localPosition = new Vector3(0,0,0);		
		name = "Hex Model";									

		mat = GetComponent<Renderer>().material;
		mat.renderQueue = 4999;
		mat.shader = Shader.Find ("Transparent/Diffuse");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_hexagon");	
		mat.color = new Color(0.7f,0.5f,0.3f);											
	}
}
