using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCam : MonoBehaviour
{
    public static RTSCam instance;

    [SerializeField] private float speed = 0.03f;
    [SerializeField] private float zoomSpeed = 10.0f;
    [SerializeField] private float rotSpeed;

    [SerializeField] private float maxHeight = 40f;
    [SerializeField] private float minHeight = 4f;

    [SerializeField] private Vector2 p1;
    [SerializeField] private Vector2 p2;

    [SerializeField] private float hsp;
    [SerializeField] private float vsp;
    [SerializeField] private float scrollSp;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    void Start()
    {
        scrollSp = -10;
            //Mathf.Log(transform.position.z) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
    }

    public void Update()
    {
        hsp = transform.position.y * speed * (Input.GetAxis("Horizontal") / 10);
        vsp = transform.position.y * speed * (Input.GetAxis("Vertical") /10);
        scrollSp = transform.position.z * (Input.GetAxis("Mouse ScrollWheel") / 10);

        {        

            if (transform.position.z <= maxHeight)
            {
                scrollSp = maxHeight - transform.position.z;
            }
            else if (transform.position.z > minHeight)
            {
                scrollSp = minHeight - transform.position.z;
            }
        }

        Vector3 verticalMove = new Vector3(0, vsp, 0); // y move
        Vector3 lateralMove = hsp * transform.right; // left right move
        Vector3 forwardMove = transform.forward; // "forward" move (negate cam moving forward dir face
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= scrollSp;

        Vector3 move = verticalMove + lateralMove + forwardMove;

        transform.position += move;
    }
}