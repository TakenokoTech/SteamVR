using UnityEngine;

public class VrUtil {

    public static void tracePosition (GameObject origin, GameObject target) {
        Vector3 vec = origin.transform.position;
        Quaternion rot = origin.transform.rotation;
        target.transform.position = vec;
        target.transform.rotation = rot;
    }
}