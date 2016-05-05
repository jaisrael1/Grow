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
		mat.shader = Shader.Find ("Transparent/Diffuse");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/tile_white");	
		mat.color = new Color(0,0,0,0.1f);		
		if (h.coordY >= 0) {
			mat.renderQueue = RenderCoordinator.HEX_BORDER_NA_A_RQ;
		} else {
			mat.renderQueue = RenderCoordinator.HEX_BORDER_NA_G_RQ;
		}
	}
}
