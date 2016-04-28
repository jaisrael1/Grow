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
		if (owner.type == 0) {
			mat.color = new Color (0.15f, 0.9f, 0.15f, 0.75f);
		} else if (owner.type == 1) {
			mat.color = new Color(.9f, .5f, 0f);
		}else if (owner.type == 2) {
			mat.color = new Color (0.0f, 0.0f, 0.9f, 0.75f);
		}//else if (owner.type == 4) {
		//	mat.color = new Color (0.9f, 0.15f, 0.15f, 0.75f);
		//}
		mat.renderQueue = RenderCoordinator.ORB_RQ;                            

	}
		
}
