using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	private int ADDED_WATER = 6;

	private Hex h;
	private Controller c;
	public EnvironmentManager em;
	private WaterModel model;
	bool absorbed;

	// Use this for initialization
	public void init(Hex h, EnvironmentManager em){
		this.em = em;
		this.h = h;
		this.c = em.c;
		this.transform.localPosition = h.transform.localPosition;
		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<WaterModel>();
		model.init(this);
		//this.transform.localScale = this.transform.localScale * .3f;
		h.waterInit (this);
	}

	public void payOff(){
		c.SendMessage("addWaterEnergy", ADDED_WATER);
		c.audioM.source3.PlayOneShot(c.audioM.clip3);
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
