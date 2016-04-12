using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	Controller c;
	public AudioSource source1;
	public AudioSource source2;
	public AudioSource source3;
	public AudioClip clip1;
	public AudioClip clip2;
	public AudioClip clip3;

	// Use this for initialization
	void Start () {
		
	}

	public void init(Controller c){
		this.c = c;
		source1 = GetComponent<AudioSource>();
		source2 = GetComponent<AudioSource>();
		source3 = GetComponent<AudioSource>();
		clip1 = Resources.Load<AudioClip>("Sounds/Sun Draft");
		clip2 = Resources.Load<AudioClip>("Sounds/branching");
		clip3 = Resources.Load<AudioClip>("Sounds/water");
		//add bgm
		//source4.clip = clip4;
		source1.loop = false;
		source1.PlayOneShot(clip1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
