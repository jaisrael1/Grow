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
	public TileModel extraTileModel;

	public ArrayList hex_edges;
	public ArrayList branches_leaving;
	public Hex hexFrom;
	public Branch branchEntering;
	public Tree tree;

	//loads branch/root sounds
	public ArrayList branchSounds;
	public ArrayList rootSounds;

	//type variables
	public const int GROUND = 1;
	public const int AIR = 2;

	//contains variables
	public const int NOTHING = 5;
	public const int WATER_SINGLE = 6;
	public const int WATER_SPRING = 7;
	public const int CLOUD = 8;
	public const int ORB = 9; //probably could be expanded
	public bool hasCloud;
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
		hasCloud = false;

		collider = this.gameObject.AddComponent<CircleCollider2D> ();
		collider.radius = Mathf.Sqrt (3) / 4f - 0.05f;
		collider.isTrigger = true;

		var modelObject = GameObject.CreatePrimitive (PrimitiveType.Quad);
		model = modelObject.AddComponent<HexModel> ();
		model.init (this);	

		hex_edges = new ArrayList ();
		branches_leaving = new ArrayList ();
		occupied = false;


		//rootSounds = new ArrayList (controller.audioM.root1, controller.audioM.root2, controller.audioM.root3, 
		//	controller.audioM.root4, controller.audioM.root5, controller.audioM.root6, controller.audioM.root7,
	//		controller.audioM.root8, controller.audioM.root9, controller.audioM.root10, controller.audioM.root11,
	//		controller.audioM.root12);

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
		bool inBaseCase = false;
		foreach (Tree i in controller.trees) {
			if (hexFrom.Equals(i.root)) {
				inBaseCase = true;
			}
		}
		if (inBaseCase)
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

		//plays grow sound
		if (controller.initialized) {
			//controller.audioM.branches.PlayOneShot(controller.audioM.randomBranch());
			if (coordY >= 0) {
				controller.audioM.randomBranch ();
			} else {
				controller.audioM.randomRoot ();
			}
		}
		hexTo.tree = this.tree;
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

	public void makeLeaf(){

		int d = findDistToRoot ();
		branchEntering.leafModel.setScale (d);

		if (hexFrom != null && hexFrom.branchEntering.leafModel != null && hexFrom.branchEntering.leafModel.targetScale < 0.8f) {
			hexFrom.branchEntering.leafModel.shrinking = true;
		} else {
			hexFrom.updateLeaf ();
		}
	}

	public void updateWidth ()
	{
		if (hexFrom != null) {
			branchEntering.raiseWidth ();
			hexFrom.updateWidth ();
		}
	}

	public void updateLeaf()
	{
			if (branchEntering != null && branchEntering.leafModel != null) {
				branchEntering.leafModel.changeScale ();
			}
		if (branchEntering != null && branchEntering.leafModel != null) {
			print (branchEntering.leafModel.shrinking);
		}
			if (hexFrom != null) {
				hexFrom.updateLeaf ();
			}
	}

	public int findDistToRoot(){
		if (hexFrom != null) {
			return 1 + hexFrom.findDistToRoot ();
		} else {
			return 1;
		}
	}
	/*
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
	*/
		

	void OnMouseEnter ()
	{
		controller.mouseOver = this;
		if (controller.placing && !controller.placingFrom.Equals(this)) {
			if (((coordY >= 0 && controller.waterEnergy >= controller.currentCost) ||
				(coordY < 0 && controller.sunEnergy >= controller.currentCost)) 
				&& controller.checkAdjacent(controller.placingFrom, this)
				&& this.type == controller.placingFrom.type && !occupied) {
				model.mat.color = new Color (0.1f, 1f, 0.1f, 1f);
			} else {
				model.mat.color = new Color (1f, 0.2f, 0.2f, 1f);
			}
		} else {
			model.mat.color = new Color (0, 0, 0, 0.5f);
		}
		model.mat.renderQueue = RenderCoordinator.HEX_BORDER_A_RQ;
	}

	void OnMouseExit ()
	{
		if (controller.mouseOver.Equals (this)) {
			controller.mouseOver = null;
		}
		model.mat.color = new Color (0, 0, 0, 0.1f);
		if (coordY >= 0) {
			model.mat.renderQueue = RenderCoordinator.HEX_BORDER_NA_A_RQ;
		} else {
			model.mat.renderQueue = RenderCoordinator.HEX_BORDER_NA_G_RQ;
		}
	}

	public void addCloud(bool isRainy){
		if (type == AIR && !hasCloud) {
			hasCloud = true;
			var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
			extraTileModel= modelObject2.AddComponent<TileModel> ();
			if (isRainy) {
				extraTileModel.init (this, TileModel.RAIN_CLOUD_MODEL);
			} else {
				extraTileModel.init (this, TileModel.CLOUD_MODEL);
			}
		}
	}

	public void removeCloud(){
		if (type == AIR && hasCloud) {
			hasCloud = false;
			Destroy (extraTileModel.gameObject);
		}
	}
	public void removeCloudByShrinking(){
		if (type == AIR && hasCloud) {
			hasCloud = false;
			extraTileModel.shrink ();
		} else {
			//print (hasCloud);
		}
	}
}
