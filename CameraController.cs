using UnityEngine;

public class CameraController : MonoBehaviour {

	public float panSpeed = 30f;
	public float panBorderThickness = 10f;

	public float scrollSpeed = 5f;
	public float minY = 10f;
	public float maxY = 80f;
	
	public Vector3 myCamPos = Vector3.zero;
	private static readonly float PanSpeed = 20f;
	private static readonly float ZoomSpeedTouch = 0.005f;
	private static readonly float ZoomSpeedMouse = 0.005f;

	private static readonly float[] BoundsX = new float[]{-1000f,1005f};
	private static readonly float[] BoundsZ = new float[]{-1000f, 14f};
	private static readonly float[] ZoomBounds = new float[]{10f, 85f};

	public Camera cam;

	private Vector3 lastPanPosition;
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

		if (Input.GetKey("w") )// || Input.mousePosition.y >= Screen.height - panBorderThickness)
		{
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("s") )// || Input.mousePosition.y <= panBorderThickness)
		{
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d") )// || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("a") )// || Input.mousePosition.x <= panBorderThickness)
		{
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("r"))
		{
			transform.Translate(transform.position = myCamPos);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;

		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);

		transform.position = pos;

		if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
		{
			HandleTouch();
		} else {
			HandleMouse();
		}
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

		// Check for scrolling to zoom the camera
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		ZoomCamera(scroll, ZoomSpeedMouse);
	}
	
	void PanCamera(Vector3 newPanPosition)
	{
		// Determine how much to move the camera
		Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
		Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
		
		// Perform the movement
		transform.Translate(move, Space.World);  
		
		// Ensure the camera remains within bounds.
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
		pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
		transform.position = pos;

		// Cache the position
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
