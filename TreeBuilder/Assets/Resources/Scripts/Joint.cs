using UnityEngine;
using System.Collections;

public class Joint : MonoBehaviour {

	public Material mat;
	public Branch branch;

	public void init(Branch b){

		branch = b;

		//transform.parent = branch.hexEnd.transform;				
		transform.localPosition = branch.hexEnd.transform.position;//new Vector3(0,0,0);		
		name = "Tree Joint Model";									

		mat = GetComponent<Renderer>().material;
		mat.shader = Shader.Find ("Transparent/Diffuse");
		mat.mainTexture = Resources.Load<Texture2D>("Textures/white_circle");	
		mat.color = branch.mat.color;//new Color (0.4f, 0.2f, 0.1f);//(0.6f,0.4f,0.3f);	
		mat.renderQueue = 5001;

		transform.localScale = new Vector3 (branch.widthEnd, branch.widthEnd, 1);

	}

	public void updateDiameter(float newDiameter){
		transform.localScale = new Vector3 (newDiameter, newDiameter, 1);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
