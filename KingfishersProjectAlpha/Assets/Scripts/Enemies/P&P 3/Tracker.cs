using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject trackedObject;
    public Vector3 averageVelocity;
    public Vector3 averageAccel;

    private Vector3 prevVelocity;
    private Vector3 prevAccel;
    private Vector3 prevPos;


    // Start is called before the first frame update
    void Start()
    {
        trackedObject = gameManager.Instance.PlayerModel;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        StartCoroutine(FindFuture());
    }

    IEnumerator FindFuture()
    {
        yield return new WaitForEndOfFrame();

        Vector3 currVelocity = (trackedObject.transform.position - prevPos) / Time.deltaTime;
        Vector3 currAccel = currVelocity - prevVelocity;

        averageVelocity = currVelocity;
        averageAccel = currAccel;


        prevPos = trackedObject.transform.position;
        prevVelocity = currVelocity;
        prevAccel = currAccel;
    }

    public Vector3 ProjectedPosition(float fTime)
    {
        Vector3 v3Ret = new Vector3();

        v3Ret = trackedObject.transform.position + (averageVelocity * Time.deltaTime * (fTime / Time.deltaTime)) + (0.5f * averageAccel * Time.deltaTime * Mathf.Pow(fTime / Time.deltaTime, 2));
        return v3Ret;
    }
}
