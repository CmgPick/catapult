using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Advertisements;

public class upgrades : MonoBehaviour {

    public bool saveAndLoad = false;

    public int MONEY = 0;

    [Header("Upgrades")]

    public int POWERLVL = 1;
    public int SIZELVL = 1;
    public int INCOMELVL = 1;

    public GameObject[] upgradeButtons; // 0 power 1 size, 2 income

    // max levels
    public int maxSizeLvl = 50;

    //increasePerLvl
    private float sizeMultiplier = 0.2f;
    private float incomeMultiplier = 0.05f;

    private float currentCost = 0;
    private PointsWinManager pointsWinMan;
    private ballLauncher ball;
    private pointsManager pointsMan;
    private SmoothFollow camScript;
    private DistanceManager distanceManager;

    //initial values on the ball
    //private float initialPower = 0f;
    private Vector3 initialSize = new Vector3 (0,0,0);
    private float initialIncome = 0f;

    // cam follow values
    private float camInitialDistance;
    private float camInitialHeight;
    private float initialMaxHeight;
    private float initialDesiredVelocity;

    [Header("Upgrade Ads")]

    public GameObject[] freeUpgradeButtons; // 0 power 1 size, 2 income
    public List<GameObject> availableFreeUpgrades;
    public List<GameObject> upgradeButtonsToHide;
    public int upgrading = 0;


    private void Start(){

        if (saveAndLoad)
        LoadValues();

        distanceManager = FindObjectOfType<DistanceManager>();
        camScript = FindObjectOfType<SmoothFollow>();
        ball = FindObjectOfType<ballLauncher>();
        pointsWinMan = FindObjectOfType<PointsWinManager>();
        pointsMan = FindObjectOfType<pointsManager>();

        //initialPower = ball.launchForceMultiplier;
        initialSize = ball.size;
        initialIncome = pointsMan.incomeMultiplier;

        camInitialDistance = camScript.offSet.z;
        camInitialHeight = camScript.offSet.y;

        initialDesiredVelocity = ball.desiredVelocity;
        initialMaxHeight = distanceManager.maxHeight;

        UpdateMultipliers();

    }

    private void LoadValues(){

        POWERLVL = PlayerPrefs.GetInt("SAVEDPOWERLVL", POWERLVL);
        SIZELVL = PlayerPrefs.GetInt("SAVEDSIZELVL", SIZELVL);
        INCOMELVL = PlayerPrefs.GetInt("SAVEDINCOMELVL", INCOMELVL);
        MONEY = PlayerPrefs.GetInt("SAVEDMONEY", MONEY);

    }

    private void SaveValues(){

        PlayerPrefs.SetInt("SAVEDPOWERLVL", POWERLVL);
        PlayerPrefs.SetInt("SAVEDSIZELVL", SIZELVL);
        PlayerPrefs.SetInt("SAVEDINCOMELVL", INCOMELVL);
        PlayerPrefs.SetInt("SAVEDMONEY", MONEY);

    }

    public void ResetSavedValues(){

        POWERLVL = 1;
        SIZELVL = 1;
        INCOMELVL = 1;
        MONEY = 0;

        PlayerPrefs.SetInt("SAVEDPOWERLVL", POWERLVL);
        PlayerPrefs.SetInt("SAVEDSIZELVL", SIZELVL);
        PlayerPrefs.SetInt("SAVEDINCOMELVL", INCOMELVL);
        PlayerPrefs.SetInt("SAVEDMONEY", MONEY);


        UpdateMultipliers();
        pointsWinMan.RefreshUpgradeButtons();

    }


    private void UpdateMultipliers(){

        SaveValues();

    }

    public  void UpgradePower(){

        currentCost = GetCostForLevel(POWERLVL);

        if (MONEY < currentCost)
            return;

        //ball.launchForceMultiplier = initialPower + (powerMultiplier * powerLvl);

        MONEY -= (int)currentCost;
        POWERLVL++;

        UpdateMultipliers();
        pointsWinMan.RefreshUpgradeButtons();
    }

    public void UpgradeSize(){

        currentCost = GetCostForLevel(SIZELVL);

        if (MONEY < currentCost || SIZELVL >= maxSizeLvl)
            return;

        float sX = initialSize.x + (sizeMultiplier * SIZELVL);
        float sY = initialSize.x + (sizeMultiplier * SIZELVL);
        float sZ = initialSize.x + (sizeMultiplier * SIZELVL);

        ball.size = new Vector3 (sX,sY,sZ);

        MONEY -= (int)currentCost;

        ball.desiredVelocity = initialDesiredVelocity + (SIZELVL * 0.5f);

        SIZELVL++;

        UpdateMultipliers();
        pointsWinMan.RefreshUpgradeButtons();

        camScript.offSet.z = camInitialDistance - (ball.size.z - 1);
        camScript.offSet.y = camInitialHeight + (ball.size.z - 1 );

        distanceManager.maxHeight = initialMaxHeight + (ball.size.z - 1)/2;
        

    }


