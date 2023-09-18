using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCam : MonoBehaviour
{
    public static RTSCam instance;

    [SerializeField] private float speed = 0.03f;
    [SerializeField] private float zoomSpeed = 10.0f;
    [SerializeField] private float rotSpeed;

    [SerializeField] private float maxSize = 15f;
    [SerializeField] private float minSize = 3f;

    [SerializeField] private Vector2 p1;
    [SerializeField] private Vector2 p2;

    [SerializeField] private float hsp;
    [SerializeField] private float vsp;
    [SerializeField] private float scrollSp;
    [SerializeField] private float projectionSize;
    [SerializeField] private Vector3 projectionPosition;

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

        projectionSize = Camera.main.orthographicSize;
        projectionPosition = transform.position;
    }

    public void Update()
    {
        hsp = Input.GetAxis("Horizontal");
        vsp = Input.GetAxis("Vertical");
        scrollSp = (Input.GetAxis("Mouse ScrollWheel") * 3);

        projectionSize = Mathf.Clamp(projectionSize -= scrollSp, minSize, maxSize);

        Vector3 direction = new Vector3(hsp, vsp, 0f);
        if(direction.magnitude > direction.normalized.magnitude)
            direction.Normalize();

        projectionPosition += direction * (speed * (projectionSize / 4f) * Time.deltaTime);

        if((transform.position - projectionPosition).magnitude > 0.3f)
            transform.position = Vector3.Lerp(transform.position, projectionPosition, 10f * Time.deltaTime);
        if(Mathf.Abs(Camera.main.orthographicSize - projectionSize) > 0.2f)
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, projectionSize, 100f * Time.deltaTime);
    }
}