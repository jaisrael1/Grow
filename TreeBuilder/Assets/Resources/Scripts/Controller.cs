using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {


	public Hex mouseOver;
	public bool placing;
	public Hex placingFrom; 
	public Hex root;
	public Hex root2;
	public Branch rootBranch;
	public Branch currentBranch;
	public int treeHeight;



	void Start () {
		populateTiles ();
		placing = false;
		treeHeight = 0;
	}
	
	// Update is called once per frame
	void Update () {

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
	}

	void populateTiles(){
		for (int i = -5; i < 6; i++) {
			for (int j = -1; j < 6; j++) {
				Hex h = placeHex (i, j);
				if (i == 0 && j == -1) {
					root = h;
					h.occupied = true;
				}
				if (i == 0 && j == 0) {
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
		
		float actX = (float)x * 0.75f;
		float actY = (float)y * Mathf.Sqrt(3)/2f;
		if (x % 2 != 0) {
			actY += Mathf.Sqrt(3)/4f;
		}
		
		GameObject hexObject = new GameObject ();
		Hex hex = hexObject.AddComponent<Hex> ();
		hex.transform.position = new Vector3 (actX, actY, 0);
		hex.init (x,y,actX,actY,this);
		return hex;
		
	}

	bool checkStart(Hex start){
		return (start.occupied && start != root);
	}

	bool checkFinish(Hex start, Hex end){
		return (!end.occupied && checkAdjacent (start, end));
	}

	bool checkAdjacent(Hex start, Hex end){
		float dist = Mathf.Sqrt (Mathf.Pow (end.realX - start.realX, 2) + Mathf.Pow (end.realY - start.realY, 2));
		return (dist < 1f);
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