    public void UpgradeIncome() {

        currentCost = GetCostForLevel(INCOMELVL);

        if (MONEY < currentCost)
            return;

        pointsMan.incomeMultiplier = initialIncome + (incomeMultiplier * INCOMELVL);

        MONEY -= (int)currentCost;
        INCOMELVL++;

        UpdateMultipliers();
        pointsWinMan.RefreshUpgradeButtons();


    }

    public int PointsToMoney(int points){

        int moneyGot = (int)(points * (incomeMultiplier * INCOMELVL));
        return moneyGot;

    }

    public float GetCostForLevel (int level) {

        float initialCost = 100;
        float multiplier = 2.5f;
        float cost = 0;

        for (int i = 0; i < level; i++) {

            cost += initialCost * multiplier * level;
            //print(cost);
        }

        return cost;

    }

    private void FreeUpgradeSuccesfull(int UpgradeTarget) {

        Debug.Log("attempting to Upgrade " + UpgradeTarget);

        switch (UpgradeTarget) {

            case 0:
            default:

                Debug.LogWarning("error Upgrading");
                break;

            case 1:
                Debug.Log("Upgraded " + UpgradeTarget);
                POWERLVL++;
            break;

            case 2:
                Debug.Log("Upgraded " + UpgradeTarget);
                SIZELVL++;
            break;

            case 3:
                Debug.Log("Upgraded " + UpgradeTarget);
                INCOMELVL++;
           break;
        }

        UpdateMultipliers();
        pointsWinMan.RefreshUpgradeButtons();
        HideFreeUpgrades();

    }

    private void HideFreeUpgrades()
    {

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].SetActive(true);

        }

        for (int i = 0; i < freeUpgradeButtons.Length; i++)
        {
            freeUpgradeButtons[i].SetActive(false);

        }

        availableFreeUpgrades.Clear();
        upgradeButtonsToHide.Clear();

    }

    public void OfferFreeUpgrade() {

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].SetActive(true);
            freeUpgradeButtons[i].SetActive(false);

        }

        availableFreeUpgrades.Clear();
        upgradeButtonsToHide.Clear();

        if (Advertisement.IsReady("rewardedVideo"))
        {

            float powerCost = GetCostForLevel(POWERLVL);
            float sizeCost = GetCostForLevel(SIZELVL);
            float incomeCost = GetCostForLevel(INCOMELVL);

            if (MONEY < powerCost)
            {
                availableFreeUpgrades.Add(freeUpgradeButtons[0]);
                upgradeButtonsToHide.Add(upgradeButtons[0]);
            }


            if (MONEY < sizeCost && SIZELVL < maxSizeLvl)
            {
                availableFreeUpgrades.Add(freeUpgradeButtons[1]);
                upgradeButtonsToHide.Add(upgradeButtons[1]);
            }


            if (MONEY < incomeCost)
            {
                availableFreeUpgrades.Add(freeUpgradeButtons[2]);
                upgradeButtonsToHide.Add(upgradeButtons[2]);
            }


            // show a random free upgrade

            int randomUpgrade = Random.Range(0, availableFreeUpgrades.Count);

            for (int i = 0; i < availableFreeUpgrades.Count; i++)
            {

                if (i == randomUpgrade){

                    availableFreeUpgrades[i].SetActive(true);
                    upgradeButtonsToHide[i].SetActive(false);
                }
            }

        }
    }

    #region ADS
    public void ShowFreeUpgradeVideo(int upgradeTarget)
    {

        Debug.Log("called ShowFreeUpgradeVideo");
        upgrading = upgradeTarget;

        var options = new ShowOptions { resultCallback = HandleUpgradeAdresult};
        Advertisement.Show("rewardedVideo",options);

    }

    private void HandleUpgradeAdresult(ShowResult result)
    {
        Debug.Log("called HandleUpgradeAdresult");

        switch (result)
        {

            case ShowResult.Finished:
                Debug.Log("Upgrade AD successfully shown.");
                FreeUpgradeSuccesfull(upgrading);
                break;

            case ShowResult.Skipped:
                Debug.Log("The upgrade ad was skipped before reaching the end.");
                break;

            case ShowResult.Failed:
                Debug.LogError("The upgrade ad failed to be shown.");
                break;
        }
    }
    #endregion
}
