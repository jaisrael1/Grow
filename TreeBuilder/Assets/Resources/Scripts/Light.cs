using UnityEngine;
using System.Collections;

public class Light : MonoBehaviour {

    private LightModel model;
    private Controller c;
	private EnvironmentManager em;
	public int type;
	bool shrinking;

	// Use this for initialization
	public void init (float x, float y, int type, EnvironmentManager em) {
		this.em = em;
		this.c = em.c;
		this.type = type;

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
		if (type != EnvironmentManager.RAINY_WEATHER) { 
            this.transform.Translate(new Vector3(0, -0.03f));
            this.transform.localScale = this.transform.localScale * 0.999f;
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

				if (type == EnvironmentManager.RAINY_WEATHER) {
					int amount = (int)(this.transform.localScale.x * 10);
					c.SendMessage ("addWaterEnergy", amount);
				} else {
					c.SendMessage ("addSunEnergy", this.transform.localScale.x);
				}
			if (!c.audioM.flowered) {
				if(model.state == 2) {
					c.audioM.rainWater.source.volume = 0.9f;
					c.audioM.rainWater.playPitched();	
				} else if (c.enviroManager.weather == 0) {
					c.audioM.regularSun.playPitched();
				} else if (c.enviroManager.weather == 1) {
					c.audioM.sunStorm.play();
				} else {
					c.audioM.regularSun.playPitched();
				}
			}
				
            this.transform.localScale = this.transform.localScale * 0.7f;
    	}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "ground_hex") {
			this.transform.localScale *= 0.7f;
		}
		if (col.gameObject.tag == "air_hex" && col.gameObject.GetComponent<Hex> ().hasCloud && type != 2) {
			if (col.gameObject.GetComponent<Hex> ().extraTileModel.modelType == TileModel.RAIN_CLOUD_MODEL) {
				this.transform.localScale *= 0.85f;
			} else {
				this.transform.localScale *= 0.996f;
			}
		}
	}
}
