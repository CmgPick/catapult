using UnityEngine;

	public class SmoothFollow : MonoBehaviour{

		public  Transform target;
        public Vector3 offSet;
        public float smoothSpeed = 0.125f;


    private void LateUpdate(){

        Vector3 desiredPosition = target.position + offSet;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);

    }

    /*
		// The distance in the x-z plane to the target
		[SerializeField]
		public float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		public float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;
        [SerializeField]
        private float xDamping;
        [SerializeField]
        private float zDamping;

    // Use this for initialization
    void Start() { }

		// Update is called once per frame
		void LateUpdate(){

			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;
            var wantedX = target.position.x;
            var wantedZ = target.position.z - distance;

        var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;
            var currentX = transform.position.x;
            var currentZ = transform.position.z;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
            currentX = Mathf.Lerp(currentX, wantedX, xDamping * Time.deltaTime);
            currentZ = Mathf.Lerp(currentZ, wantedZ, zDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			//transform.position = target.position;
			//transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(currentX ,currentHeight , currentZ);

			// Always look at the target
			transform.LookAt(target);

        Debug.Log(transform.position);
		}*/
}
