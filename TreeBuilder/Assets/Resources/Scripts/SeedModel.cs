using UnityEngine;
using System.Collections;

public class SeedModel : MonoBehaviour {

	private Tree owner;
	private Material mat;

	// Use this for initialization
	public void init (Tree owner) {
		this.owner = owner;
		transform.parent = owner.transform;
		transform.localPosition = new Vector3(0, 0, 0);

		transform.localScale *= 0.5f;

		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/Acorn");

		mat.renderQueue = RenderCoordinator.SEED_RQ;                            

	}

}
