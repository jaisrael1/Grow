using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	public AudioSource source;
	public AudioClip c;
	public bool fadingOut;
	public bool fadingIn;

	public void init(string filename)
	{
		source = this.gameObject.AddComponent<AudioSource> ();

		c = Resources.Load<AudioClip>("Sounds/"+ filename);

		source.clip = c;
	}

	public void Start(){
		fadingOut = false;
		fadingIn = false;
	}

	public void play() {
		source.Play ();
	}

	public void playPitched(){
		source.pitch = Random.Range (0.5f, 1f);
		source.Play ();
	}

	public void setLoop(bool b){
		source.loop = b;
	}

	public bool isPlaying(){
		return source.isPlaying;
	}

	public void stop(){
		source.Stop ();
	}
		
	public void fadeOut(){
		fadingOut = true;
		fadingIn = false;
	}
		
	public void fadeIn(){
		source.volume = 0f;
		source.Play ();
		fadingIn = true;
		fadingOut = false;
	}

	void Update () {
		if (fadingOut) {
			source.volume -= 0.05f;
			if (source.volume < 0.1f) {
				fadingOut = false;
				source.Stop ();
				source.volume = 1f;
			}
		}
		if (fadingIn) {
			source.volume += 0.5f;
			if (source.volume >= 1f) {
				source.volume = 1f;
				fadingIn = false;
			}
		}
	}

}
