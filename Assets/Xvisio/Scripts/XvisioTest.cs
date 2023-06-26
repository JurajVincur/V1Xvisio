using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XvisioTest : MonoBehaviour
{
    public Transform target;

    private float[] rawPose;

    // Start is called before the first frame update
    void Start()
    {
        Xvisio.Start();
        rawPose = new float[12];
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        if (Xvisio.GetPose(rawPose) == true)
        {
            target.position = new Vector3(rawPose[9], -rawPose[10], rawPose[11]);
            Matrix4x4 m = Matrix4x4.identity;
            m.m00 = rawPose[0];
            m.m01 = rawPose[1];
            m.m02 = rawPose[2];
            m.m10 = rawPose[3];
            m.m11 = rawPose[4];
            m.m12 = rawPose[5];
            m.m20 = rawPose[6];
            m.m21 = rawPose[7];
            m.m22 = rawPose[8];
            target.eulerAngles = Vector3.Scale(m.rotation.eulerAngles, new Vector3(-1f, 1f, -1f));
        }
    }

    private void OnDestroy()
    {
        Xvisio.Stop();
        Debug.Log("Stopped");
    }
}
