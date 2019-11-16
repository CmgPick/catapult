using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour {

    public string[] poorLaunchMsg;
    public string[] goodLaunchMsg;
    public string[] perfectLaunchMsg;
    public string[] destructionMsg;
    public string[] softHitMsg;
    public Text messageText;

    public float messageDuration = 2f;

    private float currentTime = 0;
    private bool messageShowing = false;


    void Start(){

        currentTime = messageDuration;

    }

    public void ShowDestructionMessage() {

        messageText.text = destructionMsg[Random.Range(0, destructionMsg.Length)];
        messageShowing = true;
        currentTime = messageDuration;

    }

    public void ShowSoftHitMessage() {

        messageText.text = softHitMsg[Random.Range(0, softHitMsg.Length)];
        messageShowing = true;
        currentTime = messageDuration;

    }


    public void ShowLaunchMessage(){

        messageText.text = perfectLaunchMsg[Random.Range(0, perfectLaunchMsg.Length)];
        messageShowing = true;
        currentTime = messageDuration;
        
    }
	
	// Update is called once per frame
	void Update () {


        if(messageShowing){

            currentTime -= Time.deltaTime;
        }

        if (messageShowing && currentTime <= 0){

            currentTime = messageDuration;
            messageText.text = "";

            messageShowing = false;
           
        }

    }
}
