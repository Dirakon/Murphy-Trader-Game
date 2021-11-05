using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public float mainSpeed = 100.0f; //regular speed
    public float _mouseSensitivity = 20f; //How sensitive it with mouse
    public Camera camera;
    public Rigidbody rigidbody;
    private static Character singleton;
    void Awake(){
        singleton=this;
    }
    private float xRotation = 0f;
    [SerializeField] private float _minCameraview = -70f, _maxCameraview = 80f;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public static void CancelEverything(){
        singleton.previousAction?.DeactivatePopUp();
        singleton.previousAction = null;
        singleton.cancelEverythingFlag = true;
        singleton.ShowMouse();
    }
    public static void AllowEverything(){
        singleton.cancelEverythingFlag = false;
    }
    bool cancelEverythingFlag = false;
    ActionOnE previousAction = null;
    public LayerMask actionELayerMask;
    // Update is called once per frame
    void Update()
    {
        if (cancelEverythingFlag)
            return;
        HideMouse();
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position,camera.transform.forward);
        ActionOnE action = null;
        if (Physics.Raycast(ray, out hit,9999f,actionELayerMask)) {

            // Debug.Log(hit.collider);
            action = hit.collider.GetComponent<ActionOnE>();
            if (action != null && (action.transform.position - camera.transform.position).magnitude >=
            action.dist){
                action = null;
            }
            // Do something with the object that was hit by the raycast.
        }
        if (previousAction != action){
            previousAction?.DeactivatePopUp();
            action?.ActivatePopUp();
        }
        previousAction = action;
        if (Input.GetKeyDown(KeyCode.E)){
            action?.ActivateAction();
        }
        //Get Mouse position Input
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity; //changed this line.
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity; //changed this line.
                                                                     //Rotate the camera based on the Y input of the mouse
        xRotation -= mouseY;
        //clamp the camera rotation between 80 and -70 degrees
        xRotation = Mathf.Clamp(xRotation, _minCameraview, _maxCameraview);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //Rotate the player based on the X input of the mouse
        transform.Rotate(Vector3.up * (mouseX * 1.5f));


        //Keyboard commands
        rigidbody.velocity = GetBaseInput() * mainSpeed;
    }

    private Vector3 GetBaseInput()
    {
        Vector3 right = camera.transform.right;
        right.y = 0;
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        return (forward * Input.GetAxis("Vertical") + 
        right * Input.GetAxis("Horizontal")).normalized;
    }
}
