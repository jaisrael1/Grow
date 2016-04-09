using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {


	public static float ORB_BASE_PROB = 10f;
	public static float WATER_BASE_PROB = 30f;

	public Hex mouseOver;
	public bool placing;
	public Hex placingFrom; 
	public Hex root;
	public Hex root2;
	public Branch rootBranch;
	public Branch currentBranch;
	public int treeHeight;

	public const int WORLD_HEIGHT = 80; // the number of vertical tiles
	public const int WORLD_WIDTH = 40;   // number of horizontal tiles 

	public Hex[,] hexArray;

	void Start () {
		populateTiles ();
		placing = false;
		treeHeight = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton("Vertical")){
			if (!(Camera.main.orthographicSize < 0.5f && Input.GetAxis("Vertical") < 0)) {
				Camera.main.orthographicSize += 0.1f * Input.GetAxis ("Vertical");
			}
		}

		Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float mouseX = worldPos.x;
		float mouseY = worldPos.y;
		worldPos.z = 0;

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

		if (Input.GetMouseButtonDown (0)) {
			/*
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			worldPos.z = 0;
			*/
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
	}

	void populateTiles(){
		hexArray = new Hex[WORLD_WIDTH, WORLD_HEIGHT];
		for (int i = -WORLD_WIDTH/2; i < WORLD_WIDTH/2; i++) {
			for (int j = -WORLD_HEIGHT/2; j < WORLD_HEIGHT/2; j++) {
				Hex h = placeHex (i, j);
				hexArray[i + WORLD_WIDTH/2, j + WORLD_HEIGHT/2] = h;
				if (i == 0 && j == 0) {
					root = h;
					h.occupied = true;
				}
				if (i == 0 && j == -1) {
					root2 = h;
					h.occupied = true;
				}
			}
		}
		GameObject branchObject = new GameObject ();
		branchObject.AddComponent<LineRenderer> ();
		rootBranch = branchObject.AddComponent<Branch> ();
		rootBranch.init (root, this);
		rootBranch.confirm (root2);
		root.addBranch (root2, rootBranch);
	}


	Hex placeHex(int x, int y){

		int cartX = x;
		int cartY = -y;

		float actX = (float)cartX * 0.75f;
		float actY = (float)cartY * Mathf.Sqrt(3)/2f;
		if (x % 2 != 0) {
			actY += Mathf.Sqrt(3)/4f;
		}
		
		GameObject hexObject = new GameObject ();
		Hex hex = hexObject.AddComponent<Hex> ();
		hex.transform.position = new Vector3 (actX, actY, 0);
		hex.init (x,y,actX,actY, 0, this);
		return hex;
		
	}

	bool checkStart(Hex start){
		return (start.occupied && start != root);
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
        float r = Random.Range(5f, 100f);
        if (wProb > r)
        {
            return 2;
        }
        r = Random.Range(5f, 100f);
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

    /*
	void OnGUI () {
		if (GUI.Button (new Rect (10,10,100,30), "CALC HEIGHT")) {
			treeHeight = root2.findHeight(0);
			print (treeHeight);
		}
	}
	*/

}

