using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour {

	public Controller c;
	public float clock;

	public const int NORMAL_WEATHER = 0;
	public const int SUNNY_WEATHER = 1;
	public const int RAINY_WEATHER = 2;
	public int weather = NORMAL_WEATHER;
	public float timeOrbLastsFor = 7f;
	public float timeOrbGotten;
	public bool experiencingOrb;

	public List<Cloud> cloudList;
	public const float CLOUD_MAXLENGTH = 11f;
	public const float CLOUD_TIME = Cloud.STEP_INTERVAL * CLOUD_MAXLENGTH;
	public float timeSinceLastCloud;

	public static float ORB_BASE_PROB = .7f;
	public static float WATER_BASE_PROB = 50f;

	GameObject lightFolder;
	List<Light> lights;
	GameObject waterFolder;
	List<Light> waterDrops;

	public void init(Controller c){
		this.c = c;
		addWaterToWorld ();
	}
	void Start () {
		cloudList = new List<Cloud> ();
		timeSinceLastCloud = 0f;
		clock = 0f;
		timeOrbGotten = 0f;
		weather = NORMAL_WEATHER;
		experiencingOrb = false;

		lightFolder = new GameObject();
		lightFolder.name = "Sundrops";
		lights = new List<Light>();
		waterFolder = new GameObject();
		waterFolder.name = "Waterdrops";
		waterDrops = new List<Light> ();

		InvokeRepeating("sunGenerator", 0f, 0.5f);
	}


	public void changeWeather (int type){
		if (type == NORMAL_WEATHER) {
			weather = NORMAL_WEATHER;
			experiencingOrb = false;
		} else {
			timeOrbGotten = clock;
			experiencingOrb = true;
			weather = type;
			if (weather == RAINY_WEATHER) {
				foreach (Light i in lights) {
					i.shrink ();
				}
			}
			if (weather == SUNNY_WEATHER) {
				foreach (Cloud i in cloudList) {
					i.shrink ();
				}
				cloudList = new List<Cloud> ();
			}
			print ("changing weather to type " + weather);
		} 
	}

	// Update is called once per frame
	void Update () {
		clock += Time.deltaTime;

		//weather stuff
		if (experiencingOrb) {
			if (clock - timeOrbGotten > timeOrbLastsFor) {
				experiencingOrb = false;
				weather = NORMAL_WEATHER;
				print ("weather should be normal again");
			}
		}

		//cloud stuff
		timeSinceLastCloud += Time.deltaTime;
		if (timeSinceLastCloud >= CLOUD_TIME) {
			if (UnityEngine.Random.Range (0, 3) == 0) {
				createCloud ();
				timeSinceLastCloud = 0f;
			} else {
				timeSinceLastCloud -= 3f;
			}
		}

	}
		
	void createCloud(){

		int height = UnityEngine.Random.Range (Controller.WORLD_HEIGHT / 6, Controller.WORLD_HEIGHT / 2 - 1);
		int length = UnityEngine.Random.Range (3, (int)CLOUD_MAXLENGTH);

		var cloudObject = new GameObject ();
		cloudList.Add (cloudObject.AddComponent<Cloud>());
		cloudList [cloudList.Count - 1].init (this, height, length);
	}
		
	void sunGenerator()
	{
		float x = UnityEngine.Random.Range(-Controller.WORLD_WIDTH / 2, Controller.WORLD_WIDTH / 2);
		createSun(0.75f * x, (Controller.WORLD_HEIGHT/2)*Mathf.Sqrt(3) / 2f);
	}

	void createSun(float x, float y)
	{
		GameObject lightObject = new GameObject();

		BoxCollider2D box = lightObject.AddComponent<BoxCollider2D>();         //Colliders
		box.size = new Vector2(0.5f, 0.5f);
		lightObject.SetActive(true);
		box.isTrigger = true;

		Rigidbody2D rig = lightObject.AddComponent<Rigidbody2D>();
		rig.isKinematic = true;

		Light newLight = lightObject.AddComponent<Light>();
		newLight.init(x, y, weather, this);

		if (weather != RAINY_WEATHER) {
			lights.Add (newLight);
			newLight.name = "Sundrop " + lights.Count;
			newLight.transform.parent = lightFolder.transform;
		} else {
			waterDrops.Add (newLight);
			newLight.name = "Waterdrop " + waterDrops.Count;
			newLight.transform.parent = waterFolder.transform;
		}
	}

	void addWaterToWorld(){
		Hex h;
		for (int i = -Controller.WORLD_WIDTH/2; i < Controller.WORLD_WIDTH/2; i++) {
			for (int j = -Controller.WORLD_HEIGHT/2; j < Controller.WORLD_HEIGHT/2; j++) {
				h = c.hexArray [i + Controller.WORLD_WIDTH / 2, j + Controller.WORLD_HEIGHT / 2];
				int type = calculateType (h);
				if (type == 2) {
					createWater (h);
				} else if (type == 3) {
					createOrb (h);
				}
			}
		}
	}

	private int calculateType(Hex h)
	{
		float wProb = getWaterProb(h);
		float oProb = getOrbProb(h);
		float r = UnityEngine.Random.Range(5f, 100f);
		if (wProb > r)
		{
			return 2;
		}
		r = UnityEngine.Random.Range(5f, 100f);
		if (oProb > r)
		{
			return 3;
		}
		else {
			return 1;
		}
	}

	private float getOrbProb(Hex h)
	{
		return ORB_BASE_PROB * Vector3.Distance (new Vector3 (0, 0, 0), h.transform.position);
	}

	private float getWaterProb(Hex h)
	{
		float y = h.transform.position.y;
		if (y >= -1)
		{
			return 0;
		}
		return WATER_BASE_PROB * ((y+Controller.WORLD_HEIGHT/2) / Controller.WORLD_HEIGHT)-1;
	}

	void createWater (Hex h){
		GameObject waterObject = new GameObject ();
		Water newWater = waterObject.AddComponent<Water> ();
		newWater.init (h, this);

		newWater.name = "Waterdrop";
		//newWater.transform.parent = waterFolder.transform;
	}

	void createOrb (Hex h){
		GameObject orbObject = new GameObject ();
		Orb newOrb = orbObject.AddComponent<Orb> ();
		newOrb.init(h, UnityEngine.Random.Range(0, 4), this);

		newOrb.name = "Orb" + newOrb.type;
	}

}
