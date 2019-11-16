using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class PointsWinManager : MonoBehaviour {

    [Header("GamoObjects")]
    public GameObject canvas; // the CANVAS!
    public GameObject pointsWindow; // the points window
    public GameObject upgradebar; // the upgrade bar
    public GameObject launchBtn;
    public GameObject touchPad;

    public GameObject doubleMoneyBtn;
    public GameObject NoConnectionMoneyBtn;

    public GameObject noConnectionContinueBtn;
    public GameObject continueGameButton;
    public GameObject gameOverWindow;

    public Text launchPointsTxt;
    public Text ptsWindowMoneyTxt; // the money we got for the points this round
    public Text bonusPointsTxt; // the money we got for the points this round

    public Text MoneyTxt; // the money in the Game Scene


    private DistanceManager distanceMan;
    private DestrucionGoal destructionGoal;

    [Header("upgrade buttons")]

    public Text powerLvlTxt;
    public Text powercostTxt;

    public Text sizeLvlTxt;
    public Text sizecostTxt;

    public Text incomeLvlTxt;
    public Text incomecostTxt;

    private upgrades upgradesScript;
    private pointsManager pointsMan;

    public Text goalLvlTxt;
    public Slider goalSlider;

    private int moneyThisRound = 0;
    private bool PointsWinShowing = false;

    private void Awake(){

        canvas.SetActive(true);
    }


    void Start () {

        distanceMan = FindObjectOfType<DistanceManager>();
        upgradesScript = FindObjectOfType<upgrades>();
        pointsMan = FindObjectOfType<pointsManager>();
        destructionGoal = FindObjectOfType<DestrucionGoal>();

        pointsWindow.SetActive(false);
        launchPointsTxt.text = "Damage: " + destructionGoal.GoalPoints + " / " + destructionGoal.currentGoal;

        RefreshUpgradeButtons();
        UpdateGoalValues();

    }


    public void RefreshUpgradeButtons(){

        powerLvlTxt.text = upgradesScript.POWERLVL.ToString();
        powercostTxt.text = ((int)upgradesScript.GetCostForLevel(upgradesScript.POWERLVL)).ToString();

        sizeLvlTxt.text = upgradesScript.SIZELVL.ToString();
        sizecostTxt.text = ((int)upgradesScript.GetCostForLevel(upgradesScript.SIZELVL)).ToString();

        incomeLvlTxt.text = upgradesScript.INCOMELVL.ToString();
        incomecostTxt.text = ((int)upgradesScript.GetCostForLevel(upgradesScript.INCOMELVL)).ToString();

        MoneyTxt.text = "Money: " + upgradesScript.MONEY;
    }

    public void HideTouchpad(){

        touchPad.transform.localScale = new Vector3(0, 0, 0);

    }

    public void CatapultView() {

        upgradebar.SetActive(true);
        launchBtn.SetActive(true);
        touchPad.transform.localScale = new Vector3(0, 0, 0);

    }

   public void BallView(){

        upgradebar.SetActive(false);
        launchBtn.SetActive(false);
        touchPad.transform.localScale = new Vector3(1, 1, 1);

    }


    // ALAO CALLED FROM GET MONEY BUTTONS
    public void UpdatePoints(){

        if (destructionGoal.GoalPoints >= destructionGoal.currentGoal) {

            destructionGoal.GoalReached();
        }
            
        UpdateGoalValues();

        launchPointsTxt.text = "Damage: " + destructionGoal.GoalPoints + " / " + destructionGoal.currentGoal;


    }

    //UPDATES THE SLIDER
    private void UpdateGoalValues(){

        goalLvlTxt.text = destructionGoal.goalLevel.ToString();

        float pointsPercentage = (float)destructionGoal.GoalPoints/ destructionGoal.currentGoal;
        goalSlider.value = pointsPercentage;

    }

    public void ShowPointsWindow(){

            if (PointsWinShowing)
            return;

        if (Advertisement.IsReady("rewardedVideo")){

            doubleMoneyBtn.SetActive(true);
            NoConnectionMoneyBtn.SetActive(false);
        }
        else
        {

            doubleMoneyBtn.SetActive(false);
            NoConnectionMoneyBtn.SetActive(true);

        }
            


        pointsWindow.SetActive(true);
        launchPointsTxt.text = "Damage: " + destructionGoal.GoalPoints + " / " + destructionGoal.currentGoal;

        moneyThisRound = upgradesScript.PointsToMoney(pointsMan.PointsThisRound);
        ptsWindowMoneyTxt.text = "Money: " + moneyThisRound;

        bonusPointsTxt.text = destructionGoal.bonusPoints.ToString() ;

        PointsWinShowing = true;

    }

    public void HidePointsWindow(){

        if (!PointsWinShowing)
            return;

        MoneyTxt.text = "Money: " + upgradesScript.MONEY; 

        pointsWindow.SetActive(false);
        launchPointsTxt.text = "Damage: " + destructionGoal.GoalPoints + " / " + destructionGoal.currentGoal;

        PointsWinShowing = false;

        UpdatePoints();
    }

    #region GET MONEY

    public void GetMoney() {

        // get money plus bonus points and set bonus points to 0
        upgradesScript.MONEY += moneyThisRound + destructionGoal.bonusPoints;
        destructionGoal.bonusPoints = 0;

        moneyThisRound = 0;
        pointsMan.HidePointsWindow();

    }

    public void GetDoubleMoney(){

        // get money plus bonus points and set bonus points to 0
        upgradesScript.MONEY += (moneyThisRound *2) + destructionGoal.bonusPoints;
        destructionGoal.bonusPoints = 0;
        moneyThisRound = 0;
        pointsMan.HidePointsWindow();
    }

    #endregion
    #region GAMEOVER

    // CALL THIS FROM WINDOW MANAGER 
    public void ShowGameOverWindow(){

        gameOverWindow.SetActive(true);

        if (Advertisement.IsReady("rewardedVideo")){

            continueGameButton.SetActive(true);
            noConnectionContinueBtn.SetActive(false);
        }   
         else
        {
            continueGameButton.SetActive(false);
            noConnectionContinueBtn.SetActive(true);
        }
            

    }

    public void RestartGame(bool video = false){

        if(video == false) { 

            moneyThisRound = 0;
            distanceMan.RestartGame(false);
        }

        else if (video == true){

            moneyThisRound = upgradesScript.PointsToMoney(pointsMan.PointsThisRound);
            upgradesScript.MONEY += moneyThisRound;
            moneyThisRound = 0;
            distanceMan.RestartGame(true);
        }

        pointsMan.HidePointsWindow();
        launchPointsTxt.text = "points: " + destructionGoal.GoalPoints + " / " + destructionGoal.currentGoal;
        gameOverWindow.SetActive(false);
        MoneyTxt.text = "Money: " + upgradesScript.MONEY;
    }


    #endregion
    #region ADS
    public void ContinueGameVideo() {
        
            var options = new ShowOptions { resultCallback = HandleContinueAdResult };
            Advertisement.Show("rewardedVideo", options);

    }

    private void HandleContinueAdResult(ShowResult result){

        switch (result){

            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                RestartGame(true);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                RestartGame(false);
                break;

            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                RestartGame(false);
                break;
        }
    }

    public void DoubleMoneyVideo() {

            var options = new ShowOptions { resultCallback = handleMoneyAdResult };
            Advertisement.Show("rewardedVideo", options);

    }

    private void handleMoneyAdResult(ShowResult result){

        switch (result){

            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                GetDoubleMoney();
                break;

            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                GetMoney();
                break;

            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                GetMoney();
                break;
        }
    }
    #endregion

}
