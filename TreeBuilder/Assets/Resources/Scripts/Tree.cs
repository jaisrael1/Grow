using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {

	public int stage;
	public const int CAMERA_CONTROL_STAGE = 10; //the user has just picked up the seed orb and the camera is moving into position
	public const int FLOAT_STAGE = 11; //the seed is floating to its appropriate location
	public const int GROW_STAGE = 12; // the root and stem are expanding
	public const int DONE_STAGE = 13; // we're done initializing
	public float clock;
	public Controller c;
	public Hex root;
	public Branch airRootBranch;
	public Branch groundRootBranch;
	public float destX;
	public float destY;
	public int coordX;

	public Color branchColor;
	public Color rootColor;
	public Color leafColor;

	public float saplingHeight;
	public SeedModel seed;

	public void init(Controller c, float startX, float startY, float destX, float destY, int coordX){
		this.transform.position = new Vector3 (startX, startY, 0);
		this.c = c;
		this.destX = destX;
		this.destY = destY;
		this.coordX = coordX;
		c.inControl = false;

		var seedObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		seed = seedObject.AddComponent<SeedModel> ();
		seed.init (this);

		saplingHeight = 0f;

		int i = Random.Range (0, c.branchColors.Count);
		branchColor = c.branchColors[i];
		leafColor   = c.leafColors[i];
		rootColor   = c.rootColors[i];

	}

	void Start () {
		stage = CAMERA_CONTROL_STAGE;
	}

	// Update is called once per frame
	void Update () {
		if (stage == CAMERA_CONTROL_STAGE) {
			if (Camera.main.orthographicSize > 2.1f) {
				Camera.main.orthographicSize -= 0.05f;
			}
			else if (Camera.main.orthographicSize < 2f) {
				Camera.main.orthographicSize += 0.05f;
			}

			Camera.main.transform.position = Vector3.MoveTowards (Camera.main.transform.position, vectorize(this.transform.position), 0.1f);

			if (Camera.main.orthographicSize > 1.9f && Camera.main.orthographicSize < 2.1f
				&& Vector3.Distance(Camera.main.transform.position, vectorize(this.transform.position)) < 0.2f) {
				stage = FLOAT_STAGE;
			}

		} else if (stage == FLOAT_STAGE) {
			
			this.transform.position = Vector3.MoveTowards (this.transform.position, new Vector3 (destX, destY, 0), 0.05f);
			Camera.main.transform.position = vectorize(this.transform.position);
			/*
			if (c.currentBranch != null) {
				Destroy (c.currentBranch.gameObject);
				c.placing = false;
			}
			*/
			if (Vector3.Distance (this.transform.position, new Vector3 (destX, destY, 0)) < 0.1f) {
				stage = GROW_STAGE;
				GameObject rootHexObject = new GameObject ();
				root = rootHexObject.AddComponent<Hex> ();
				root.tree = this;
				root.transform.position = new Vector3 (destX, -Mathf.Sqrt (3) / 4f, 0);
				root.rootInit (destX, destY, c);


				GameObject branchObject = new GameObject ();
				branchObject.AddComponent<LineRenderer> ();
				airRootBranch = branchObject.AddComponent<Branch> ();
				airRootBranch.init (root, c, true);
				airRootBranch.setColor (true);

				GameObject branchObject2 = new GameObject ();
				branchObject2.AddComponent<LineRenderer> ();
				groundRootBranch = branchObject2.AddComponent<Branch> ();
				groundRootBranch.init (root, c, true);
				groundRootBranch.setColor (false);

				Destroy (seed.gameObject);
				/*
				if (coordX == Controller.WORLD_WIDTH / 2 - 1) {
					c.enviroManager.removeNewTreeOrbs ();
				}
				*/
			}

		} else if (stage == GROW_STAGE) {
			saplingHeight += 0.002f;
			airRootBranch.lr.SetPosition (1, new Vector3(destX, -Mathf.Sqrt(3)/4 + saplingHeight, 0));
			groundRootBranch.lr.SetPosition (1, new Vector3(destX, -Mathf.Sqrt(3)/4  - saplingHeight, 0));

			if (saplingHeight > Mathf.Sqrt (3f) / 4f) {

				root.addBranch (c.hexAt(coordX, 0), airRootBranch);
				airRootBranch.confirm (c.hexAt(coordX, 0));

				root.addBranch (c.hexAt(coordX, -1), groundRootBranch);
				groundRootBranch.confirm (c.hexAt(coordX, -1));

				c.inControl = true;
				stage = DONE_STAGE;
			}
		}
	}
	//returns a vector for the camera to be at
	public Vector3 vectorize(Vector3 baseV){
		return new Vector3(baseV.x, baseV.y, Camera.main.transform.position.z);
	}

}
//0, math.sqrt(3)/4 for first root coords