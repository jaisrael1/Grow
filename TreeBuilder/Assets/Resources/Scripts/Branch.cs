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

	public float widthStart;
	public float widthEnd;

	Joint joint;

	public Color branchColor = new Color (0.95f, 0.64f, 0.37f);

	// Use this for initialization
	public void init(Hex hexStart, Controller controller){

		this.controller = controller;
		this.hexStart = hexStart;
		placedYet = false;
		if (hexStart.coordX == null) {
			placedYet = true;
		}
		mat = gameObject.GetComponent<LineRenderer> ().material;
		mat.shader = Shader.Find ("Transparent/Diffuse");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_square");	
		if (hexStart.type == Hex.AIR) {
			mat.color = branchColor;	
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
    }

	public void raiseWidth(){
		if (widthStart < 0.45f) {
			widthStart += 0.015f;
		}
		if (widthEnd < 0.45f) {
			widthEnd += 0.015f;
		}
		lr.SetWidth (widthStart, widthEnd);
		joint.updateDiameter (widthEnd);
	}

	// Update is called once per frame
	void Update () {
		if (!placedYet && controller != null) {
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
}
