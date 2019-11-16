using UnityEngine;

public class Explotion : MonoBehaviour {

    public float ExplotionPower =10.0f;
    public float radius = 1.0f;
    public float upforce = 1.0f;

	// Use this for initialization
	void Start () {

        Explode();
	}

    // Update is called once per frame
    void Explode () {

        Physics.IgnoreLayerCollision(10, 12);


        Vector3 explotionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explotionPos, radius);

        foreach (Collider hit in colliders){

            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null)
            rb.AddExplosionForce(ExplotionPower, explotionPos, radius, upforce, ForceMode.Impulse);

        }
	}
}
