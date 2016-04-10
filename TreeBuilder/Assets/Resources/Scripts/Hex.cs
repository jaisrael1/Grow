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
	public TileModel tileModel;

	public ArrayList hex_edges;
	public ArrayList branches_leaving;

	public Hex hexFrom;
	public Branch branchEntering;

	public const int GROUND = 1;
	public const int AIR = 2;
	public int type;
	public bool isRoot;


	public void init (int coordX, int coordY, float realX, float realY, Controller c){
		this.coordX = coordX;
		this.coordY = coordY;
		this.realX = realX;
		this.realY = realY;
		controller = c;

		if (coordY < 0) { 
			type = GROUND;
		} else {
			type = AIR;
		}

		collider = this.gameObject.AddComponent<CircleCollider2D> ();
		collider.radius = Mathf.Sqrt (3) / 4f - 0.05f;
		collider.isTrigger = true;

		var modelObject = GameObject.CreatePrimitive (PrimitiveType.Quad);
		model = modelObject.AddComponent<HexModel> ();
		model.init (this);	

		hex_edges = new ArrayList ();
		branches_leaving = new ArrayList ();
		occupied = false;

		if (type == GROUND) {
			var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
			tileModel = modelObject2.AddComponent<TileModel> ();
			tileModel.init (this);	
		}
	}

	public void rootInit(float realX, float realY, Controller c){
		this.realX = realX;
		this.realY = realY;
		controller = c;
		hex_edges = new ArrayList ();
		branches_leaving = new ArrayList ();
		occupied = true;
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
