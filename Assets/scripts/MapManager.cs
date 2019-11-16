using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    public bool debug = false;

    public Transform ball;
    public Transform mapHolder;
    public GameObject emptyMap;
    public List<GameObject> mapPieces;
    public int mapsize = 66;
    public int activationDistance = 30;

    //building Respawn
    public GameObject[] mapPrefabs;
    private int prefabToInstantiate = 0; // current prefab to instantiate
    public int zone = 1; // grop of map pieces (*  zonelenght)
    public int zoneLenght = 4; // how many blocks will be instantiated before a wall
    public int counter = 0; // for instantiating and deleting pieces

    private upgrades upgradeScript;
    public Material breakableMaterial;
    public Material unbreakableMaterial;
    public int currentPower;

	// Use this for initialization
	void Awake () {

        upgradeScript = FindObjectOfType<upgrades>();
        currentPower = upgradeScript.POWERLVL;

        for (int i = 0; i < mapHolder.childCount; i++)
            mapPieces.Add(mapHolder.GetChild(i).gameObject);
    }



    public void ResetMap(){

        zone = 1;
        prefabToInstantiate = 0;

        for (int i = 0; i < mapHolder.childCount; i++)
            Destroy(mapHolder.GetChild(i).gameObject);

        mapPieces.Clear();

        counter = 0;

        GameObject map1 = Instantiate(emptyMap, new Vector3(0, 0, 0), Quaternion.identity);
        map1.transform.parent = mapHolder;
        mapPieces.Add(map1);

        GameObject map2 = Instantiate(emptyMap, new Vector3(0, 0, mapsize), Quaternion.identity);
        map2.transform.parent = mapHolder;
        mapPieces.Add(map2);

    }
	
	// Update is called once per frame
	void Update () {

        if (ball.transform.position.z >= (activationDistance + (mapsize * counter))) {

            currentPower = upgradeScript.POWERLVL;

            //print("prefabToInstantiate " + prefabToInstantiate);


                GameObject map3 = Instantiate(mapPrefabs[prefabToInstantiate], mapHolder);
                map3.transform.position = new Vector3(0, 0, mapsize * (counter + 2));
                mapPieces.Add(map3);

            // DESTROY MAP PIECES AND REMOVE THEM FROM LIST

            if ((counter - 1) >= 0){

                Destroy(mapPieces[0]);
                mapPieces.RemoveAt(0);
            }

            // RANDOMLY INSTANTIATE OBSTACLES

            Transform obstacles = map3.transform.Find("OBSTACLES");

            for (int i = 0; i < obstacles.childCount; i++){

                int random = Random.Range(0, 3);
                bool isActive = random == 1 ? true: false;

                obstacles.GetChild(i).gameObject.SetActive(isActive);

            }

            //INSTANTIATE FINAL WALL ON ZONE END

            if (counter == (zoneLenght * zone)) {

                if (prefabToInstantiate < mapPrefabs.Length)
                {
                    prefabToInstantiate++;

                }
                else
                {

                    prefabToInstantiate = Random.Range(0, mapPieces.Count);

                }


                zone++;
                map3.transform.Find("FINALWALL").gameObject.SetActive(true);

                }

            counter++;

            //print("counter " + counter);

        } 
    }

}
