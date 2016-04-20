﻿using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour {

    private LightModel model;
<<<<<<< HEAD
    private Controller c;
	private EnvironmentManager em;
	public int type;
	bool shrinking;

	// Use this for initialization
	public void init (float x, float y, int type, EnvironmentManager em) {
		this.em = em;
		this.c = em.c;
		this.type = type;
=======
    private Controller m;
    private int type; //0 For sun, 1 for rain

	// Use this for initialization
	public void init (float x, float y, Controller m) {
        this.m = m;
        if(m.weather == 2)
        {
            type = 1;
        }
        else
        {
            type = 0;
        }
>>>>>>> 609317f0538ae1b6ceae340b51d417d98968150b
        this.transform.localPosition = new Vector3(x, y);
        var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        model = modelObject.AddComponent<LightModel>();
		model.init(this, type);
        this.tag = "sundrop";
		if(type == EnvironmentManager.SUNNY_WEATHER)
        {
            this.transform.localScale = this.transform.localScale * 2;
        }
		if(type == EnvironmentManager.RAINY_WEATHER)
        {
            this.transform.localScale = this.transform.localScale * 0.5f;
        }
    }

	void Start(){
		shrinking = false;
	}

    // Update is called once per frame
    void Update() {
<<<<<<< HEAD
		if (type != EnvironmentManager.RAINY_WEATHER) { 
=======
        if (type == 0) { 
>>>>>>> 609317f0538ae1b6ceae340b51d417d98968150b
            this.transform.Translate(new Vector3(0, -0.03f));
            this.transform.localScale = this.transform.localScale * 0.999f;
            if(m.weather == 2)
            {
                this.transform.localScale *= 0.9f;
            }
        }
        else
        {
            this.transform.Translate(new Vector3(0, -0.06f));
        }

		if (shrinking) {
			this.transform.localScale *= 0.95f;
		}

        if(this.transform.localScale.x < 0.1)
        {
            Destroy(this.gameObject);
        }
	}

	public void shrink(){
		shrinking = true;
	}

    // When a sundrop hits a branch collider
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "joint") {
<<<<<<< HEAD
			if (em.weather == EnvironmentManager.RAINY_WEATHER)
=======
            if (type == 1)
>>>>>>> 609317f0538ae1b6ceae340b51d417d98968150b
            {
                int amount = (int)(this.transform.localScale.x * 10);
                c.SendMessage("addWaterEnergy",amount);
            }
            else {
                c.SendMessage("addSunEnergy", this.transform.localScale.x);
            }
            this.transform.localScale = this.transform.localScale * 0.7f;
			c.audioM.source1.PlayOneShot(c.audioM.clip1);
        }
    }

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground_hex") {
			this.transform.localScale *= 0.7f;
		}
		if (col.gameObject.tag == "air_hex" && col.gameObject.GetComponent<Hex> ().hasCloud && m.weather!= 2) {
			this.transform.localScale *= 0.85f;
		}
	}
}
