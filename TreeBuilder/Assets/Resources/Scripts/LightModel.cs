using UnityEngine;
using System.Collections;

public class LightModel : MonoBehaviour {

    private Light owner;
    private Material mat;

    // Use this for initialization
    public void init (Light owner, int weather) {
        this.owner = owner;
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0,0);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        if (weather != 2)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/sun image");
        }
        else
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/water drop");
        }
        mat.color = new Color(1, 1, 1);
		mat.renderQueue = RenderCoordinator.SUNDROP_RQ;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
