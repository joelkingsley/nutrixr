using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingManager : MonoBehaviour
{
    public GameObject AnchorToFollow;

    public Vector3 positionOffset;

    private Vector3 originPosition;
    private Vector3 targetPosition;
    private Quaternion originRotation;
    private Quaternion targetRotation;

    //private bool scaling = false;

    private bool moving = false;
    private float timeCount = 0.0f;

    public float distanceThreshold = 0.3f;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = AnchorToFollow.transform.position + Quaternion.LookRotation(AnchorToFollow.transform.forward) * positionOffset;
        //Ignore difference in elevation
        //pos1 = new Vector3(pos1.x, 0, pos1.z);
        //pos2 = new Vector3(pos2.x, 0, pos2.z);

        float distance = Vector3.Distance(pos1, pos2);

        if ((distance > distanceThreshold && !moving)) {

            //Position
            originPosition = transform.position;
            //targetPosition = AnchorToFollow.transform.position + Quaternion.LookRotation(AnchorToFollow.transform.forward) * positionOffset;
            targetPosition = AnchorToFollow.transform.position + Quaternion.Euler(0, AnchorToFollow.transform.rotation.eulerAngles.y, 0) * positionOffset;

            //Rotation
            originRotation = transform.rotation;
            targetRotation = Quaternion.Euler(0, 0, 0) * Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, AnchorToFollow.transform.rotation.eulerAngles.y, 0), 360);

            timeCount = 0;
            moving = true;
        }
        if (moving) {
            transform.rotation = Quaternion.Lerp(originRotation, targetRotation, timeCount);
            transform.position = Vector3.Lerp(originPosition, targetPosition, timeCount);
        }

        if(timeCount < 1) {
            timeCount += Time.deltaTime * speed;
        } else {
            moving = false;
            timeCount = 0;
        }
    }
}
