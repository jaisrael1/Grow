using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	EnvironmentManager em;
	Color currentColorBottom;
	Color currentColorTop;
	Color targetColorBottom;
	Color targetColorTop;
	bool moving;
	LineRenderer lr;
	Material mat;

	Color normBottom;
	Color normTop;
	Color sunBottom;
	Color sunTop;
	Color rainBottom;
	Color rainTop;

	public void init(EnvironmentManager em){
		this.em = em;
		lr = this.gameObject.AddComponent<LineRenderer> ();
		normBottom = new Color (d(0), d(188), d(255));
		normTop =    new Color (d(172), d(234), d(255));
		sunBottom =  new Color (d (107), d (216), d (255));
		sunTop =     new Color (d (208), d (243), d (255));
		rainBottom = new Color (d (161), d (161), d (161));
		rainTop =    new Color (d (70), d (70), d (70));

		//lr.material.shader = Shader.Find ("Transparent/Diffuse");
		lr.material = new Material(Shader.Find("Particles/Additive"));

		//lr.material.mainTexture = Resources.Load<Texture2D>("Particles/Additive");
		//lr.material.color = new Color (1, 0, 0, 0);
		lr.material.renderQueue = RenderCoordinator.BACKGROUND_RQ;

		lr.SetColors (normBottom, normTop);
		currentColorTop = normTop;
		currentColorBottom = normBottom;

		lr.SetWidth (Controller.WORLD_WIDTH, Controller.WORLD_WIDTH);
		lr.SetPosition(0, new Vector3(0f, -1f, 0f));
		lr.SetPosition(1, new Vector3(0f, Controller.WORLD_HEIGHT * Mathf.Sqrt(3) / 4, 0f));

	}

	public void change(int weather){
		if (weather == EnvironmentManager.RAINY_WEATHER) {
			targetColorBottom = rainBottom;
			targetColorTop = rainTop;
		} 
		if (weather == EnvironmentManager.SUNNY_WEATHER) {
			targetColorBottom = sunBottom;
			targetColorTop = sunTop;
		}
		if (weather == EnvironmentManager.NORMAL_WEATHER) {
			targetColorBottom = normBottom;
			targetColorTop = normTop;
		}
		moving = true;
	}

	// Update is called once per frame
	void Update () {
		if (moving) {

			float rt = incrementColor (currentColorTop.r, targetColorTop.r);
			float gt = incrementColor (currentColorTop.g, targetColorTop.g);
			float bt = incrementColor (currentColorTop.b, targetColorTop.b);
			float rb = incrementColor (currentColorBottom.r, targetColorBottom.r);
			float gb = incrementColor (currentColorBottom.g, targetColorBottom.g);
			float bb = incrementColor (currentColorBottom.b, targetColorBottom.b);

			currentColorTop = new Color (rt, gt, bt);
			currentColorBottom = new Color (rb, gb, bb);
			lr.SetColors (currentColorBottom, currentColorTop);

			if (rt == targetColorTop.r && gt == targetColorTop.g && bt == targetColorTop.b &&
				rb == targetColorBottom.r && gb == targetColorBottom.g && bb == targetColorBottom.b){
				moving = false;
			}
				
		}
	}
		
	float d(int v){
		return ((float)v / 255f);
	}

	void Start(){
		moving = false;
	}

	float incrementColor(float current, float target){
		if (current == target) {
			return current;
		}
		if (Mathf.Abs (current - target) <= 0.0015f) {
			return target;
		}
		if (current > target) {
			return current - 0.001f;
		}
		if (current < target) {
			return current + 0.001f;
		} else {
			return 0;
		}
	}
}
