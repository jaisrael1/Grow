using UnityEngine;
using System.Collections;


public class Hex : MonoBehaviour {

	public int   coordX;
	public int   coordY;
	public float realX;
	public float realY;
	public bool occupied;
	public int hexType;

	public CircleCollider2D collider;

	public Controller controller;
	public HexModel model;

	public ArrayList hex_edges;
	public ArrayList branches_leaving;

	public Hex hexFrom;
	public Branch branchEntering;

	public void init (int coordX, int coordY, float realX, float realY, int hexType, Controller c){
		this.coordX = coordX;
		this.coordY = coordY;
		this.realX = realX;
		this.realY = realY;
		controller = c;
		this.hexType = hexType;

		collider = this.gameObject.AddComponent<CircleCollider2D>();
		collider.radius = Mathf.Sqrt(3)/4f - 0.05f;
		collider.isTrigger = true;

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<HexModel>();
		model.init(this);	

		hex_edges = new ArrayList ();
		branches_leaving = new ArrayList ();
		occupied = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addBranch(Hex hexTo, Branch b){
		hex_edges.Add (hexTo);
		branches_leaving.Add (b);
		hexTo.hexFrom = this;
		hexTo.occupied = true;
		hexTo.branchEntering = b;
	}

	public void updateWidth(){
		if (hexFrom != null) {
			branchEntering.raiseWidth ();
			hexFrom.updateWidth ();
		}
	}

	public int findHeight(int h){
		if (hex_edges.Count != branches_leaving.Count) {
			print ("Edges assigned incorrectly!");
		}
		int max = h;
		foreach (Hex i in hex_edges){
			int thisHeight = i.findHeight (h + 1);
			if (thisHeight > max) {
				max = thisHeight;
			}
		}
		return max;
	}


	void OnMouseEnter(){
		controller.mouseOver = this;
	}

	void OnMouseExit(){
		if (controller.mouseOver = this) {
			controller.mouseOver = null;
		}
	}
}
