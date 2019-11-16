using UnityEngine;
using UnityEngine.UI;

public class DistanceManager : MonoBehaviour {

    [Header ("GameObjects")]
    public Transform ball;
    public GameObject landButton;
    private Rigidbody rb;
    private ballLauncher ballScript;
    private upgrades upgradeScript;
    private pointsManager pointsMan;
    public Text distanceText;
    public Text livesText;

    [Header("distance values")]
    public bool activeVelocity;
    private float distance = 0f;
    public float landingDistance = 100;
    private float nominalSpeed;
    public float maxSpeed = 200;
    public bool recordDistance = true;
    public float maxHeight = 20;
    private bool isBraking = false;
    
    private float excessSpeed;
    public float boostSpeedDivideFactor = 2f;
    

    [Header("braking")]
    private float brakeForce;
    public float brakeSmoothing = 10f;
    private float currentBrakeForce = 0f;
    private bool isLanding = false;
    private float brakingDistance;


    [Header("Player stats")]

    public int startingLives = 3;
    public int lives = 3; // currently used for showing a free upgrade when lives = 0
    private float maxDistance = 0f;

    [Header("effects")]
    public ParticleSystem ballTrail;
    public float particleEmitterMultiplier = 4f;
    private ParticleSystem.EmissionModule trailEmmiter;
    public float trailDeactivationVelocity = 20;
    public float trailSizeMultiplier = 0.2f;



    // Use this for initialization
    void Start () {

        lives = startingLives;
        //livesText.text = "X " + lives;
        upgradeScript = FindObjectOfType<upgrades>();
        pointsMan = FindObjectOfType<pointsManager>();
        ballScript = ball.GetComponent<ballLauncher>();
        rb = ball.GetComponent<Rigidbody>();

        nominalSpeed = ballScript.desiredVelocity;
        //maxSpeed = (nominalSpeed * boostSpeedMultiplyFactor) * mapMan.currentPower;

        trailEmmiter = ballTrail.emission;

    }

    private void FixedUpdate(){

        if (activeVelocity) {

            if (maxDistance > landingDistance && rb.useGravity == false) {

                ballTrail.gameObject.SetActive(true);

                float percentage = ball.position.z / (maxDistance -landingDistance);
                float inversedPercentage = 1 - percentage;
                float currentBoost = inversedPercentage * maxSpeed;

                if (currentBoost > 0) 
                rb.velocity = new Vector3 (rb.velocity.x, rb.velocity.y, nominalSpeed + currentBoost);

            }

            // DONT GO OVER MAX Y HEIGHT
            if (ball.position.z >= 0 && ball.position.y > maxHeight)
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            //BRAKE
            if (rb.velocity.z > ballScript.desiredVelocity && isBraking && rb.useGravity) {

                currentBrakeForce += (brakeForce) * Time.deltaTime;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y -0.1f, rb.velocity.z - currentBrakeForce);

            }

            if (rb.velocity.z <= ballScript.desiredVelocity && isBraking){

                isBraking = false;
                //ballTrail.gameObject.SetActive(false);
            }

        }
        
    }

    // Update is called once per frame
    void Update () {


        // TRAIL EFFECT
        ballTrail.transform.position = ball.position;
        trailEmmiter.rateOverTime = rb.velocity.z * particleEmitterMultiplier;

        if(rb.velocity.z < ballScript.desiredVelocity)
            ballTrail.gameObject.SetActive(false);

        float particleSize = rb.velocity.z  * trailSizeMultiplier * ball.localScale.z;

        var main = ballTrail.main;
        main.startSizeX = particleSize;
        main.startSizeY = particleSize;


        if (recordDistance && ball.position.z >= 0)
            distance = ball.position.z;

        if (rb.useGravity == false && ball.position.z >= 0 && !isLanding)
            landButton.SetActive(true);

        if (rb.useGravity == true)
            landButton.SetActive(false);

        if (distance < (maxDistance - landingDistance) && !isLanding)
            rb.useGravity = false;

        if (distance > (maxDistance - landingDistance) && !isLanding)
            LandBall();
	}

    public void ResetDistance() {

        if(distance > maxDistance)
        maxDistance = distance;

        //maxDistance = (mapMan.mapsize * mapMan.zoneLenght - 1) * mapMan.currentPower;

        distanceText.text = (int)maxDistance + " Mts";

        distance = 0f;

        recordDistance = true;
        isLanding = false;
        isBraking = false;
        currentBrakeForce = 0;
        excessSpeed = 0;
        brakeForce = 0;

        //fastSpeed = (nominalSpeed / boostSpeedMultiplyFactor) * mapMan.counter;
    }

    public void LandBall() {

        rb.useGravity = true;
        isLanding = true;

        // brake

        brakingDistance = ball.position.z + landingDistance;

        if (rb.velocity.z > ballScript.desiredVelocity) {

                excessSpeed = rb.velocity.z - ballScript.desiredVelocity;
        } else
                excessSpeed = 0;

            brakeForce = excessSpeed / brakeSmoothing;
            isBraking = true;
        

        //ballTrail.gameObject.SetActive(false);
            
    }

    public void LoseLive() {

        if (distance > maxDistance)
            maxDistance = distance;

        distanceText.text = (int)maxDistance + " Mts";

        lives--;
        //livesText.text = "X " + lives;

        if(lives == 0) {

            upgradeScript.OfferFreeUpgrade();
            lives = startingLives;

        }

    }

    #region DISCONTINUED
    public void RestartGame(bool SawAds = false) {

        lives = startingLives;
        //livesText.text = "X " + lives;

        if (!SawAds) {

            distance = 0;
            maxDistance = 0;

            print("noads!");
        }

        ResetDistance();
        upgradeScript.OfferFreeUpgrade();
    }
    #endregion


}
