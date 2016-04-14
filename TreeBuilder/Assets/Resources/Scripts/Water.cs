using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	private int ADDED_WATER = 6;

	private Hex h;
	private Controller m;
	private WaterModel model;
	bool absorbed;

	// Use this for initialization
	public void init(Hex h, Controller m){
		this.h = h;
		this.m = m;
		this.transform.localPosition = h.transform.localPosition;
		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<WaterModel>();
		model.init(this);
		//this.transform.localScale = this.transform.localScale * .3f;
		h.waterInit (this);
	}

	public void payOff(){
		m.SendMessage("addWaterEnergy", ADDED_WATER);
		m.audioM.source3.PlayOneShot(m.audioM.clip3);

		absorbed = true;
	}

	void Start(){
		absorbed = false;
	}

	void Update(){
		if (absorbed) {
			this.transform.localScale *= 0.95f;
			if (this.transform.localScale.x < 0.1f) {
				Destroy (this.gameObject);
			}
		}
	}

	/*
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "joint") {
			m.SendMessage("addWaterEnergy", ADDED_WATER);
			m.audioM.source3.PlayOneShot(m.audioM.clip3);
			Destroy (this.gameObject);
			//TODO: add audio here
		}

	}
	*/
}
