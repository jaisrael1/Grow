using UnityEngine;
using System.Collections;

public class HexModel : MonoBehaviour {

	Hex hex;
	Material mat;

	// Use this for initialization
	public void init(Hex h){
		
		hex = h;

		transform.parent = hex.transform;				
		transform.localPosition = new Vector3(0,0,0);		
		name = "Hex Model";									

		mat = GetComponent<Renderer>().material;
		mat.renderQueue = 5000;
		mat.shader = Shader.Find ("Transparent/Diffuse");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/tile");	
		mat.color = new Color(1,1,1);											

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
