using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;

    private float inputX;

    public float moveSpeed = 5, jumpForce = 7, jumpLineLength = 0.2f;

    public LayerMask groundedLayer;

    private SpringJoint2D Joint;
    private LineRenderer lr;

    public Transform player;

    Vector3 grapplePostion, GrapPos;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        lr = transform.GetComponent<LineRenderer>();
    }

    void Update()
    {
        //获取移动的值
        inputX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, jumpLineLength, groundedLayer);

        if (hit && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += new Vector2(0, jumpForce);
        }

        player.position += new Vector3(inputX, 0);

        if (Input.GetMouseButtonDown(0))
        {
            StartGrappling();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrappling();
        }
    }

    private void LateUpdate() {
        DrawLine();
    }

    void StartGrappling()
    {
        RaycastHit2D grapplHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (grapplHit.collider != null)
        {
            GrapPos = grapplHit.point;
            Joint = gameObject.AddComponent<SpringJoint2D>();
            Joint.connectedAnchor = GrapPos;
            Joint.autoConfigureDistance = false;

            Joint.distance = 2 * 0.8f;

            lr.positionCount = 2;
        }
    }

    void StopGrappling()
    {
        lr.positionCount = 0;
        Destroy(Joint);
    }

    public void DrawLine()
    {
        if (!Joint)
        {
            return;
        }
        lr.SetPosition(0, player.position);

        lr.SetPosition(1, GrapPos);
    }
}
