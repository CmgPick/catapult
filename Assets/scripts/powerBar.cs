using UnityEngine;
using UnityEngine.UI;

public class powerBar : MonoBehaviour {

    private Slider powerSlider;
    public float currentPower = 0f;
    private bool isIncreasing = true;
    public float powerBarSpeed = 5f;

    public int failedLaunchForce = 50;
    public int PerfectLaunchForce = 95;

    // Use this for initialization
    void Awake () {

        powerSlider = this.GetComponent<Slider>();
        currentPower = 0;
        powerSlider.value = currentPower;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        powerSlider.value = currentPower;

        //max values is 100 for 100%
        if (currentPower < 100 && isIncreasing){
            currentPower += powerBarSpeed * Time.deltaTime;
        }

        if (currentPower >= 100 && isIncreasing) {

            isIncreasing = false;
        }

        // min value is 0
        if (currentPower > 0 && !isIncreasing){

            currentPower -= powerBarSpeed * Time.deltaTime;
        }

        if (currentPower <= 0 && !isIncreasing){

            isIncreasing = true;
        }

    }
}
