using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour {

    public GameObject settingsPannel;
    public GameObject deletePanel;

    public Slider volumeSlider;
    public Toggle soundToogle;

	// Use this for initialization
	void Start () {

        settingsPannel.SetActive(false);
        deletePanel.SetActive(false);
    }
	

    public void ShowSettingsPannel() {

        settingsPannel.SetActive(true);
    }

    public void ToogleDeletePanel(){

        if (deletePanel.activeSelf)
            deletePanel.SetActive(false);
        else
            deletePanel.SetActive(true);
    }

    public void SaveSettings(){

        SOUNDMANAGER.SOUND = soundToogle.isOn;
        SOUNDMANAGER.VOLUME = volumeSlider.value;
        SOUNDMANAGER.SaveValues();

        settingsPannel.SetActive(false);

    }

}
