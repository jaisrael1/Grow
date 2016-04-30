using UnityEngine;
using System.Collections;

public class Branch : MonoBehaviour {

	public Material mat;
	public LineRenderer lr;
	public Hex hexStart;
	public Hex hexEnd;
	public bool placedYet;
	public Controller controller;
	Vector3 worldPos;

	public bool isRoot;

	public float widthStart;
	public float widthEnd;

	public Joint joint;
	public LeafModel leafModel;

	public Color branchColor ;
	public Color rootColor ;

	// Use this for initialization
	public void init(Hex hexStart, Controller controller, bool isRoot){
		branchColor = new Color(d(139), d(69), d(19));
		rootColor = Color.gray;
		this.isRoot = isRoot;
		this.controller = controller;
		this.hexStart = hexStart;
		placedYet = false;
		if (hexStart.coordX == null) {
			placedYet = true;
		}
		mat = gameObject.GetComponent<LineRenderer> ().material;
		mat.shader = Shader.Find("Sprites/Default");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_square");	
		if (hexStart.type == Hex.AIR) {
			mat.color = branchColor;	
		} else {
			mat.color = rootColor;
		}
		mat.renderQueue = RenderCoordinator.BRANCH_RQ;
			
		lr = gameObject.GetComponent<LineRenderer> ();
		lr.material = mat;

		lr.SetPosition (0, hexStart.transform.position);
		lr.SetPosition (1, hexStart.transform.position);
	
		widthStart = 0.085f;
		widthEnd   = 0.07f;
		lr.SetWidth (widthStart, widthEnd);

	}

	void Start(){
		//placedYet = true;
	}

	public void confirm(Hex hexEnd){
		this.hexEnd = hexEnd;
		lr.SetPosition (1, hexEnd.transform.position);
		placedYet = true;
		lr.SetWidth (widthStart, widthEnd);

		if (hexEnd.type == Hex.AIR) {
			mat.color = branchColor;
		}

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		joint = modelObject.AddComponent<Joint>();
		joint.init(this);
        modelObject.tag = "joint";
        modelObject.name = "joint";
        
        //Removing the default mesh collider so I can add my own
        DestroyImmediate(modelObject.GetComponent<MeshCollider>());

        //Adding colliders to the branch joints
        BoxCollider2D box = modelObject.AddComponent<BoxCollider2D>();         
        box.size = new Vector2(0.5f, 0.5f);
        modelObject.SetActive(true);
        box.isTrigger = true;

		if (hexEnd.coordY >= 0) {
			var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
			leafModel = modelObject2.AddComponent<LeafModel> ();
			leafModel.init (this);
		}
    }

	public void raiseWidth(){
		if (widthStart < 0.45f) {
			widthStart += 0.004f;
		}
		if (widthEnd < 0.45f) {
			widthEnd += 0.004f;
		}
		lr.SetWidth (widthStart, widthEnd);
		joint.updateDiameter (widthEnd);
	}

	// Update is called once per frame
	void Update () {
		if (!placedYet && controller != null && !isRoot) {
			worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			worldPos.z = 0;
			lr.SetPosition (1, worldPos);

			float dist = Vector3.Distance(worldPos, hexStart.transform.position);
			float width = 0.085f * (Mathf.Sqrt (3f) / 2f / dist);
			if (width > 0.3f) {
				width = 0.3f;
			}
			lr.SetWidth (width, width);
		}
	}

	float d(int v){
		return ((float)v / 255f);
	}

	public void setColor(bool isInAir){
		if (mat != null) {
			if (isInAir) {
				mat.color = branchColor;
			} else {
				mat.color = rootColor;
			}
		}
	}
}
