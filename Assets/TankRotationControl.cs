using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRotationControl : MonoBehaviour
{
    public Vector3 rotation;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        rotation = obj.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rotation = obj.transform.eulerAngles;
        if (rotation.z >= 180f) {
            obj.transform.eulerAngles = new Vector3(rotation.x,rotation.y,0f);
        }
    }
}
