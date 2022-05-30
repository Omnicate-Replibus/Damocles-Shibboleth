using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerawalker : MonoBehaviour
{
    public int sensitivity = 5;
    public float clampAngle = 80.0f;

    public GameObject controltime;
    private UIcontrol _paused;

    private float mouseSensitivity = 150f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    GameObject player;
    public CharacterController controller;
    public float speed = 6f;


    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        player = this.transform.parent.gameObject; // Getting the parent object.
        controller = player.GetComponent<CharacterController>(); // You can use the getcomponent in child instead
        _paused = controltime.GetComponent<UIcontrol>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
        transform.parent.transform.rotation = Quaternion.Euler(0.0f, rotY, 0.0f);

        if(_paused.paused == false)
        {
            rotY += mouseX * mouseSensitivity * sensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * sensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 Direction = (player.transform.forward * vertical + player.transform.right * horizontal).normalized;

            controller.Move(Direction * speed * Time.deltaTime);

        }

    }
}
