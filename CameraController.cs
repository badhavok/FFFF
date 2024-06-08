using UnityEngine;

public class CameraController : MonoBehaviour {

	public float panSpeed = 30f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 5f;
	
	public Vector3 myCamPos = Vector3.zero;
	private static readonly float PanSpeed = 20f;
	private static readonly float ZoomSpeedTouch = 0.005f;
	private static readonly float ZoomSpeedMouse = 0.005f;

	public float[] BoundsX = new float[]{-1000f,1005f};
	public float[] BoundsZ = new float[]{-1000f, 14f};
	public float[] BoundsY = new float[]{-40, 40};
	public float[] ZoomBounds = new float[]{10f, 85f};

	public Camera cam;
	public static Camera PlayerCam;

	private Vector3 lastPanPosition;
	private Vector3 rotateValue;
	private Vector3 currentEulerAngles;
	private float x, y, z;
	private int panFingerId; // Touch mode only

	private bool wasZoomingLastFrame; // Touch mode only
	private Vector2[] lastZoomPositions; // Touch mode only	

	void Start()
	{
	    myCamPos = transform.position;
	}
	void Awake()
	{
		cam = GetComponent<Camera>();
		PlayerCam = cam;
	}

	// Update is called once per frame
	void Update ()
	{	
		// if (isMenuOpen)
		// {
			// return;
		// }
		if (GameManager.GameIsOver)
		{
			this.enabled = false;
			return;
		}
		// Move
		if (Input.GetKey("w") )// || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
			transform.Translate(new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z) * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("s") )// || Input.mousePosition.y <= panBorderThickness)
		{
			transform.Translate(new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z) * -panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d") )// || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			transform.Translate(cam.transform.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("a") )// || Input.mousePosition.x <= panBorderThickness)
		{
			transform.Translate(cam.transform.right * -panSpeed * Time.deltaTime, Space.World);
		}
		// Rotate
		if (Input.GetKey("q") )// || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			transform.Rotate(Vector3.down * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("e") )// || Input.mousePosition.x <= panBorderThickness)
		{
			transform.Rotate(Vector3.up * panSpeed * Time.deltaTime, Space.World);
		}
		// Zoom
		if (Input.GetKey("z") )
		{
			transform.Translate(Vector3.up * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("x") )
		{
			transform.Translate(Vector3.down * panSpeed * Time.deltaTime, Space.World);
		}
		// Reset
		if (Input.GetKey("r"))
		{
			transform.Translate(transform.position = myCamPos);
		}
		
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;

		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, BoundsY[0], BoundsY[1]);
		pos.x = Mathf.Clamp(pos.x, BoundsX[0], BoundsX[1]);
		pos.z = Mathf.Clamp(pos.z, BoundsZ[0], BoundsZ[1]);

		transform.position = pos;

		// if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
		// {
			HandleTouch();
		// } else {
			HandleMouse();
		// }

		
	}

	void HandleTouch()
	{
		switch(Input.touchCount) {

		case 1: // Panning
			wasZoomingLastFrame = false;
			
			// If the touch began, capture its position and its finger ID.
			// Otherwise, if the finger ID of the touch doesn't match, skip it.
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				lastPanPosition = touch.position;
				panFingerId = touch.fingerId;
			} else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved) {
				PanCamera(touch.position);
			}
			break;

		case 2: // Zooming
			Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
			if (!wasZoomingLastFrame) {
				lastZoomPositions = newPositions;
				wasZoomingLastFrame = true;
			} else {
				// Zoom based on the distance between the new positions compared to the 
				// distance between the previous positions.
				float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
				float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
				float offset = newDistance - oldDistance;

				ZoomCamera(offset, ZoomSpeedTouch);
				
				lastZoomPositions = newPositions;
			}
			break;
			
		default: 
			wasZoomingLastFrame = false;
			break;
		}
	}

	void HandleMouse()
	{
		// On mouse down, capture it's position.
		// Otherwise, if the mouse is still down, pan the camera.
		if (Input.GetMouseButtonDown(0))
		{
			lastPanPosition = Input.mousePosition;
		} else if (Input.GetMouseButton(0)) {
			PanCamera(Input.mousePosition);
		}

		if(Input.GetMouseButton(1))
		{
			Debug.Log("Button 2");
			y = Input.GetAxis("Mouse X");
			x = Input.GetAxis("Mouse Y");
			Debug.Log(x + ":" + y);
			rotateValue = new Vector3(x, y * -1, 0);
			transform.eulerAngles = transform.eulerAngles - rotateValue;
		}

		// Check for scrolling to zoom the camera
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		ZoomCamera(scroll, ZoomSpeedMouse);
	}
	
	void PanCamera(Vector3 newPanPosition)
	{
		Vector3 viewDelta = newPanPosition - lastPanPosition;
		Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
		Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
		Vector3 moveVec = viewDelta.x * right + viewDelta.y * forward;
		moveVec = Vector3.ClampMagnitude(moveVec, 1);
		transform.Translate(moveVec * panSpeed * Time.deltaTime, Space.World);

		// Debug.DrawRay(Camera.main.transform.position, moveVector);
		// Debug.DrawRay(Camera.main.transform.position, moveVector.normalized, Color.red, 0.5f);
		lastPanPosition = newPanPosition;
	}

	void ZoomCamera(float offset, float speed)
	{
		if (offset == 0) {
			return;
		}

		cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
	}
}
