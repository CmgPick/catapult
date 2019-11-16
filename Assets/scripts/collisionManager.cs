using UnityEngine;

public class collisionManager : MonoBehaviour {

    public bool debug = false;

    private int explotionLayer = 10;
    private int ballLayer = 12;
    private int entireBuldingLayer = 14;
    private int destroyedBuildingLayer = 13;

    private pointsManager pointsMan;
    private Messages messages;
    private SOUNDMANAGER soundManager;
    private ballLauncher ball;
    private upgrades upgradeScript;
    public GameObject explotionPrefab;
    public GameObject points3dTxt;
    public Vector3 pointsTxtOffset;
    public float explotionSizeMultiplier = 2.0f;

    // Use this for initialization
    void Start () {

        upgradeScript = FindObjectOfType<upgrades>();
        ball = FindObjectOfType<ballLauncher>();
        soundManager = FindObjectOfType<SOUNDMANAGER>();
        pointsMan = FindObjectOfType<pointsManager>();
        messages = FindObjectOfType<Messages>();

        Physics.IgnoreLayerCollision(entireBuldingLayer, destroyedBuildingLayer);
        Physics.IgnoreLayerCollision(ballLayer, destroyedBuildingLayer);
        Physics.IgnoreLayerCollision(ballLayer, explotionLayer);

    }


    // separate the other object to pieces
    void Destruct (GameObject objectToDestroy) {

        Physics.IgnoreLayerCollision(ballLayer, destroyedBuildingLayer);

        GameObject entireVersion = objectToDestroy.transform.parent.transform.Find("entire").gameObject;
        GameObject destroyedVersion = objectToDestroy.transform.Find("destroyed").gameObject;

        // makes this child of the buildings root
        destroyedVersion.transform.parent = objectToDestroy.transform.parent.parent;

        //Destroy(entireVersion);
        Destroy(entireVersion.transform.parent.gameObject);
        destroyedVersion.SetActive(true);

        destroyedVersion.AddComponent<removeColliders>();

    }

    void OnTriggerEnter(Collider collision){

        if (collision.transform.GetChild(0).tag == "destructable"){

            Structure hittedStructure = collision.gameObject.transform.GetComponent<Structure>();

            if (upgradeScript.POWERLVL >= hittedStructure.PowerToDestroy){

                messages.ShowDestructionMessage();
                Destruct(collision.transform.GetChild(0).gameObject);

                //add points
                pointsMan.addPoints(hittedStructure.points);

                // instantiate explotion
                GameObject newExplotion = Instantiate(explotionPrefab, transform.position, Quaternion.identity);
                Explotion explotion = newExplotion.GetComponent<Explotion>();
                explotion.radius = ball.size.z * explotionSizeMultiplier;

                //instantiate 3d points text
                Vector3 textPos = transform.position + pointsTxtOffset;
                GameObject pointsTxt = Instantiate(points3dTxt, textPos, Quaternion.identity);
                pointsTxt.GetComponent<TextMesh>().text = hittedStructure.points.ToString();

                //play destroySound
                soundManager.PlayDestroy();

                ball.canAccel = true;
            }
            else
                ball.canAccel = false;
      
        }
    }


    void OnCollisionEnter(Collision collision){


        if (collision.transform.tag == "destructable"){

            if (debug)
                print("collision Magnitude :" + collision.relativeVelocity.magnitude);

            Structure hittedStructure = collision.gameObject.transform.parent.GetComponent<Structure>();
            if (upgradeScript.POWERLVL >= hittedStructure.PowerToDestroy){


            } else
            {
                // on a soft hit
                messages.ShowSoftHitMessage();
               
            }

        } 

  


    }
}
