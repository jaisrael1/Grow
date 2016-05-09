using UnityEngine;
using System.Collections;

public class LeafModel : MonoBehaviour {

	public Material mat;
	public Branch b;
	public float targetScale;
	public bool shrinking;
	bool firstTime;

	public void init (Branch b){
		this.b = b;
		transform.parent = b.hexEnd.transform;
		transform.localPosition = new Vector3(0, 0, 0);
		//transform.position = b.joint.transform.position;
		//transform.localScale = b.joint.transform.localScale;

		targetScale = transform.localScale.x * 0.3f;
		transform.localScale *= 0.01f;

		shrinking = false;
		firstTime = true;
		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_orb");
		mat.color = b.hexStart.tree.leafColor;//new Color (d(34), d(139), d(34));
		mat.renderQueue = RenderCoordinator.LEAF_RQ;   
	}

	public void changeScale(){
		//if (transform.localScale.x < 2f) {
			//transform.localScale = new Vector3 (transform.localScale.x + 0.5f, transform.localScale.y + 0.5f, 0);
			targetScale += 0.07f;
		//}
	}

	public void setScale(int d){
		float f = (float)d / 10.5f;
		//transform.localScale = new Vector3 (transform.localScale.x + f, transform.localScale.y + f, 0);
		targetScale = targetScale + f;
		/*
		if (transform.localScale.x >= 11.5f) {
			transform.localScale = new Vector3 (11.5f, 11.5f, 0);
		}
		*/
		if (targetScale > 0.85f) {
			targetScale = 0.85f;
		}
	}



	void Update(){
		if (transform.localScale.x < targetScale && !shrinking && firstTime){
			transform.localScale = new Vector3 (transform.localScale.x + 0.035f, transform.localScale.y + 0.035f, 0);
			if (transform.localScale.x >= targetScale) {
				firstTime = false;
			}
		}
		if (transform.localScale.x < targetScale && !shrinking && !firstTime){
			transform.localScale = new Vector3 (transform.localScale.x + 0.0025f, transform.localScale.y + 0.0025f, 0);
		}
		if (shrinking) {
			transform.localScale *= 0.9f;
			if (transform.localScale.x < 0.25f) {
				Destroy (this.gameObject);
			}
		}
	}

	void Start(){
		shrinking = false;
		firstTime = true;
	}

	float d(int v){
		return ((float)v / 255f);
	}
	public void flower(){
		mat.mainTexture = Resources.Load<Texture2D>("Textures/Flower");
		mat.color = Color.white;
		transform.localScale = new Vector3 (1f, 1f, 0);
	}
}
