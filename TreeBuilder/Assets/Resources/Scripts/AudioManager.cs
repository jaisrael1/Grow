using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	Controller c;

	public bool changeSound;
	public bool maxBGM;
	public bool flowered;


	public ArrayList branchSounds;
	public ArrayList rootSounds;
	/*
	public AudioSource bgm;
	public AudioSource bgfx;
	public AudioSource branches;
	public AudioSource roots;
	public AudioSource sun;
	public AudioSource water;
	public AudioSource system;
	public AudioSource orbs;
*/

	public Sound newTree;
	public Sound newSeed;

	public Sound abovegroundBGFX;
	public Sound undergroundBGFX;

	public Sound bgm1;
	public Sound bgm2;
	public Sound bgm3;

	public Sound menuTheme;

	public Sound branch1;
	public Sound branch2;
	public Sound branch3;
	public Sound branch4;
	public Sound branch5;
	public Sound branch6;
	public Sound branch7;
	public Sound branch8;
	public Sound branch9;
	public Sound branch10;
	public Sound branch11;
	public Sound branch12;

	public Sound root1;
	public Sound root2;
	public Sound root3;
	public Sound root4;
	public Sound root5;
	public Sound root6;
	public Sound root7;
	public Sound root8;
	public Sound root9;
	public Sound root10;
	public Sound root11;
	public Sound root12;

	public Sound regularSun;
	public Sound sunStorm;

	public Sound mouseClick;

	public Sound regularWater;
	public Sound rainWater;

	// Use this for initialization
	void Start () {
		
	}

	public void init(Controller c){
		//load game controller
		this.c = c;


		changeSound = true;
		maxBGM = false;

		/*create audio source objects
		bgm = GetComponent<AudioSource>();
		bgfx = GetComponent<AudioSource>();
		branches = GetComponent<AudioSource>();
		roots = GetComponent<AudioSource>();
		sun = GetComponent<AudioSource>();
		water = GetComponent<AudioSource>();
		system = GetComponent<AudioSource>();
		orbs = GetComponent<AudioSource>();
		*/

		GameObject sound0 = new GameObject ();
		abovegroundBGFX = sound0.AddComponent<Sound> ();
		abovegroundBGFX.init ("abovegroundBGFX");

		GameObject sound1 = new GameObject ();
		undergroundBGFX = sound1.AddComponent<Sound> ();
		undergroundBGFX.init ("undergroundBGFX");

		//load audio clips
		//abovegroundBGFX = Resources.Load<Sound>("Sounds/abovegroundBGFX");
		//undergroundBGFX = Resources.Load<Sound>("Sounds/undergroundBGFX");

		GameObject sound2 = new GameObject ();
		bgm1 = sound2.AddComponent<Sound> ();
		bgm1.init ("bgm_1");

		GameObject sound3 = new GameObject ();
		bgm2 = sound3.AddComponent<Sound> ();
		bgm2.init ("bgm_2");

		GameObject sound4 = new GameObject ();
		bgm3 = sound4.AddComponent<Sound> ();
		bgm3.init ("bgm_3");

		menuTheme = new GameObject().AddComponent<Sound> ();
		menuTheme.init("menuTheme");
		menuTheme.setLoop(true);

		/*
		bgm1 = Resources.Load<Sound>("Sounds/bgm_1");
		bgm2 = Resources.Load<Sound>("Sounds/bgm_2");
		bgm3 = Resources.Load<Sound>("Sounds/bgm_3");
		*/

		GameObject sound = new GameObject ();
		branch1 = sound.AddComponent<Sound> ();
		branch1.init ("branch_1");
		branch2 = new GameObject().AddComponent<Sound> ();
		branch2.init ("branch_2");
		branch3 = new GameObject().AddComponent<Sound> ();
		branch3.init ("branch_3");
		branch4 = new GameObject().AddComponent<Sound> ();
		branch4.init ("branch_4");
		branch5 = new GameObject().AddComponent<Sound> ();
		branch5.init ("branch_5");
		branch6 = new GameObject().AddComponent<Sound> ();
		branch6.init ("branch_6");
		branch7 = new GameObject().AddComponent<Sound> ();
		branch7.init ("branch_7");
		branch8 = new GameObject().AddComponent<Sound> ();
		branch8.init ("branch_8");
		branch9 = new GameObject().AddComponent<Sound> ();
		branch9.init ("branch_9");
		branch10 = new GameObject().AddComponent<Sound> ();
		branch10.init ("branch_10");
		branch11 = new GameObject().AddComponent<Sound> ();
		branch11.init ("branch_11");
		branch12 = new GameObject().AddComponent<Sound> ();
		branch12.init ("branch_12");
		/*
		branch1 = Resources.Load<Sound>("Sounds/branch_1");
		branch2 = Resources.Load<Sound>("Sounds/branch_2");
		branch3 = Resources.Load<Sound>("Sounds/branch_3");
		branch4 = Resources.Load<Sound>("Sounds/branch_4");
		branch5 = Resources.Load<Sound>("Sounds/branch_5");
		branch6 = Resources.Load<Sound>("Sounds/branch_6");
		branch7 = Resources.Load<Sound>("Sounds/branch_7");
		branch8 = Resources.Load<Sound>("Sounds/branch_8");
		branch9 = Resources.Load<Sound>("Sounds/branch_9");
		branch10 = Resources.Load<Sound>("Sounds/branch_10");
		branch11 = Resources.Load<Sound>("Sounds/branch_11");
		branch12 = Resources.Load<Sound>("Sounds/branch_12");
		*/
		root1 = new GameObject().AddComponent<Sound> ();
		root1.init ("root_1");
		root2 = new GameObject().AddComponent<Sound> ();
		root2.init ("root_2");
		root3 = new GameObject().AddComponent<Sound> ();
		root3.init ("root_3");
		root4 = new GameObject().AddComponent<Sound> ();
		root4.init ("root_4");
		root5 = new GameObject().AddComponent<Sound> ();
		root5.init ("root_5");
		root6 = new GameObject().AddComponent<Sound> ();
		root6.init ("root_6");
		root7 = new GameObject().AddComponent<Sound> ();
		root7.init ("root_7");
		root8 = new GameObject().AddComponent<Sound> ();
		root8.init ("root_8");
		root9 = new GameObject().AddComponent<Sound> ();
		root9.init ("root_9");
		root10 = new GameObject().AddComponent<Sound> ();
		root10.init ("root_10");
		root11 = new GameObject().AddComponent<Sound> ();
		root11.init ("root_11");
		root12 = new GameObject().AddComponent<Sound> ();
		root12.init ("root_12");
		/*
		root1 = Resources.Load<Sound>("Sounds/root_1");
		root2 = Resources.Load<Sound>("Sounds/root_2");
		root3 = Resources.Load<Sound>("Sounds/root_3");
		root4 = Resources.Load<Sound>("Sounds/root_4");
		root5 = Resources.Load<Sound>("Sounds/root_5");
		root6 = Resources.Load<Sound>("Sounds/root_6");
		root7 = Resources.Load<Sound>("Sounds/root_7");
		root8 = Resources.Load<Sound>("Sounds/root_8");
		root9 = Resources.Load<Sound>("Sounds/root_9");
		root10 = Resources.Load<Sound>("Sounds/root_10");
		root11 = Resources.Load<Sound>("Sounds/root_11");
		root12 = Resources.Load<Sound>("Sounds/root_12");
		*/

		regularSun = new GameObject().AddComponent<Sound> ();
		regularSun.init ("regularSun");

		sunStorm = new GameObject().AddComponent<Sound> ();
		sunStorm.init ("sunStorm");

		//regularSun = Resources.Load<Sound>("Sounds/regularSun");
		//sunStorm = Resources.Load<Sound>("Sounds/sunStorm");

		mouseClick = new GameObject().AddComponent<Sound> ();
		mouseClick.init ("mouseClick");

		//mouseClick = Resources.Load<Sound>("Sounds/mouseClick");

		regularWater = new GameObject ().AddComponent<Sound> ();
		regularWater.init ("regularWater");

		rainWater = new GameObject ().AddComponent<Sound> ();
		rainWater.init ("rainWater");

		newSeed = new GameObject().AddComponent<Sound> ();
		newSeed.init ("newSeed");
		newSeed.setLoop(true);

		newTree = new GameObject().AddComponent<Sound> ();
		newTree.init ("newTree");

		//regularWater = Resources.Load<Sound>("Sounds/regularWater");

		//loads branch + root sfx into the array TODO
		branchSounds = new ArrayList ();
		branchSounds.Add(branch1);
		branchSounds.Add(branch2);
		branchSounds.Add(branch3);
		branchSounds.Add(branch4);
		branchSounds.Add(branch5);
		branchSounds.Add(branch6);
		branchSounds.Add(branch7);
		branchSounds.Add(branch8);
		branchSounds.Add(branch9);
		branchSounds.Add(branch10);
		branchSounds.Add(branch11);
		branchSounds.Add(branch12);

		rootSounds = new ArrayList ();
		rootSounds.Add(root1);
		rootSounds.Add(root2);
		rootSounds.Add(root3);
		rootSounds.Add(root4);
		rootSounds.Add(root5);
		rootSounds.Add(root6);
		rootSounds.Add(root7);
		rootSounds.Add(root8);
		rootSounds.Add(root9);
		rootSounds.Add(root10);
		rootSounds.Add(root11);
		rootSounds.Add(root12);


		mouseClick.setLoop (false);

		//starts bgm
		bgm1.setLoop(true);
		bgm2.setLoop(true);
		bgm3.setLoop(true);

		//Scene scene = SceneManager.GetActiveScene ();
		bgm1.play ();

		abovegroundBGFX.setLoop (true);
		abovegroundBGFX.play ();

		undergroundBGFX.setLoop (true);
		undergroundBGFX.play ();
	}

	public void randomBranch () {
		int randomBranch = Random.Range(0, branchSounds.Count);
		Sound s = (Sound) branchSounds[randomBranch];
		s.play ();
	}

	public void randomRoot () {
		int randomRoot = Random.Range(0, rootSounds.Count);
		Sound s = (Sound) rootSounds[randomRoot];
		s.play ();
	}

	// Update is called once per frame
	void Update () {
		if (!maxBGM) {
			if (c.trees.Count == 2 && changeSound) {
				print("changing to bgm2");
				//bgm.clip = bgm2;
				bgm1.stop();
				changeSound = false;
				bgm2.play ();
			}

			else if (c.trees.Count > 3) {
				print("changing to bgm3");
				//bgm.clip = bgm3;
				bgm2.stop();
				bgm3.play ();
				maxBGM = true;
			}
		}

		if (flowered) {
			bgm1.stop();
			bgm2.stop();
			bgm3.stop();
			menuTheme.play();
			flowered = false;
		}

		abovegroundBGFX.source.volume = 0.9f * Camera.main.transform.position.y;//Camera.main.ScreenToWorldPoint (Input.mousePosition).y;
		undergroundBGFX.source.volume = -0.2f *  Camera.main.transform.position.y;//Camera.main.ScreenToWorldPoint (Input.mousePosition).y;

		//if (Input.GetMouseButtonDown(0)) {
		//	system.Play();
		//}
	}

}
