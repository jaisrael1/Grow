using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	//public AudioManager audioM;
	public AudioSource source;
	public AudioClip c;
	public bool fading;

	public void init(string filename){
		//this.audioM = audioM;

		source = this.gameObject.AddComponent<AudioSource> ();

		c = Resources.Load<AudioClip>("Sounds/"+ filename);

		source.clip = c;
	}

	public void Start(){
		fading = false;
	}

	public void play() {
		source.volume = 1f;
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
		fading = true;
	}

	// Update is called once per frame
	void Update () {
		if (fading) {
			source.volume = source.volume -= 0.05f;
			if (source.volume < 0.1f) {
				fading = false;
				source.Stop ();
			}
		}
	}
}
