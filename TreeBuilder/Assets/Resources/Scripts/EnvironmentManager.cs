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
	public float timeSunnyLastsFor = 15f;
	public float timeSunnyGotten;
	public bool experiencingSunny;

	public List<Cloud> cloudList;
	public const float CLOUD_MAXLENGTH = 11f;
	public const float CLOUD_TIME = Cloud.STEP_INTERVAL * CLOUD_MAXLENGTH;
	public float timeSinceLastCloud;
	public int rainclouds;

	public static float ORB_BASE_PROB = 10f;
	public static float WATER_BASE_PROB = 50f;

	GameObject lightFolder;
	List<Light> lights;
	GameObject waterFolder;
	List<Light> waterDrops;
	Background background;
	public List<Orb> orbs;

	public void init(Controller c){
		this.c = c;
		orbs = new List<Orb>();
		addWaterToWorld ();
	}
	void Start () {


		GameObject backgroundObject = new GameObject ();
		background = backgroundObject.AddComponent<Background> ();
		background.init (this);


		cloudList = new List<Cloud> ();
		timeSinceLastCloud = 0f;
		clock = 0f;
		timeSunnyGotten = 0f;
		weather = NORMAL_WEATHER;
		experiencingSunny = false;

		lightFolder = new GameObject();
		lightFolder.name = "Sundrops";
		lights = new List<Light>();
		waterFolder = new GameObject();
		waterFolder.name = "Waterdrops";
		waterDrops = new List<Light> ();

		InvokeRepeating("sunGenerator", 0f, 0.25f);
	}


	public void changeWeather (int type){
		weather = type;
		if (type == NORMAL_WEATHER) {
			experiencingSunny = false;
		} 
		if (type == SUNNY_WEATHER) {
			timeSunnyGotten = clock;
			experiencingSunny = true;
			foreach (Cloud i in cloudList) {
				i.shrink ();
			}
			cloudList = new List<Cloud> ();
			rainclouds = 0;
		}
		if (type == RAINY_WEATHER) {
			foreach (Light i in lights) {
			//	i.shrink ();
			}
			//lights = new List<Light> ();
			createCloud (true);
			rainclouds++;
		}
		print ("changing weather to type " + weather);
		//background.change (weather);
	}

	// Update is called once per frame
	void Update () {
		clock += Time.deltaTime;

		//weather stuff
		if (experiencingSunny) {
			if (clock - timeSunnyGotten > timeSunnyLastsFor) {
				changeWeather (NORMAL_WEATHER);
			}
		}

		//cloud stuff
		timeSinceLastCloud += Time.deltaTime;
		if (timeSinceLastCloud >= CLOUD_TIME && weather != SUNNY_WEATHER) {
			if (UnityEngine.Random.Range (0, 3) == 0) {
				createCloud (false);
				timeSinceLastCloud = 0f;
			} else {
				timeSinceLastCloud -= 3f;
			}
		}

	}
		
	void createCloud(bool isRain){
		int height = UnityEngine.Random.Range (Controller.WORLD_HEIGHT / 6, Controller.WORLD_HEIGHT / 2 - 1);
		int length;
		if (!isRain) {
			length = UnityEngine.Random.Range (3, (int)CLOUD_MAXLENGTH);
		} else {
			length = Controller.WORLD_WIDTH / 3 + UnityEngine.Random.Range (-3, 3);;
		}
		var cloudObject = new GameObject ();
		Cloud cl = cloudObject.AddComponent<Cloud> ();
		cl.init (this, height, length, isRain);
		cloudList.Add (cl);
	}
		
	void sunGenerator()
	{
		float x = UnityEngine.Random.Range(-Controller.WORLD_WIDTH / 2, Controller.WORLD_WIDTH / 2);
		if (weather == NORMAL_WEATHER || weather == RAINY_WEATHER){
			createSun (0.75f * x, (Controller.WORLD_HEIGHT / 2) * Mathf.Sqrt (3) / 2f, NORMAL_WEATHER );
		}
		if (weather == SUNNY_WEATHER) {
			createSun (0.75f * x, (Controller.WORLD_HEIGHT / 2) * Mathf.Sqrt (3) / 2f, SUNNY_WEATHER );
		}
	}

	public void createSun(float x, float y, int type)
	{
		GameObject lightObject = new GameObject();

		BoxCollider2D box = lightObject.AddComponent<BoxCollider2D>();         //Colliders
		box.size = new Vector2(0.5f, 0.5f);
		lightObject.SetActive(true);
		box.isTrigger = true;

		Rigidbody2D rig = lightObject.AddComponent<Rigidbody2D>();
		rig.isKinematic = true;

		Light newLight = lightObject.AddComponent<Light>();

		newLight.init (x, y, type, this);
		

		if (type != RAINY_WEATHER) {
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
		if(Vector3.Distance(new Vector3(0, 0, 0), h.transform.position) < 7){
            return 0;
        }else{
            return ORB_BASE_PROB;
        }
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
		if (h.transform.position.y <= 4) {
			newOrb.init (h, UnityEngine.Random.Range (1, 3), this);
		} else if (h.transform.position.y > 4) {
			newOrb.init (h, UnityEngine.Random.Range (0, 3), this);
		}
		newOrb.name = "Orb" + newOrb.type;
		orbs.Add (newOrb);
	}

	public void removeNewTreeOrbs(){
		foreach (Orb i in orbs) {
			if (i.type == 0) {
				i.Shrink ();
			}
		}
	}
}
