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
			targetScale += 0.05f;
		//}
	}

	public void setScale(int d){
		float f = (float)d / 11f;
		//transform.localScale = new Vector3 (transform.localScale.x + f, transform.localScale.y + f, 0);
		targetScale = targetScale + f;
		/*
		if (transform.localScale.x >= 11.5f) {
			transform.localScale = new Vector3 (11.5f, 11.5f, 0);
		}
		*/
		if (targetScale > 0.8f) {
			targetScale = 0.8f;
		}
	}



	void Update(){
		if (transform.localScale.x < targetScale && !shrinking && firstTime){
			transform.localScale = new Vector3 (transform.localScale.x + 0.07f, transform.localScale.y + 0.07f, 0);
			if (transform.localScale.x >= targetScale) {
				firstTime = false;
			}
		}
		if (transform.localScale.x < targetScale && !shrinking && !firstTime){
			transform.localScale = new Vector3 (transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 0);
		}
		if (shrinking) {
			transform.localScale *= 0.8f;
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
		transform.localScale = new Vector3 (5f, 5f, 0);
	}
}
