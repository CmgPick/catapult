using UnityEngine;

public class DestrucionGoal : MonoBehaviour {

    public int GoalPoints = 0;

    public int InitialGoal = 5000;
    public int goalLevel = 1;
    public int currentGoal;
    public int bonusPoints;

    private BallSelector ballSelector;

    // Use this for initialization
    void Awake () {

        // ONLY FOR TESTING REMOVE!!
        PlayerPrefs.SetInt("BALL", 0);
        PlayerPrefs.SetInt("GOALLVL", 1);

        ballSelector = FindObjectOfType<BallSelector>();
        goalLevel = PlayerPrefs.GetInt("GOALLVL", 1);

        bonusPoints = 0;
        currentGoal = InitialGoal * goalLevel;


	}
	
	public void GoalReached () {

        bonusPoints = currentGoal / 100;

        goalLevel++;
        currentGoal = InitialGoal * goalLevel;
        PlayerPrefs.SetInt("GOALLVL", goalLevel);

        ballSelector.UnlockBalls();

        if (ballSelector.unlockedBalls >1 && goalLevel == ballSelector.levelsForUnlocking * ballSelector.unlockedBalls)
            ballSelector.ShowNewBallWindow();

        GoalPoints = 0;
    }

    // CALLED FROM GET MONEY BUTTONS
    public void ResetPoints(){

        Debug.Log("points zero");
        GoalPoints = 0;

    }




}
