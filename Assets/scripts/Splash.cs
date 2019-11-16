using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {
    public Slider sliderBar;
    public Text versionTxt;

	// Use this for initialization
	void Start () {

        StartCoroutine(LoadAsyncScene());
        versionTxt.text ="V: " + Application.version;
	}


    IEnumerator LoadAsyncScene(){

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("map1");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone){

            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            sliderBar.value = progress;
            yield return null;
        }


    }

}
