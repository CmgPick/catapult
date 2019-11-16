using System.Collections;
using UnityEngine;

public class removeColliders : MonoBehaviour {

    private float waitTime = 5f;

	// Use this for initialization
	void Start () {

        StartCoroutine(wait());
	}

    IEnumerator wait(){

        yield return new WaitForSeconds(waitTime);
        RemovePysics();
    }


    void RemovePysics () {

        for (int i = 0; i < transform.childCount; i++){

            GameObject piece = transform.GetChild(i).gameObject;
            Destroy(piece.GetComponent<Rigidbody>());
            Destroy(piece.GetComponent<Collider>());
        }

        Destroy(this.GetComponent<removeColliders>());
		
	}
}
