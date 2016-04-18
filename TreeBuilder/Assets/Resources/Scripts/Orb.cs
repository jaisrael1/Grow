using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {

	private Hex h;
	private Controller m;
	private OrbModel model;
	public int type;
	bool absorbed;

	public void init(Hex h, int type, Controller m){
		this.h = h;
		this.m = m;
		this.type = type;
		this.transform.localPosition = h.transform.localPosition;
		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<OrbModel>();
		model.init(this);
		this.transform.localScale *= .3f;
		//this.transform.localScale = this.transform.localScale * .3f;
		h.orbInit (this);
	}

	public void payOff(){
		//TODO:  Implement orb functions.
		//m.SendMessage("addWaterEnergy", type);
		//m.audioM.source3.PlayOneShot(m.audioM.clip3);
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
}