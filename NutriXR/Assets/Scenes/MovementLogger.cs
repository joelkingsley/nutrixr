using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLogger : MonoBehaviour
{
    public GameObject CameraRig;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(LogPosition), 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LogPosition() {
        Vector3 position = CameraRig.transform.position;
        DataLogger.Log("MovementLogger", "Pos: " + position);
    }

}
