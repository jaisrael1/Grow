using UnityEngine;
using System.Collections;

public class WormModel : MonoBehaviour {

    private Worm owner;
    private Material mat;

    // Use this for initialization
    public void init(Worm owner)
    {
        this.owner = owner;
        transform.parent = owner.transform;
        transform.localPosition = new Vector3(0, 0, 0);

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.mainTexture = Resources.Load<Texture2D>("Textures/Worm");

        mat.color = new Color(1, 1, 1);
        mat.renderQueue = RenderCoordinator.ANIMAL_RQ;
    }
}
