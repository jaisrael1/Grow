using UnityEngine;
using System.Collections;

public class TileModel : MonoBehaviour
{
	Hex hex;
	Material mat;
	public const int CLOUD_MODEL = 1;
	public const int RAIN_CLOUD_MODEL = 3;
	public const int GROUND_MODEL = 2;
	public int modelType;
	public bool shrinking;

	public void init (Hex h, int modelType)
	{
		hex = h;
		this.modelType = modelType;
		transform.parent = hex.transform;				
		transform.localPosition = new Vector3 (0, 0, 0);		
		name = "Hex Model";									

		mat = GetComponent<Renderer> ().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D> ("Textures/white_hexagon");	
		if (modelType == GROUND_MODEL) {
			mat.color = new Color (d(99), d(69), d(10));
			mat.renderQueue = RenderCoordinator.GROUND_RQ;
		}
		if (modelType == RAIN_CLOUD_MODEL) {
			mat.color = new Color (0.6f, 0.6f, 0.6f, 0.60f);
			mat.renderQueue = RenderCoordinator.CLOUD_RQ;
		}
		if (modelType == CLOUD_MODEL) {
			mat.color = new Color (1, 1, 1, 0.5f);
			mat.renderQueue = RenderCoordinator.CLOUD_RQ;
		}
	}

	void Start(){
		shrinking = false;
	}

	public void shrink(){
		if (modelType == CLOUD_MODEL) {
			shrinking = true;
		}
	}

	void Update(){
		if (shrinking) {
			this.transform.localScale *= 0.95f;
			if(this.transform.localScale.x < 0.1)
			{
				Destroy(this.gameObject);
			}
		}
	}

	float d(int v){
		return ((float)v / 255f);
	}

}
