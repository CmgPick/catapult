using UnityEngine;
using UnityEngine.SceneManagement;

public class pointsManager : MonoBehaviour {

    private DestrucionGoal destrucionGoal;
    private PointsWinManager pointsWinMan;
    private ballLauncher ball;

    public int PointsThisRound = 0;
    public float incomeMultiplier = 1;
    private int PointsToAdd = 0;
    public int pointAddSpeed = 25;


    // Use this for initialization
    void Start () {

        destrucionGoal = FindObjectOfType<DestrucionGoal>();
        ball = FindObjectOfType<ballLauncher>();
        pointsWinMan = FindObjectOfType<PointsWinManager>();

    }

    void Update(){

        if (PointsToAdd > 0 ){

            pointsWinMan.UpdatePoints();
        }
        

        if (PointsToAdd >pointAddSpeed){

            PointsThisRound += pointAddSpeed;
            destrucionGoal.GoalPoints += pointAddSpeed;
            PointsToAdd -= pointAddSpeed;
        }

        else if (PointsToAdd >0 && PointsToAdd <= pointAddSpeed){

            PointsThisRound += PointsToAdd;
            destrucionGoal.GoalPoints += PointsToAdd;
            PointsToAdd = 0;

            pointsWinMan.UpdatePoints();

        }


    }

    // Update is called once per frame
    public void addPoints (int points) {

        PointsToAdd += (int)(points * incomeMultiplier);
        
	}

    public void ShowPointsWin(){

        if (PointsToAdd > 0 ){

            PointsThisRound += PointsToAdd; // eliminates residual points
            PointsToAdd = 0;

        }

        pointsWinMan.ShowPointsWindow();
    }


    public void HidePointsWindow(){

        PointsThisRound = 0;

        ball.NewLaunch();
        pointsWinMan.HidePointsWindow();

    }

    // CALL THIS ONE INSTEAD OF WINDOWS MANAGER VERSION
    public void ShowGameOverWindow(){

        if (PointsToAdd > 0){

            PointsThisRound += PointsToAdd; // eliminates residual points
            PointsToAdd = 0;

        }

        pointsWinMan.ShowGameOverWindow();
    }

    public void HideGameOverWindow() {

        PointsThisRound = 0;
        ball.NewLaunch();
    }



    public void LoadSacene(){

        SceneManager.LoadScene(0);

    }
}
