using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {

	private Hex h;
<<<<<<< HEAD
    //private float clock = 0;
    //private float currentTime = 0;
	private Controller c;
	private EnvironmentManager em;
=======
	private Controller m;
>>>>>>> 609317f0538ae1b6ceae340b51d417d98968150b
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
        if(type == 1 | type == 2)
        {
<<<<<<< HEAD
            //currentTime = clock;
			em.changeWeather (EnvironmentManager.SUNNY_WEATHER);
        }
        if (type == 3 | type == 4)
        {
            //currentTime = clock;
			em.changeWeather (EnvironmentManager.RAINY_WEATHER);
=======
            m.weather = 1;
            m.SendMessage("resetWeather");
            
        }
        if (type == 3 | type == 4)
        {
            m.weather = 2;
            m.SendMessage("resetWeather");
>>>>>>> 609317f0538ae1b6ceae340b51d417d98968150b
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
<<<<<<< HEAD
		/*
        clock += Time.deltaTime;
        if(clock-currentTime > 30)
        {
            c.weather = 0;
        }
*/
=======
>>>>>>> 609317f0538ae1b6ceae340b51d417d98968150b
	}
    
}