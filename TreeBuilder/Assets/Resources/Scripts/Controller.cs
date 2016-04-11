using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Controller : MonoBehaviour {


	public static float ORB_BASE_PROB = 10f;
	public static float WATER_BASE_PROB = 30f;

	public Hex mouseOver;
	public bool placing;
	public Hex placingFrom; 
	//public Hex airRoot;
	//public Hex groundRoot;
	public Hex root;
	public Branch airRootBranch;
	public Branch groundRootBranch;
	public Branch currentBranch;
	public int treeHeight;

    //GameObject hexFolder;
    //List<Hex> hexes;

    GameObject lightFolder;
    List<Light> lights;

    public int sunEnergy = 10;
    string sunDisplay;

	public AudioManager audioM;

    public const int WORLD_HEIGHT = 80; // the number of vertical tiles
	public const int WORLD_WIDTH = 50;   // number of horizontal tiles 

	public Hex[,] hexArray;

	void Start () {
		populateTiles ();
		placing = false;
		treeHeight = 0;

		GameObject audioObject = new GameObject ();
		audioObject.name = "audio manager";
		audioObject.AddComponent<AudioSource>();
		audioM = audioObject.AddComponent<AudioManager> ();
		audioM.init (this);

        //Folder to store all hexes
        /*hexFolder = new GameObject();
        hexFolder.name = "Hexes";
        hexes = new List<Hex>();*/

        //SunDrops
        lightFolder = new GameObject();
        lightFolder.name = "Sundrops";
        lights = new List<Light>();
        InvokeRepeating("sunGenerator", 0f, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {

		//CAMERA STUFF
		if (Input.GetButton("Vertical")){
			if (!(Camera.main.orthographicSize < 0.5f && Input.GetAxis("Vertical") < 0)) {
				Camera.main.orthographicSize += 0.1f * Input.GetAxis ("Vertical");
			}
		}
		if (Input.mousePosition.x > Camera.main.pixelWidth * 9f / 10f && Input.mousePosition.x < Camera.main.pixelWidth) {
			Camera.main.transform.Translate(new Vector3(0.1f, 0, 0));
		}
		if (Input.mousePosition.x < Camera.main.pixelWidth / 10f && Input.mousePosition.x > 0) {
			Camera.main.transform.Translate (new Vector3 (-0.1f, 0, 0));
		}
		if (Input.mousePosition.y > Camera.main.pixelHeight * 9f / 10f && Input.mousePosition.y < Camera.main.pixelHeight) {
			Camera.main.transform.Translate (new Vector3 (0, 0.1f, 0));
		}
		if (Input.mousePosition.y < Camera.main.pixelHeight / 10f && Input.mousePosition.y > 0) {
			Camera.main.transform.Translate (new Vector3 (0, -0.1f, 0));
		}

		//BRANCH-DRAWING STUFF
		if (Input.GetMouseButtonDown (0)) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			worldPos.z = 0;

			if (!placing) {
				if (mouseOver != null && checkStart (mouseOver)) {
					placingFrom = mouseOver;
					placing = true;

					GameObject branchObject = new GameObject ();
					branchObject.AddComponent<LineRenderer> ();
					Branch branch = branchObject.AddComponent<Branch> ();
					branch.init (placingFrom, this);
					currentBranch = branch;
				}
			}
		}
		if (Input.GetMouseButtonUp(0)){
			if (placing){
				Hex end = mouseOver;
				if (end != null && checkFinish (placingFrom, end)) {
					placingFrom.addBranch (end, currentBranch);
					currentBranch.confirm (end);
					end.updateWidth ();
				} else {
					Destroy (currentBranch.gameObject);
				}
				currentBranch = null;
				placing = false;
			}
		}

        sunDisplay = "Sunlight: " + sunEnergy;
		/*
		 * Some stuff for debugging
=======
		/* debugging stuff
>>>>>>> origin/master
		if (Input.GetMouseButtonUp(0)) { 

			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 	
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			print("you just clicked at "+mouseX+" "+mouseY);

			if (mouseOver != null) {
				print (mouseOver.coordX+" "+mouseOver.coordY);
				//print(mouseOver.Equals(hexArray[mouseOver.coordX + WORLD_WIDTH / 2, mouseOver.coordY + WORLD_HEIGHT /2]));
			}
		}
		*/

	}

	void populateTiles(){
		hexArray = new Hex[WORLD_WIDTH, WORLD_HEIGHT];
		for (int i = -WORLD_WIDTH/2; i < WORLD_WIDTH/2; i++) {
			for (int j = -WORLD_HEIGHT/2; j < WORLD_HEIGHT/2; j++) {
				hexArray[i + WORLD_WIDTH/2, j + WORLD_HEIGHT/2] = placeHex (i, j);
			}
		}
		GameObject rootHexObject = new GameObject ();
		root = rootHexObject.AddComponent<Hex> ();
		root.transform.position = new Vector3 (0, -Mathf.Sqrt(3)/4f, 0);
		root.rootInit (0, Mathf.Sqrt(3)/4f, this);

		GameObject branchObject = new GameObject ();
		branchObject.AddComponent<LineRenderer> ();
		airRootBranch = branchObject.AddComponent<Branch> ();
		airRootBranch.init (root, this);
		root.addBranch( hexArray[WORLD_WIDTH / 2, WORLD_HEIGHT / 2], airRootBranch);
		airRootBranch.confirm(hexArray[WORLD_WIDTH/2,WORLD_HEIGHT/2]);

		GameObject branchObject2 = new GameObject ();
		branchObject2.AddComponent<LineRenderer> ();
		groundRootBranch = branchObject2.AddComponent<Branch> ();
		groundRootBranch.init (root, this);
		root.addBranch( hexArray[WORLD_WIDTH / 2, WORLD_HEIGHT / 2 - 1], groundRootBranch);
		groundRootBranch.confirm(hexArray[WORLD_WIDTH/2,WORLD_HEIGHT/2 - 1]);
	}

	Hex placeHex(int x, int y){

		int cartX = x;
		int cartY = y;

		float actX = (float)cartX * 0.75f;
		float actY = (float)cartY * Mathf.Sqrt(3)/2f;
		if (x % 2 != 0) {
			actY -= Mathf.Sqrt(3)/4f;
		}
		
		GameObject hexObject = new GameObject ();
		Hex hex = hexObject.AddComponent<Hex> ();
		hex.transform.position = new Vector3 (actX, actY, 0);
		hex.init (x, y, actX, actY, this);

        //Tried to put all the hexes in a folder, didnt work for some reason

        /*hexes.Add(hex);
        hex.name = "Hex " + hexes.Count;
        hex.transform.parent = hexFolder.transform;*/

        return hex;
		
	}

	bool checkStart(Hex start){
		return (start.occupied);
	}

	bool checkFinish(Hex start, Hex end){
		return (!end.occupied && 
			    checkAdjacent (start, end) &&
				start.type == end.type);
	}

	bool checkAdjacent(Hex start, Hex end){
		float dist = Mathf.Sqrt (Mathf.Pow (end.realX - start.realX, 2) + Mathf.Pow (end.realY - start.realY, 2));
		return (dist < 1f);
	}
		
	
    private int calculateType(int x, int y)
    {
        float wProb = getWaterProb(x, y);
        float oProb = getOrbProb(x, y);
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

    private float getOrbProb(int x, int y)
    {
        if (y == WORLD_HEIGHT - 1)
        {
            return 0;
        }
        return ORB_BASE_PROB * (1 - y * y / (WORLD_HEIGHT * WORLD_HEIGHT));
    }

    private float getWaterProb(int x, int y)
    {
        if (y == WORLD_HEIGHT - 1)
        {
            return 0;
        }
        return WATER_BASE_PROB * y / WORLD_HEIGHT;
    }

    void sunGenerator()
    {
        System.Random random = new System.Random();
        float x = random.Next(-WORLD_WIDTH/2, WORLD_WIDTH/2);
        createSun(0.75f * x, (WORLD_HEIGHT/2)*Mathf.Sqrt(3) / 2f);
    }

    void createSun(float x, float y)
    {
        GameObject lightObject = new GameObject();
        Light newLight = lightObject.AddComponent<Light>();
        newLight.init(x, y, this);

        BoxCollider2D box = lightObject.AddComponent<BoxCollider2D>();         //Colliders
        box.size = new Vector2(0.5f, 0.5f);
        lightObject.SetActive(true);
        box.isTrigger = true;

        Rigidbody2D rig = lightObject.AddComponent<Rigidbody2D>();
        rig.isKinematic = true;

        lights.Add(newLight);
        newLight.name = "Sundrop " + lights.Count;
        newLight.transform.parent = lightFolder.transform;
    }

    void addSunEnergy(float scale)
    {
        int value = (int)(scale * 10);
        sunEnergy += value;
    }

    
	void OnGUI () {
        /*if (GUI.Button (new Rect (10,10,100,30), "CALC HEIGHT")) {
			treeHeight = root2.findHeight(0);
			print (treeHeight);
		}*/
        GUI.contentColor = Color.yellow;
        GUI.Label(new Rect(Screen.width -100, Screen.height/2, 100, 20), sunDisplay);
    }
	

}

