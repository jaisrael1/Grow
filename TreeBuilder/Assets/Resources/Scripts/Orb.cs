using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {

	private Hex h;
    //private float clock = 0;
    //private float currentTime = 0;
	private Controller c;
	private EnvironmentManager em;

    private OrbModel model;
	public int type;
	bool absorbed;

	public void init(Hex h, int type, EnvironmentManager em){
		this.h = h;
		this.em = em;
		this.c = em.c;
		this.type = type;
		this.transform.localPosition = h.transform.localPosition;
		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		model = modelObject.AddComponent<OrbModel>();
		model.init(this);
		this.transform.localScale *= .3f;
		h.orbInit (this);
	}

	public void payOff(){
		//TODO:  Implement orb functions.
		//m.SendMessage("addWaterEnergy", type);
		//m.audioM.source3.PlayOneShot(m.audioM.clip3);
		absorbed = true;
		if (type == 1) {
			c.createNewTree (h.realX, h.realY);
		}
        if(type == 2)
        {
            //currentTime = clock;
			em.changeWeather (EnvironmentManager.SUNNY_WEATHER);
        }
        if (type == 3 | type == 4)
        {
            //currentTime = clock;
			em.changeWeather (EnvironmentManager.RAINY_WEATHER);
        }
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