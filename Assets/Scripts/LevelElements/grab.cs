using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grab : MonoBehaviour
{
    public Transform player;
    public Transform playerCam;
    public float throwForce = 10;
    private bool hasPlayer = false;
    private bool beingCarried = false;
    private bool touched = false;
    private Joint playerJoint;
    void Start(){
        player = GameObject.FindWithTag("Player").transform;
        playerCam = GameObject.FindWithTag("MainCamera").transform;
        playerJoint = player.GetComponentInChildren<Joint>();
    }

    void Update()
    {
        float dist = Vector3. Distance(gameObject.transform.position, player.position);
        if (dist <= 1.9f)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }
        if (hasPlayer && Input. GetKey(KeyCode.R))
        {
            Connect(true);

        }
        if (beingCarried)
        {
            if (Input. GetMouseButtonDown(0))
            {
                Connect(false);
                GetComponent<Rigidbody>() .AddForce(playerCam.forward * throwForce);
            }
            else if (Input. GetMouseButtonDown (1))
            {
                Connect(false);
            }
        }


    }

            private void Connect(bool value)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            beingCarried = value;
            if(value){
                rb.constraints = RigidbodyConstraints.None;
                playerJoint.connectedBody = rb;
            }
            else{
                rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ  ;
                playerJoint.connectedBody = null;
            }
        }

}
