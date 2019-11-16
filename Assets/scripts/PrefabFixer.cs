using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabFixer : MonoBehaviour {

    public bool FIX = false;

	void FixPrefab () {

        this.gameObject.AddComponent<BoxCollider>();
        this.GetComponent<BoxCollider>().isTrigger = true;
        this.gameObject.AddComponent<Structure>();

        Transform entire = this.transform.GetChild(0).GetComponent<Transform>();
        entire.name = "entire";
        entire.tag = "destructable";
        entire.gameObject.layer = 14;
        entire.gameObject.AddComponent<MeshCollider>();
        entire.GetComponent<MeshCollider>().isTrigger = false;
        entire.GetComponent<MeshCollider>().enabled = false;

        this.transform.position = Vector3.zero;
        entire.transform.localPosition = Vector3.zero;
        entire.localScale = new Vector3(1, 1, 1);
        entire.localRotation = new Quaternion(0, 0, 0, 0);

        MeshRenderer meshRender = entire.GetComponent<MeshRenderer>();
        meshRender.receiveShadows = false;
        meshRender.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        meshRender.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        meshRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;


        FIX = false;
        PrefabFixer fixer = gameObject.GetComponent<PrefabFixer>();
        DestroyImmediate(fixer);
	}
	
	// Update is called once per frame
	void Update () {

        if (FIX)
            FixPrefab();
	}
}
