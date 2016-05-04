using UnityEngine;
using System.Collections;

public class OrbEffectModel : MonoBehaviour {

	public EnvironmentManager em;
	public float realX;
	public float realY;
	public float alpha;
	public float size;
	public Material mat;


	public void init(EnvironmentManager em, float realX, float realY, int type){
		this.em = em;
		transform.position = new Vector3 (realX, realY, 0);
		transform.localScale *= 0.8f;

		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find("Sprites/Default");
		if (type == EnvironmentManager.SUNNY_WEATHER)
		{
			mat.mainTexture = Resources.Load<Texture2D>("Textures/Sundrop1");
		}
		if (type == EnvironmentManager.RAINY_WEATHER)
		{
			mat.mainTexture = Resources.Load<Texture2D>("Textures/WaterDrop");
		}
		mat.color = new Color(1, 1, 1, 1);
		mat.renderQueue = RenderCoordinator.SUNDROP_RQ;
	}

	void Start () {
		
	}

	void Update () {
		transform.localScale *= 1.0218f;
		if (mat.color.a <= 0.05f) {
			Destroy (this.gameObject);
		}
		mat.color = new Color (1, 1, 1, mat.color.a - 0.0155f);
	}
}
