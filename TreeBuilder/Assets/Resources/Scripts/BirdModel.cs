using UnityEngine;
using System.Collections;

public class BirdModel : MonoBehaviour {

	private Bird owner;
	private Material mat;
	int state = 0;

	// Use this for initialization
	public void init (Bird owner) {
		this.owner = owner;
		transform.parent = owner.transform;
		transform.localPosition = new Vector3(0, 0,0);

		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/Bird1");

		mat.color = new Color(1, 1, 1);
		mat.renderQueue = RenderCoordinator.ANIMAL_RQ;

		InvokeRepeating("switchModel", 1f, 0.5f);
	}

	void switchModel()
	{
		if (state == 0)
		{
			mat.mainTexture = Resources.Load<Texture2D>("Textures/Bird2");
			state = 1;
		}
		else if(state == 1)
		{
			mat.mainTexture = Resources.Load<Texture2D>("Textures/Bird1");
			state = 0;
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
