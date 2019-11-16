using UnityEngine.UI;
using UnityEngine;

public class BallSelector : MonoBehaviour {

    public GameObject ballSelectWindow;
    public Button ballSelectWindowBtn;
    public GameObject newBallWindow;
    public Image NewBallImage;

    public int levelsForUnlocking = 1;
    public int unlockedBalls = 0;

    public GameObject[] unlockedBallsBtns; // activates/deactivates the balls
    public Material[] ballMaterials;
    public Sprite[] ballIcons;
    
    private Renderer ballRenderer;
    private DestrucionGoal destrucionGoal;

    // Use this for initialization
    void Start () {

        destrucionGoal = FindObjectOfType<DestrucionGoal>();
        ballRenderer = FindObjectOfType<ballLauncher>().GetComponent<Renderer>();

        UnlockBalls();

        int savedBall = PlayerPrefs.GetInt("BALL", 0);

        ballRenderer.material = ballMaterials[savedBall];
        ballSelectWindowBtn.image.sprite = ballIcons[savedBall];

        ballSelectWindow.SetActive(false);
    }
	

	public void ShowNewBallWindow () {

            newBallWindow.SetActive(true);
            NewBallImage.sprite = ballIcons[unlockedBalls -1];
    }

    //call this fom the get money/double money buttons
    public void HideNewBallWindow() {

        newBallWindow.SetActive(false);
    }

    public void UnlockBalls(){

        unlockedBalls = 0;

        for (int i = 0; i < unlockedBallsBtns.Length; i++){

            if (destrucionGoal.goalLevel > i * levelsForUnlocking){

                unlockedBallsBtns[i].SetActive(true);
                unlockedBalls++;
            }
            else
                unlockedBallsBtns[i].SetActive(false);
        }

        if (unlockedBalls > 1)
            ballSelectWindowBtn.gameObject.SetActive(true);
        else
            ballSelectWindowBtn.gameObject.SetActive(false);

    }

    public void SelectBall(int selectedBall) {

        ballRenderer.material = ballMaterials[selectedBall];
        ballSelectWindowBtn.image.sprite = ballIcons[selectedBall];

        ballSelectWindow.SetActive(false);

        PlayerPrefs.SetInt("BALL", selectedBall);

    }

    public void ShowBallSelectionWindow() {

        ballSelectWindow.SetActive(true);

    }



}
