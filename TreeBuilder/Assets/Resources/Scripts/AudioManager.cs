using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	Controller c;
	public AudioSource source1;
	public AudioClip clip1;

	// Use this for initialization
	void Start () {
		
	}

	public void init(Controller c){
		this.c = c;
		source1 = GetComponent<AudioSource>();
		clip1 = Resources.Load<AudioClip>("Sounds/Sun Draft");
		//source1.clip = clip1;
		source1.loop = false;
		source1.PlayOneShot(clip1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
