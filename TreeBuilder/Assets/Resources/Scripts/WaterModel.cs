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
		mat.mainTexture = Resources.Load<Texture2D>("Textures/water drop");
		mat.color = new Color(1, 1, 1);
		mat.renderQueue = 5003;                            

	}

	// Update is called once per frame
	void Update () {

	}
}
