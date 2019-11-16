using UnityEngine;


public class Structure : MonoBehaviour {

    private MapManager mapMan;

    public int points = 100;
    public float PowerToDestroy = 2f;
    public bool changeMaterial = true;


    private void Start(){
        mapMan = FindObjectOfType<MapManager>();
        

        PowerToDestroy = PowerToDestroy + mapMan.zone  -1;

        if (changeMaterial) { 

        if (this.PowerToDestroy > mapMan.currentPower) 
            this.GetComponentInChildren<MeshRenderer>().material = mapMan.unbreakableMaterial;
        else
            this.GetComponentInChildren<MeshRenderer>().material = mapMan.breakableMaterial;

        }
    }
}
