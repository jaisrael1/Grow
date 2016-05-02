using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	Controller c;

	public bool changeSound;
	public bool maxBGM;


	public ArrayList branchSounds;
	public ArrayList rootSounds;

	public AudioSource bgm;
	public AudioSource bgfx;
	public AudioSource branches;
	public AudioSource roots;
	public AudioSource sun;
	public AudioSource water;
	public AudioSource system;
	public AudioSource orbs;


	public AudioClip abovegroundBGFX;
	public AudioClip undergroundBGFX;

	public AudioClip bgm1;
	public AudioClip bgm2;
	public AudioClip bgm3;


	public AudioClip branch1;
	public AudioClip branch2;
	public AudioClip branch3;
	public AudioClip branch4;
	public AudioClip branch5;
	public AudioClip branch6;
	public AudioClip branch7;
	public AudioClip branch8;
	public AudioClip branch9;
	public AudioClip branch10;
	public AudioClip branch11;
	public AudioClip branch12;

	public AudioClip root1;
	public AudioClip root2;
	public AudioClip root3;
	public AudioClip root4;
	public AudioClip root5;
	public AudioClip root6;
	public AudioClip root7;
	public AudioClip root8;
	public AudioClip root9;
	public AudioClip root10;
	public AudioClip root11;
	public AudioClip root12;

	public AudioClip regularSun;
	public AudioClip sunStorm;

	public AudioClip mouseClick;

	public AudioClip regularWater;

	// Use this for initialization
	void Start () {
		
	}

	public void init(Controller c){
		//load game controller
		this.c = c;

		changeSound = true;
		maxBGM = false;

		//create audio source objects
		bgm = GetComponent<AudioSource>();
		bgfx = GetComponent<AudioSource>();
		branches = GetComponent<AudioSource>();
		roots = GetComponent<AudioSource>();
		sun = GetComponent<AudioSource>();
		water = GetComponent<AudioSource>();
		system = GetComponent<AudioSource>();
		orbs = GetComponent<AudioSource>();

		//load audio clips
		abovegroundBGFX = Resources.Load<AudioClip>("Sounds/abovegroundBGFX");
		undergroundBGFX = Resources.Load<AudioClip>("Sounds/undergroundBGFX");

		bgm1 = Resources.Load<AudioClip>("Sounds/bgm_1");
		bgm2 = Resources.Load<AudioClip>("Sounds/bgm_2");
		bgm3 = Resources.Load<AudioClip>("Sounds/bgm_3");

		branch1 = Resources.Load<AudioClip>("Sounds/branch_1");
		branch2 = Resources.Load<AudioClip>("Sounds/branch_2");
		branch3 = Resources.Load<AudioClip>("Sounds/branch_3");
		branch4 = Resources.Load<AudioClip>("Sounds/branch_4");
		branch5 = Resources.Load<AudioClip>("Sounds/branch_5");
		branch6 = Resources.Load<AudioClip>("Sounds/branch_6");
		branch7 = Resources.Load<AudioClip>("Sounds/branch_7");
		branch8 = Resources.Load<AudioClip>("Sounds/branch_8");
		branch9 = Resources.Load<AudioClip>("Sounds/branch_9");
		branch10 = Resources.Load<AudioClip>("Sounds/branch_10");
		branch11 = Resources.Load<AudioClip>("Sounds/branch_11");
		branch12 = Resources.Load<AudioClip>("Sounds/branch_12");

		root1 = Resources.Load<AudioClip>("Sounds/root_1");
		root2 = Resources.Load<AudioClip>("Sounds/root_2");
		root3 = Resources.Load<AudioClip>("Sounds/root_3");
		root4 = Resources.Load<AudioClip>("Sounds/root_4");
		root5 = Resources.Load<AudioClip>("Sounds/root_5");
		root6 = Resources.Load<AudioClip>("Sounds/root_6");
		root7 = Resources.Load<AudioClip>("Sounds/root_7");
		root8 = Resources.Load<AudioClip>("Sounds/root_8");
		root9 = Resources.Load<AudioClip>("Sounds/root_9");
		root10 = Resources.Load<AudioClip>("Sounds/root_10");
		root11 = Resources.Load<AudioClip>("Sounds/root_11");
		root12 = Resources.Load<AudioClip>("Sounds/root_12");

		regularSun = Resources.Load<AudioClip>("Sounds/regularSun");
		sunStorm = Resources.Load<AudioClip>("Sounds/sunStorm");

		mouseClick = Resources.Load<AudioClip>("Sounds/mouseClick");

		regularWater = Resources.Load<AudioClip>("Sounds/regularWater");


		//loads branch + root sfx into the array TODO
		branchSounds = new ArrayList ();
		branchSounds.Add(c.audioM.branch1);
		branchSounds.Add(c.audioM.branch2);
		branchSounds.Add(c.audioM.branch3);
		branchSounds.Add(c.audioM.branch4);
		branchSounds.Add(c.audioM.branch5);
		branchSounds.Add(c.audioM.branch6);
		branchSounds.Add(c.audioM.branch7);
		branchSounds.Add(c.audioM.branch8);
		branchSounds.Add(c.audioM.branch9);
		branchSounds.Add(c.audioM.branch10);
		branchSounds.Add(c.audioM.branch11);
		branchSounds.Add(c.audioM.branch12);

		rootSounds = new ArrayList ();
		rootSounds.Add(c.audioM.root1);
		rootSounds.Add(c.audioM.root2);
		rootSounds.Add(c.audioM.root3);
		rootSounds.Add(c.audioM.root4);
		rootSounds.Add(c.audioM.root5);
		rootSounds.Add(c.audioM.root6);
		rootSounds.Add(c.audioM.root7);
		rootSounds.Add(c.audioM.root8);
		rootSounds.Add(c.audioM.root9);
		rootSounds.Add(c.audioM.root10);
		rootSounds.Add(c.audioM.root11);
		rootSounds.Add(c.audioM.root12);


		system.loop = false;
		system.clip = mouseClick;

		//starts bgm
		bgm.loop = true;
		bgm.clip = bgm1;
		bgm.Play();


	
	}

	public AudioClip randomBranch () {
		int randomBranch = Random.Range(0, branchSounds.Count);
		return (AudioClip) branchSounds[randomBranch];
	}

	public AudioClip randomRoot () {
		int randomRoot = Random.Range(0, rootSounds.Count);
		return (AudioClip) rootSounds[randomRoot];
	}

	// Update is called once per frame
	void Update () {
		if (!bgm.isPlaying) {
			bgm.Play();
		}
		if (!maxBGM) {
			if (c.trees.Count == 2 && changeSound || c.trees.Count == 3 && changeSound) {
				print("changing to bgm2");
				bgm.clip = bgm2;
				changeSound = false;
			}

			else if (c.trees.Count > 3) {
				print("changing to bgm3");
				bgm.clip = bgm3;
				maxBGM = true;
			}
		}

		//if (Input.GetMouseButtonDown(0)) {
		//	system.Play();
		//}
	}

}
