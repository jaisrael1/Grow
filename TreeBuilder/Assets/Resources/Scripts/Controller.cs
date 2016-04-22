using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

	public EnvironmentManager enviroManager;

	public bool initialized;

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
	public int currentCost;
    GameObject hexFolder;

	//List<GameObject> hexes;

	public const int COST_LIN = 1;
	public const float COST_EX = 1.1f;

    public int sunEnergy = 1000;
    string sunDisplay;

	public int waterEnergy = 1000;
	string waterDisplay;

	public AudioManager audioM;
	public GameObject audioObject;

    public const int WORLD_HEIGHT = 80; // the number of vertical tiles
	public const int WORLD_WIDTH = 50;  // number of horizontal tiles 

	public Hex[,] hexArray;

    private float clock = 0;
    private float currentTime = 0;

    void Start () {
		initialized = false;
		hexFolder = new GameObject();
		populateTiles ();
		placing = false;
		treeHeight = 0;

		audioObject = new GameObject ();
		audioObject.name = "audio manager";
		audioObject.AddComponent<AudioSource>();
		audioM = audioObject.AddComponent<AudioManager> ();
		audioM.init (this);

		var enviroManagerObject = new GameObject ();
		enviroManagerObject.name = "environment manager";
		enviroManager = enviroManagerObject.AddComponent<EnvironmentManager> ();
		enviroManager.init (this);

        //Folder to store all hexes

        hexFolder.name = "Hexes";
		//hexes = new List<GameObject>();

		sunEnergy = 1000;
		waterEnergy = 1000;
		initialized = true;

    }
	
	// Update is called once per frame
	void Update () {

		//CAMERA STUFF
		if (Input.GetKey(KeyCode.E)){
			if (!(Camera.main.orthographicSize < 0.5f && Input.GetAxis("Vertical") < 0)) {
				Camera.main.orthographicSize += 0.1f;
			}
		}
        if (Input.GetKey(KeyCode.Q))
        {
            if (!(Camera.main.orthographicSize < 1f))
            {
                Camera.main.orthographicSize -= 0.1f;
            }
        }
        if ((Input.mousePosition.x > Camera.main.pixelWidth * 9f / 10f && Input.mousePosition.x < Camera.main.pixelWidth) | Input.GetKey(KeyCode.D)) {
			Camera.main.transform.Translate(new Vector3(0.1f, 0, 0));
		}
		if ((Input.mousePosition.x < Camera.main.pixelWidth / 10f && Input.mousePosition.x > 0) | Input.GetKey(KeyCode.A)){
			Camera.main.transform.Translate (new Vector3 (-0.1f, 0, 0));
		}
		if ((Input.mousePosition.y > Camera.main.pixelHeight * 9f / 10f && Input.mousePosition.y < Camera.main.pixelHeight) | Input.GetKey(KeyCode.W)) {
			Camera.main.transform.Translate (new Vector3 (0, 0.1f, 0));
		}
		if ((Input.mousePosition.y < Camera.main.pixelHeight / 10f && Input.mousePosition.y > 0) | Input.GetKey(KeyCode.S)) {
			Camera.main.transform.Translate (new Vector3 (0, -0.1f, 0));
		}

		//BRANCH-DRAWING STUFF
		if (Input.GetMouseButtonDown (0)) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			worldPos.z = 0;

			if (mouseOver != null) {
				print (mouseOver.coordX + " " + mouseOver.coordY);
			}

			if (!placing) {
				if (mouseOver != null && checkStart (mouseOver)) {
					placingFrom = mouseOver;
					placing = true;

					GameObject branchObject = new GameObject ();
					branchObject.AddComponent<LineRenderer> ();
					Branch branch = branchObject.AddComponent<Branch> ();
					branch.init (placingFrom, this);
					currentBranch = branch;

					currentCost = findCost (placingFrom);
					print ("the cost of that would be " + currentCost);
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
					if (end.type == Hex.GROUND) {
						sunEnergy -= currentCost;
					} else {
						waterEnergy -= currentCost;
					}
				} else {
					Destroy (currentBranch.gameObject);
				}
				currentBranch = null;
				placing = false;
				currentCost = 0;
			}
		}

        sunDisplay = "Sunlight: " + sunEnergy;
		waterDisplay = "Water: " + waterEnergy;

	}

	void populateTiles(){
		hexArray = new Hex[WORLD_WIDTH, WORLD_HEIGHT];
		for (int i = -WORLD_WIDTH/2; i < WORLD_WIDTH/2; i++) {
			for (int j = -WORLD_HEIGHT/2; j < WORLD_HEIGHT/2; j++) {
				hexArray[i + WORLD_WIDTH/2, j + WORLD_HEIGHT/2] = placeHex (i, j);
			}
		}
		initializeRoots ();
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

        //hexes.Add(hexObject);
        hexObject.name = "Hex";
        hexObject.transform.parent = hexFolder.transform;

        return hex;
	}

	bool checkStart(Hex start){
		return (start.occupied);
	}
		

	bool checkFinish(Hex start, Hex end){
		if (start.type == Hex.AIR) {
			return (!end.occupied &&
			checkAdjacent (start, end) &&
			start.type == end.type &&
			waterEnergy >= currentCost);
		} else {
			return (!end.occupied &&
				checkAdjacent (start, end) &&
				start.type == end.type &&
				sunEnergy >= currentCost);
		}
	}

	bool checkAdjacent(Hex start, Hex end){
		float dist = Mathf.Sqrt (Mathf.Pow (end.realX - start.realX, 2) + Mathf.Pow (end.realY - start.realY, 2));
		return (dist < 1f);
	}
		
	public Hex hexAt(int coordX, int coordY){
		return hexArray[coordX + WORLD_WIDTH / 2, coordY + WORLD_HEIGHT / 2];
	}

    void addSunEnergy(float scale)
    {
        int value = (int)(scale * 10);
        sunEnergy += value;
    }

	void addWaterEnergy(int amount){
		waterEnergy += amount;
	}

	public int findCost(Hex h){
		return ((int)Mathf.Pow (h.findHeight (), COST_EX) * COST_LIN);
	}
    
	void OnGUI () {
        
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(Screen.width -100, Screen.height/2, 100, 20), sunDisplay);
		GUI.Label(new Rect(Screen.width -100, Screen.height/2-40, 100, 20), waterDisplay);

    }

    private void initializeRoots(){
		GameObject rootHexObject = new GameObject ();
		root = rootHexObject.AddComponent<Hex> ();
		root.transform.position = new Vector3 (0, -Mathf.Sqrt (3) / 4f, 0);
		root.rootInit (0, Mathf.Sqrt (3) / 4f, this);

		GameObject branchObject = new GameObject ();
		branchObject.AddComponent<LineRenderer> ();
		airRootBranch = branchObject.AddComponent<Branch> ();
		airRootBranch.init (root, this);
		root.addBranch (hexArray [WORLD_WIDTH / 2, WORLD_HEIGHT / 2], airRootBranch);
		airRootBranch.confirm (hexArray [WORLD_WIDTH / 2, WORLD_HEIGHT / 2]);

		GameObject branchObject2 = new GameObject ();
		branchObject2.AddComponent<LineRenderer> ();
		groundRootBranch = branchObject2.AddComponent<Branch> ();
		groundRootBranch.init (root, this);
		root.addBranch (hexArray [WORLD_WIDTH / 2, WORLD_HEIGHT / 2 - 1], groundRootBranch);
		groundRootBranch.confirm (hexArray [WORLD_WIDTH / 2, WORLD_HEIGHT / 2 - 1]);
	}

}

