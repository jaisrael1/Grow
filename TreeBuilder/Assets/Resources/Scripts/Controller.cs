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
	public int currentCost;
    GameObject hexFolder;

	public Tree currentTree;
	public List<Tree> trees;
	public int farthestRight;
	public int farthestLeft;
	public bool inControl;
	//List<GameObject> hexes;

	public const int COST_LIN = 1;
	public const float COST_EX = 1.0f;

	public const int INITIAL_SUN_ENERGY = 50;
	public const int INITIAL_WATER_ENERGY = 50;

	public List<Color> branchColors;
	public List<Color> leafColors;
	public List<Color> rootColors;
	public int colorsArrayIndex;

	public bool flowering = false;

	public int sunEnergy;
    string sunDisplay;
	public int waterEnergy;
	string waterDisplay;
    public Texture2D resourceBackground;

	public AudioManager audioM;
	public GameObject audioObject;

    public const int WORLD_HEIGHT = 72; // the number of vertical tiles
	public const int WORLD_WIDTH = 100;  // number of horizontal tiles 
	public bool[] availableRoots;
	public Hex[,] hexArray;

    private float clock = 0;
    private float currentTime = 0;

    void Start () {
		initialized = false;
		inControl = false;
		initTreeColors ();

		hexFolder = new GameObject();
		populateTiles ();
		placing = false;

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

		sunEnergy = INITIAL_SUN_ENERGY;
		waterEnergy = INITIAL_WATER_ENERGY;
        resourceBackground = Resources.Load<Texture2D>("Textures/ResouceBackdrop");
        //resourceBackground.Resize(512, 512);
        initialized = true;

		trees = new List<Tree> ();
		GameObject treeObject = new GameObject ();
		currentTree = treeObject.AddComponent<Tree> ();
		currentTree.init (this, 0, 1.398f, 0, -Mathf.Sqrt (3f) / 4f, 0);
		trees.Add (currentTree);

		availableRoots = new bool[WORLD_WIDTH];
		for (int i = 0; i < availableRoots.Length; i++) {
			availableRoots [i] = (i % 2 == 0);
		}
		availableRoots [WORLD_WIDTH / 2] = false;


    }
	
	// Update is called once per frame
	void Update () {
		if (inControl) {
			//CAMERA STUFF
			if (Input.GetKey (KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0) {
				if (!(Camera.main.orthographicSize < 0.5f && Input.GetAxis ("Vertical") < 0)) {
					Camera.main.orthographicSize += 0.1f;
				}
			}
			if (Input.GetKey (KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0) {
				if (!(Camera.main.orthographicSize < 1f)) {
					Camera.main.orthographicSize -= 0.1f;
				}
			}
			if ((Input.mousePosition.x > Camera.main.pixelWidth * 9f / 10f && Input.mousePosition.x < Camera.main.pixelWidth) | Input.GetKey (KeyCode.D)) {
				Camera.main.transform.Translate (new Vector3 (0.1f, 0, 0));
			}
			if ((Input.mousePosition.x < Camera.main.pixelWidth / 10f && Input.mousePosition.x > 0) | Input.GetKey (KeyCode.A)) {
				Camera.main.transform.Translate (new Vector3 (-0.1f, 0, 0));
			}
			if ((Input.mousePosition.y > Camera.main.pixelHeight * 9f / 10f && Input.mousePosition.y < Camera.main.pixelHeight) | Input.GetKey (KeyCode.W)) {
				Camera.main.transform.Translate (new Vector3 (0, 0.1f, 0));
			}
			if ((Input.mousePosition.y < Camera.main.pixelHeight / 10f && Input.mousePosition.y > 0) | Input.GetKey (KeyCode.S)) {
				Camera.main.transform.Translate (new Vector3 (0, -0.1f, 0));
			}

			//BRANCH-DRAWING STUFF
			if (Input.GetMouseButtonDown (0)) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				float mouseX = worldPos.x;
				float mouseY = worldPos.y;
				worldPos.z = 0;
				/*
				if (mouseOver != null) {
					print (mouseOver.coordX + " " + mouseOver.coordY);
				}
*/
				if (!placing) {
					if (mouseOver != null && checkStart (mouseOver)) {
						placingFrom = mouseOver;
						placing = true;

						GameObject branchObject = new GameObject ();
						branchObject.AddComponent<LineRenderer> ();
						Branch branch = branchObject.AddComponent<Branch> ();
						branch.init (placingFrom, this, false);
						currentBranch = branch;

						currentCost = findCost (placingFrom);
						print ("the cost of that would be " + currentCost);
					}
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				if (placing) {
					Hex end = mouseOver;
					if (end != null) {
						end.model.mat.color = new Color (0, 0, 0, 0.5f);
					}
					if (end != null && checkFinish (placingFrom, end)) {
						placingFrom.addBranch (end, currentBranch);
						currentBranch.confirm (end);
						end.updateWidth ();
						if (end.coordY >= 0) {
							end.makeLeaf ();
						}
						if (end.type == Hex.GROUND) {
							sunEnergy -= currentCost;
						} else {
							waterEnergy -= currentCost;
						}
						checkFarthests (end.coordX, end.coordY);
					} else {
						Destroy (currentBranch.gameObject);
					}

					currentBranch = null;
					placing = false;
					currentCost = 0;
				}
			}
		}
        sunDisplay = "Sunlight: " + sunEnergy;
		waterDisplay = "Water: " + waterEnergy;
		if (placing && placingFrom != null && placingFrom.coordY >= 0) {
			waterDisplay = "Water: " + waterEnergy + " - " + currentCost;
		}
		if (placing && placingFrom != null && placingFrom.coordY < 0) {
			sunDisplay = "Sunlight: " + sunEnergy + " - " + currentCost;
		}
	}

	void populateTiles(){
		hexArray = new Hex[WORLD_WIDTH, WORLD_HEIGHT];
		for (int i = -WORLD_WIDTH/2; i < WORLD_WIDTH/2; i++) {
			for (int j = -WORLD_HEIGHT/2; j < WORLD_HEIGHT/2; j++) {
				hexArray[i + WORLD_WIDTH/2, j + WORLD_HEIGHT/2] = placeHex (i, j);
			}
		}
		//initializeRoots ();
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

	void checkFarthests(int x, int y){
		if (y == 0 || y == -1) {
			availableRoots [x + WORLD_WIDTH / 2] = false;
		}
		bool noEmptySpaces = true;
		for (int i = 0; i < availableRoots.Length; i++) {
			if (availableRoots [i]) {
				noEmptySpaces = false;
			}
		}
		if (noEmptySpaces) {
			enviroManager.removeNewTreeOrbs ();
		}
	}

	bool checkStart(Hex start){
		return (start.occupied);
	}

	public void createNewTree(float startX, float startY, int startXCoord){
		GameObject treeObject = new GameObject ();
		currentTree = treeObject.AddComponent<Tree> ();
		int xChange = 0;
		bool assigned = false;
		while (!assigned) {
			bool tooFar = true;
			if (startXCoord + xChange < WORLD_WIDTH / 2) {
				tooFar = false;
				if (availableRoots [startXCoord + xChange + WORLD_WIDTH / 2]) {
					assigned = true;
					continue;
				}
			}
			xChange *= -1;
			if (startXCoord + xChange >= -WORLD_WIDTH / 2) {
				tooFar = false;
				if (availableRoots [startXCoord + xChange + WORLD_WIDTH / 2]) {
					assigned = true;
					continue;
				}
			}
			if (tooFar) {
				print ("error in constructing new tree");
			}
			xChange *= -1;
			xChange++;
		}
		currentTree.init(this, startX, startY, (float)(startXCoord + xChange) * 0.75f, -Mathf.Sqrt(3f)/4f, startXCoord + xChange);
		audioM.newTree.play();
		trees.Add(currentTree);
		checkFarthests (startXCoord + xChange, 0);
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

	public bool checkAdjacent(Hex start, Hex end){
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
		return Math.Min(h.findHeight () * COST_LIN * trees.Count, 100);
	}

	public void flower(){
		flowering = true;
	}
    
	void OnGUI () {
        
		GUI.contentColor = Color.yellow;
        GUI.DrawTexture(new Rect(Screen.width - 200, Screen.height / 2 - 70, 210, 160), resourceBackground);
        GUI.Label(new Rect(Screen.width -160, Screen.height/2, 160, 20), sunDisplay);
		GUI.contentColor = new Color(0, 1, 1);
        GUI.Label(new Rect(Screen.width -160, Screen.height/2-30, 160, 20), waterDisplay);
       

    }

	void initTreeColors(){
		branchColors = new List<Color> ();
		rootColors = new List<Color> ();
		leafColors = new List<Color> ();
		//tan
		branchColors.Add (new Color (d (255), d (250), d (193)));
		rootColors.Add (new Color (d(174),d(170), d(122)));
		leafColors.Add (new Color(0,d(102), 0));
		//deep
		branchColors.Add (new Color(d(71), d(42), d(12)));
		rootColors.Add (new Color(d(186), d(186), d(186)));
		leafColors.Add (new Color(d(31), d(90), d(22)));
		//rosewood
		branchColors.Add (new Color(d(152), d(109), d(67)));
		rootColors.Add (new Color(d(239), d(239), d(239)));
		leafColors.Add (new Color(d(73), d(166), d(73)));
		//tan roots
		branchColors.Add (new Color(d(115), d(113), d(51)));
		rootColors.Add (new Color(d(170), d(182), 0f));
		leafColors.Add (new Color(d(20), d(146), 0f));
		//lizard leaf
		branchColors.Add (new Color (d (63), d (60), 0f));
		rootColors.Add (new Color(d(144), d(144), d(144)));
		leafColors.Add (new Color(d(76), d(153), 0f));

		branchColors.Add (new Color(d(102), d(102), 0f));
		rootColors.Add (new Color(d(204), 1f, d(204)));
		leafColors.Add (new Color(d(128), d(255), 0f));

		colorsArrayIndex = UnityEngine.Random.Range (0, branchColors.Count);
	}

	float d(int v){
		return ((float)v / 255f);
	}
}

