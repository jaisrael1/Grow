using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour {

    private LightModel model;
    private Controller m;

	// Use this for initialization
	public void init (float x, float y, Controller m) {
        this.m = m;
        this.transform.localPosition = new Vector3(x, y);
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<LightModel>();
        model.init(this);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector3(0, -0.03f));
        this.transform.localScale = this.transform.localScale * 0.999f;
        if(this.transform.position.y < -0.5 || this.transform.localScale.x < 0.1)
        {
            Destroy(this.gameObject);
        }
	}

    // When a sundrop hits a branch collider
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "joint") {
            m.SendMessage("addSunEnergy", this.transform.localScale.x);
            this.transform.localScale = this.transform.localScale * 0.7f;
			m.audioM.source1.PlayOneShot(m.audioM.clip1);
        }
    }
}
