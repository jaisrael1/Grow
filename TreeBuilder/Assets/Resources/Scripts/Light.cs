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
        model.init(this, m.weather);
        this.tag = "sundrop";
        if(m.weather == 1)
        {
            this.transform.localScale = this.transform.localScale * 2;
        }
        if(m.weather == 2)
        {
            this.transform.localScale = this.transform.localScale * 0.5f;
        }
    }

    // Update is called once per frame
    void Update() {
        if (m.weather != 2) { 
            this.transform.Translate(new Vector3(0, -0.03f));
            this.transform.localScale = this.transform.localScale * 0.999f;
        }
        else
        {
            this.transform.Translate(new Vector3(0, -0.06f));
        }
        if(this.transform.localScale.x < 0.1)
        {
            Destroy(this.gameObject);
        }
	}

    // When a sundrop hits a branch collider
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "joint") {
            if (m.weather == 2)
            {
                int amount = (int)(this.transform.localScale.x * 10);
                m.SendMessage("addWaterEnergy",amount);
            }
            else {
                m.SendMessage("addSunEnergy", this.transform.localScale.x);
            }
            this.transform.localScale = this.transform.localScale * 0.7f;
			m.audioM.source1.PlayOneShot(m.audioM.clip1);
        }
    }

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground_hex") {
			this.transform.localScale *= 0.7f;
		}
		if (col.gameObject.tag == "air_hex" && col.gameObject.GetComponent<Hex> ().hasCloud) {
			this.transform.localScale *= 0.85f;
		}
	}
}
