using UnityEngine;
using System.Collections;

public class WaterModel : MonoBehaviour {

	private Water owner;
	private Material mat;

	// Use this for initialization
	public void init (Water owner) {
		this.owner = owner;
		transform.parent = owner.transform;
		transform.localPosition = new Vector3(0, 0,0);

		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_hexagon");
		mat.color = new Color(0.15f, 0.15f, 0.9f, 0.75f);
		mat.renderQueue = RenderCoordinator.WATER_RQ;                            

	}

}
