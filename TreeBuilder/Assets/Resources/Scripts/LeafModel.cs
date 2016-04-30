using UnityEngine;
using System.Collections;

public class LeafModel : MonoBehaviour {

	public Material mat;
	public Branch b;

	public void init (Branch b){
		this.b = b;
		transform.parent = b.joint.transform;
		transform.localPosition = new Vector3(0, 0, 0);
		transform.localScale *= 0.4f;//= new Vector3 (0.8f, 0.8f, 0);
	
		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_orb");
		mat.color = new Color (d(34), d(139), d(34));
		mat.renderQueue = RenderCoordinator.LEAF_RQ;   
	}

	public void changeScale(){
		if (transform.localScale.x < 2f) {
			transform.localScale = new Vector3 (transform.localScale.x + 0.5f, transform.localScale.y + 0.5f, 0);
		}
	}

	public void setScale(int d){
		float f = (float)d;
		transform.localScale = new Vector3 (transform.localScale.x + f, transform.localScale.y + f, 0);
		if (transform.localScale.x >= 11.5f) {
			transform.localScale = new Vector3 (11.5f, 11.5f, 0);
		}
	}

	float d(int v){
		return ((float)v / 255f);
	}

}
