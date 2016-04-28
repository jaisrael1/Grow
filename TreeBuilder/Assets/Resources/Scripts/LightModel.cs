using UnityEngine;
using System.Collections;

public class LightModel : MonoBehaviour {

    private Light owner;
    private Material mat;
    int state = 0;

    // Use this for initialization
    public void init (Light owner, int weather) {
        this.owner = owner;
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0,0);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        if (weather != 2)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/Sundrop1");
        }
        else
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/WaterDrop");
        }
        mat.color = new Color(1, 1, 1);
		mat.renderQueue = RenderCoordinator.SUNDROP_RQ;

        InvokeRepeating("switchModel", 1f, 1f);
    }

    void switchModel()
    {
        if (state == 0)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/sundrop2");
            state = 1;
        }
        else
        {
            mat.mainTexture = Resources.Load<Texture2D>("Textures/Sundrop1");
            state = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
