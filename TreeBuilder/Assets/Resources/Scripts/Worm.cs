using UnityEngine;
using System.Collections;

public class Worm : MonoBehaviour {

    private WormModel model;
    private Controller c;
    private EnvironmentManager em;


    // Use this for initialization
    public void init(float x, float y, EnvironmentManager em)
    {
        this.em = em;
        this.c = em.c;
        //this.type = type;

        this.transform.localPosition = new Vector3(x, y);
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<WormModel>();
        model.init(this);

        this.transform.localScale = this.transform.localScale * 0.5f;

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x > 0.75f * Controller.WORLD_WIDTH / 2)
        {
            Destroy(this.gameObject);
        }
        this.transform.Translate(new Vector2(0.02f, 0));

    }
}
