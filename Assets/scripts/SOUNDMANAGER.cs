using UnityEngine;

public class SOUNDMANAGER : MonoBehaviour {

    // SOUNDMANAGER 1.0 script By Karel Pick

    // automatically makes this child of  Camera.main if present
    // automatically gets audio and music sources from the transform childs
    // automatically saves preferences
    //requires two childs with audiosources and with names:

    //soundsSource transform NAME = SoundsSource
    //musicSource transform NAME = MusicSource

    [Header ("Values")]
    public static bool SOUND = true;
    public static bool MUSIC = true;
    public static float VOLUME = 1.0f;

    [Header("Audiosources")]
    public AudioSource soundsSource;
    public AudioSource musicSource;

    [Header("Clips")]

    public AudioClip [] launchSound;
    public AudioClip [] destroySound;
    public AudioClip gameOverSound;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    // Use this for initialization
    void Awake () {

        LoadValues();
        GetSources();

        Camera mainCam = Camera.main;
        if (mainCam != null)
            transform.parent = mainCam.transform;
		
	}

    public void GetSources(){

        AudioSource[] audioSources = transform.GetComponentsInChildren<AudioSource>();

        //Debug.Log(audioSources.Length);

        for (int i = 0; i < audioSources.Length; i++){

            if (audioSources[i].name == "SoundsSource")
                soundsSource = audioSources[i];

            if (audioSources[i].name == "MusicSource")
                musicSource = audioSources[i];
        }  

    }

    public static void SaveValues(){

        if (SOUND)
            PlayerPrefs.SetInt("SOUNDON", 1);
        else
            PlayerPrefs.SetInt("SOUNDON", 0);

        if (MUSIC)
            PlayerPrefs.SetInt("MUSICON", 1);
        else
            PlayerPrefs.SetInt("MUSICON", 0);

        PlayerPrefs.SetFloat("VOLUMEON", VOLUME);

        Debug.Log("SOUNDMANAGER values saved");
    }

    public void LoadValues(){

        int sound = PlayerPrefs.GetInt("SOUNDON", 1);
        int music = PlayerPrefs.GetInt("MUSICON", 1);
        float volume = PlayerPrefs.GetFloat("VOLUMEON", VOLUME);

        if (sound == 0)
            SOUND = false;
        else
            SOUND = true;

        if (music == 0)
            MUSIC = false;
        else
            MUSIC = true;

        VOLUME = volume;
    }

    // here is where the actual SOUND gets Played ♪♪♪
    private void PlaySound(AudioClip soundToPlay){

        if (soundToPlay)
            soundsSource.PlayOneShot(soundToPlay, VOLUME);
        else
            Debug.LogWarning("Please add a SOUND to play");
    }

    // here is where the actual MUSIC gets Played ♪♪♪
    private void PlayMusic(AudioClip musicToPlay){

        if (musicToPlay){

            musicSource.clip = musicToPlay;
            musicSource.loop = true;
            musicSource.Play();
            musicSource.volume = VOLUME;

        }

        else
            Debug.LogWarning("Please add a MUSIC to play");
    }

    // ADD SOUNDS TO PLAY HERE
    public void PlayLaunch(){

        if (SOUND && launchSound.Length > 0){

            int randomSound = Random.Range(0, launchSound.Length);
            PlaySound(launchSound[randomSound]);
        }
    }

    public void PlayDestroy(){

        if (SOUND && destroySound.Length >0){

            int randomSound = Random.Range(0, destroySound.Length);
            PlaySound(destroySound[randomSound]);
        }
            
    }

    public void PlayGameOver(){

            PlaySound(gameOverSound);
    }

    // ADD MUSIC TO PLAY HERE
    public void PlayMenuMusic() {

        if (MUSIC)
            PlayMusic(menuMusic);

    }

    public void PlayGameMusic() {

        if (MUSIC)
            PlayMusic(gameMusic);

    }
}
