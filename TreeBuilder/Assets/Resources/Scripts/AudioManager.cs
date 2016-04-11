using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	Controller c;
	public AudioSource source1;
	public AudioSource source2;
	public AudioClip clip1;
	public AudioClip clip2;

	// Use this for initialization
	void Start () {
		
	}

	public void init(Controller c){
		this.c = c;
		source1 = GetComponent<AudioSource>();
		source2 = GetComponent<AudioSource>();
		clip1 = Resources.Load<AudioClip>("Sounds/Sun Draft");
		clip2 = Resources.Load<AudioClip>("Sounds/Orb Draft");
		//source1.clip = clip1;
		source1.loop = false;
		source1.PlayOneShot(clip1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
