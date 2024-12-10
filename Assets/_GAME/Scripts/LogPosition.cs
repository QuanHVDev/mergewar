using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPosition : MonoBehaviour
{
    // Update is called once per frame
    void Update() {
        Logs.Log(transform.position.ToString());
    }
}
