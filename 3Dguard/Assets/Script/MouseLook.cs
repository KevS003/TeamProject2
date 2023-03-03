using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
  public float mouseSensitivity = 100f;

  public Transform playerBody;

  private float xRotation = 0f;

  public float rcRange = .3f;

  public bool off = false;

  //reference guard script to turn off
  //public EnemyGuard offReference



    // Start is called before the first frame update
    void Start()
    {
      Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
    //ray cast to detect things in front and ask player if they want to interact
      Vector3 direction = Vector3.forward;
      Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * rcRange));
      Debug.DrawRay(transform.position, transform.TransformDirection(direction * rcRange));

      if(Physics.Raycast(theRay, out RaycastHit hit, rcRange))
      {
        if(hit.collider.tag == "BackRobot")
        {
          Debug.Log("EnemyDetected");
          //turn on UI input here for the button
          if(Input.GetKeyDown(KeyCode.F))
          {
            Debug.Log("ROBOTOFF");
            //TurnOff();
          }
        }

      }


        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp (xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);


    }



}
