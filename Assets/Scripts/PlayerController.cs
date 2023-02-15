using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed;
	public float playerSize;
	private Rigidbody myRigidbody;
	
	private Vector3 moveInput;
	private Vector3 moveVelocity;
	
	private Camera mainCamera;
	
	private GameObject camOrigin;
	private GameObject player;
	private Vector2 turn;
	private Vector3 camRot;
	private Vector3 finalCamRot;
	private Vector2 zoom;
		
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        camOrigin = GameObject.Find("camOrigin");
		player = GameObject.Find("Player");
		zoom = new Vector2(0, 30);
		//Cursor.lockState = CursorLockMode.Locked;
		player.transform.localScale = new Vector3(playerSize, playerSize, playerSize);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed * player.transform.localScale.x;
        turn.x = Input.GetAxis("Mouse X");
        turn.y = Input.GetAxis("Mouse Y");
        zoom += Input.mouseScrollDelta*2;
        camRot.y+= turn.x;
        camRot.x+= turn.y;
        finalCamRot = new Vector3(camRot.x, camRot.y, 0);
        
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        
        mainCamera.fieldOfView = zoom.y;
        
        if(groundPlane.Raycast(cameraRay, out rayLength)){
        	Vector3 pointToLook = cameraRay.GetPoint(rayLength);
        	Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
        	
		transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		// camOrigin.transform.position = new Vector3(mainCamera.transform.position.x-pointToLook.x, transform.position.y, mainCamera.transform.position.z-pointToLook.z);
		//	camOrigin.transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
		camOrigin.transform.position = Vector3.Lerp(camOrigin.transform.position, transform.position, 2*Time.deltaTime);
		//camOrigin.transform.eulerAngles += new Vector3(camOrigin.transform.rotation.x, camOrigin.transform.rotation.y+turn.x, camOrigin.transform.rotation.z);
		camOrigin.transform.eulerAngles = new Vector3(camRot.x, camRot.y, 0);
		if (camRot.x >= 15) {camRot.x = 15;}
		if (camRot.x <= -45) {camRot.x = -45;}	
		if (camRot.y >= 25) {camRot.y = 25;}
		if (camRot.y <= -25) {camRot.y = -25;}
		if (zoom.y <= 25) {zoom.y = 25;}
		if (zoom.y >= 90) {zoom.y = 90;}
        }
    }
    
    void FixedUpdate() {
		myRigidbody.velocity = moveVelocity;
    }
}
