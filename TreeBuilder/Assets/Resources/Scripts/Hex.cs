using UnityEngine;
using System.Collections;


public class Hex : MonoBehaviour
{

	public int coordX;
	public int coordY;
	public float realX;
	public float realY;
	public bool occupied;
	public bool isRoot;
	public CircleCollider2D collider;

	public Controller controller;
	public HexModel model;
	public TileModel tileModel;

	public ArrayList hex_edges;
	public ArrayList branches_leaving;

	public Hex hexFrom;
	public Branch branchEntering;

	//type variables
	public const int GROUND = 1;
	public const int AIR = 2;

	//contains variables
	public const int NOTHING = 5;
	public const int WATER_SINGLE = 6;
	public const int WATER_SPRING = 7;
	public const int CLOUD = 8;
	public const int ORB = 9; //probably could be expanded
	public Water w;
	public int type;
	public int contains;
	public Orb o;

	public void init (int coordX, int coordY, float realX, float realY, Controller c)
	{
		this.coordX = coordX;
		this.coordY = coordY;
		this.realX = realX;
		this.realY = realY;
		controller = c;

		contains = NOTHING;

		collider = this.gameObject.AddComponent<CircleCollider2D> ();
		collider.radius = Mathf.Sqrt (3) / 4f - 0.05f;
		collider.isTrigger = true;

		var modelObject = GameObject.CreatePrimitive (PrimitiveType.Quad);
		model = modelObject.AddComponent<HexModel> ();
		model.init (this);	

		hex_edges = new ArrayList ();
		branches_leaving = new ArrayList ();
		occupied = false;

		if (coordY < 0) { 
			type = GROUND;
			this.gameObject.tag = "ground_hex";
			var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
			tileModel = modelObject2.AddComponent<TileModel> ();
			tileModel.init (this, TileModel.GROUND_MODEL);	
		} else {
			type = AIR;
			this.gameObject.tag = "air_hex";
		}
	}

	public void rootInit (float realX, float realY, Controller c)
	{
		this.realX = realX;
		this.realY = realY;
		controller = c;
		hex_edges = new ArrayList ();
		branches_leaving = new ArrayList ();
		occupied = true;
	}

	public void waterInit(Water w){
		contains = WATER_SINGLE;
		this.w = w;
	}
	public void orbInit(Orb o){
		contains = ORB;
		this.o = o;
	}

	public int findHeight()
	{
		if (hexFrom.Equals(controller.root))
		{
			return 1;
		} else {
			return 1 + hexFrom.findHeight();
		}
	}

	public void addBranch (Hex hexTo, Branch b)
	{
		hex_edges.Add (hexTo);
		branches_leaving.Add (b);
		hexTo.hexFrom = this;
		hexTo.occupied = true;
		hexTo.branchEntering = b;
		if (controller.initialized) {
			controller.audioM.source1.PlayOneShot (controller.audioM.clip2);
		}
		hexTo.waterCheck ();
		hexTo.orbCheck ();
	}

	public void waterCheck(){
		if (contains == WATER_SINGLE && w != null) {
			w.payOff ();
			w = null;
			contains = NOTHING;
		}
	}
	public void orbCheck(){
		if(contains == ORB && o != null){
			o.payOff ();
			o = null;
			contains = NOTHING;
		}
	}

	public void updateWidth ()
	{
		if (hexFrom != null) {
			branchEntering.raiseWidth ();
			hexFrom.updateWidth ();
		}
	}

	public int findHeight (int h)
	{
		if (hex_edges.Count != branches_leaving.Count) {
			print ("Edges assigned incorrectly!");
		}
		int max = h;
		foreach (Hex i in hex_edges) {
			int thisHeight = i.findHeight (h + 1);
			if (thisHeight > max) {
				max = thisHeight;
			}
		}
		return max;
	}
		
	void OnMouseEnter ()
	{
		controller.mouseOver = this;
		model.mat.color = new Color (1, 1, 1 , 0.5f);
	}

	void OnMouseExit ()
	{
		if (controller.mouseOver.Equals (this)) {
			controller.mouseOver = null;
		}
		model.mat.color = new Color (1, 1, 1, 0.25f);
	}

	public void addCloud(){
		if (type == AIR && contains == NOTHING) {
			contains = CLOUD;
			var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
			tileModel = modelObject2.AddComponent<TileModel> ();
			tileModel.init (this, TileModel.CLOUD_MODEL);	
		}
	}

	public void removeCloud(){
		if (type == AIR && contains == CLOUD) {
			contains = NOTHING;
			Destroy (tileModel.gameObject);
		}
	}
}
