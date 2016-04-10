using UnityEngine;
using System.Collections;

public class HexModel : MonoBehaviour {

	Hex hex;
	public Material mat;

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
		mat.color = new Color(1,1,1,0.25f);											

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
