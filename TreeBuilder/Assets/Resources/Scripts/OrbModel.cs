using UnityEngine;
using System.Collections;

public class OrbModel : MonoBehaviour {

	private Orb owner;
	private Material mat;

	// Use this for initialization
	public void init (Orb owner) {
		this.owner = owner;
		transform.parent = owner.transform;
		transform.localPosition = new Vector3(0, 0,0);

		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_orb");
		mat.color = new Color(0.15f, 0.9f, 0.9f, 0.75f);
		mat.renderQueue = RenderCoordinator.ORB_RQ;                            

	}

}
