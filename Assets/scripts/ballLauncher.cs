using UnityStandardAssets.CrossPlatformInput; // do not delete
using System.Collections;
using UnityEngine;

public class ballLauncher : MonoBehaviour {

    private Rigidbody rb; // the ball rigidbody
    private SmoothFollow camFollower; // the cam

    private bool canBeLaunched = true;
    private bool isLaunched = false;
    private bool currentRunEnded = false;

    private bool isGrowing = false;
    private float growSmoothing = 10;

    private pointsManager pointsMan;
    private PointsWinManager winMan;
    private Messages messages;
    private SOUNDMANAGER soundManager;
    private MapManager mapMan;
    private DistanceManager distanceManager;

    [Header("forces")]
    public bool canAccel = true;
    public float launchForceMultiplier = 50;
    public float desiredVelocity = 25f;
    public float minimumVelocity = 0.5f; // minimum velocity to reset
    public float sideMovementMultiplier = 5f;
    public bool  printVelocity = false;
    public Vector3 size = new Vector3(1, 1, 1);
    public float sideSpeedSmooth = 1f;

    [Header("effects")]

    public Animator animator;

    [Header("gameobjects")]
    public Transform launcher; // the aimer to get the direction from
    public Transform spoon; // the catapult spoon, holds the ball

    [Header("GameOver")]
    public GameObject explotionPrefab;
    public GameObject brokenBallPrefab;
    public GameObject brokenBall;
    private Renderer ballrenderer;
    private bool ballExploted = false;

    // Use this for initialization
    void Awake () {

        distanceManager = FindObjectOfType<DistanceManager>();
        mapMan = FindObjectOfType<MapManager>();
        soundManager = FindObjectOfType<SOUNDMANAGER>();
        messages = FindObjectOfType<Messages>();
        winMan = FindObjectOfType<PointsWinManager>();
        rb = this.GetComponent<Rigidbody>();
        camFollower = FindObjectOfType<SmoothFollow>();
        pointsMan = FindObjectOfType<pointsManager>();

        winMan.CatapultView();

        transform.position = spoon.position;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        ballrenderer = this.GetComponent<Renderer>();

    }

    void BallExplotion() {

        if (!ballExploted) {

            distanceManager.LoseLive();
            soundManager.PlayGameOver();
            ballrenderer.enabled = false;
            brokenBall.SetActive(true);
            Instantiate(explotionPrefab, transform.position, Quaternion.identity);
            ballExploted = true;

            rb.constraints = RigidbodyConstraints.FreezeAll;
            this.GetComponent<SphereCollider>().enabled = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }

    void ReconstructBall(){

        Destroy(brokenBall);

        ballrenderer.enabled = true;
        GameObject newBrokenBall = Instantiate(brokenBallPrefab, transform.position, Quaternion.identity);
        newBrokenBall.transform.parent = this.transform;
        newBrokenBall.SetActive(false);
        brokenBall = newBrokenBall;

        ballExploted = false;
    }

    void FixedUpdate(){

 
        if (printVelocity)
            print(rb.velocity.z);

        // accelerate the ball
        AccelBall();

        // move sideways
        SideMovement();

        // smoothly grow the ball
        if (isGrowing && transform.localScale.z < size.z && transform.position.z > 0){

            float percentage = transform.position.z / 100;
            transform.localScale += size * percentage/growSmoothing; 
        }

        if (isGrowing && transform.localScale.z >= size.z)
            isGrowing = false;

    }

    void SideMovement() {

        float sideForce = 0f;

#if UNITY_EDITOR
        if (canAccel)
           sideForce = (Input.GetAxis("Horizontal") * sideMovementMultiplier);
#else
        if(canAccel && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        sideForce = (CrossPlatformInputManager.GetAxis("Horizontal") * sideMovementMultiplier);
#endif

        float currentForce = rb.velocity.x;
        float smoothedVelocity = Mathf.Lerp(currentForce, sideForce, sideSpeedSmooth * Time.deltaTime);
        rb.velocity = new Vector3(smoothedVelocity, rb.velocity.y, rb.velocity.z);

    }

    void AccelBall() {

        if (rb.velocity.z <= desiredVelocity && canAccel){

            rb.AddForce(new Vector3(0, 0, desiredVelocity),ForceMode.Acceleration);
        }

        if (!canAccel){

            rb.velocity = new Vector3(0, 0, 0);
        }

    }

    void Update(){

        // end current launch if velocity is slow enough
        if (isLaunched && rb.velocity.z <= minimumVelocity){

            EndCurrentLaunch();
        }

        // end current launch if Y pos is too low

        if (transform.position.y <= -1){

            EndCurrentLaunch();
        }

    }

    void EndCurrentLaunch(){

        if (currentRunEnded)
            return;

        // RESET DISTANCE
        distanceManager.recordDistance = false;

        BallExplotion();
        canAccel = false;

        winMan.HideTouchpad();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        //if (distanceManager.lives > 0)

            pointsMan.ShowPointsWin();
        
        //else
            //pointsMan.ShowGameOverWindow();

        currentRunEnded = true;
    }

    public void NewLaunch(){

        ResetPosition();

    }

    IEnumerator ballVelocity(){

        yield return new WaitForSeconds(0.5f);
        isLaunched = true;

    }

    public void LaunchBall(){

        if (!canBeLaunched)
            return;

        this.GetComponent<SphereCollider>().enabled = true;
        canAccel = true;

        // play the catapult animation
        animator.Play("LaunchAnim");

        messages.ShowLaunchMessage();

            // dont aplply more force
        canBeLaunched = false;

        // soothly gwrow the ball to size
        isGrowing = true;

        // play launch sound
        soundManager.PlayLaunch();

        transform.position = launcher.position;
        rb.constraints = RigidbodyConstraints.None;

        float launchForce = launchForceMultiplier;

        // get launch direction
        Vector3 launchDir = new Vector3(launcher.forward.x, launcher.forward.y, launcher.forward.z);
        //apply force
        rb.AddForce(launchDir * launchForce, ForceMode.VelocityChange);
        // add rotation
        rb.AddTorque(new Vector3 (1,0,0) * launchForce, ForceMode.VelocityChange);

        camFollower.target = this.transform;

        StartCoroutine(ballVelocity());

        winMan.BallView();
        winMan.UpdatePoints();

    }

    // RESET for a new launch
    void ResetPosition() {

        currentRunEnded = false;

        // RESET DISTANCE
        distanceManager.ResetDistance();

        //reset the map to reactivate pieces
        mapMan.ResetMap();

        // reset the view
        winMan.CatapultView();

        //resize the ball
        transform.localScale = new Vector3(1, 1, 1);

        //position the ball in the launcher and remove physics
        transform.position = spoon.position;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        canBeLaunched = true;
        isLaunched = false;

        // gets a new broken ball prefab to destroy
        ReconstructBall();
    }

}
