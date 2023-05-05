using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Portal_camera : MonoBehaviour
{
    [SerializeField] Transform exit;
    [SerializeField] Transform portal;
    [SerializeField] Transform playerPos;
    [SerializeField] Camera seeThrough;
    [SerializeField] Camera playerCamera;
    [SerializeField] Collider findPlayer;
    [SerializeField] int lockMin;
    [SerializeField] int lockMax;
    [SerializeField] int XLock;
    [SerializeField] int YLock;
    [SerializeField] private UnityEngine.Vector3 origin;

    private bool playerNear;
    private float angleDiff;
    private UnityEngine.Vector3 seeThroughPos;
    private UnityEngine.Quaternion rotationRelation;
    private UnityEngine.Vector3 finalSeeCam;
    private float xPos;
    private float yPos;
    private float x;
    private float y;
    private float z;

    private void Start()
    {
        origin = seeThrough.transform.localPosition;
    }

    private void Update()
    {
        if(playerNear)
        {
            moveCamera();
            rotateCamera();
        }
    }

    void moveCamera()
    {
        xPos = playerPos.transform.position.x;
        xPos = Mathf.Clamp(-xPos, -XLock, XLock);
        yPos = playerPos.transform.position.y;
        yPos = Mathf.Clamp(yPos, -YLock, YLock);
        finalSeeCam = new Vector3(xPos, yPos-5, 0);
        transform.localPosition = origin + finalSeeCam;

    }

    void rotateCamera()
    {
        angleDiff = UnityEngine.Quaternion.Angle(portal.rotation, exit.rotation);
        rotationRelation = UnityEngine.Quaternion.AngleAxis(angleDiff, UnityEngine.Vector3.up);
        seeThroughPos = rotationRelation * playerCamera.transform.forward;
        x -= seeThroughPos.y;
        x = Mathf.Clamp(x, lockMin, lockMax);
        y -= seeThroughPos.x;
        y = Mathf.Clamp(y, lockMin, lockMax);
        transform.localRotation = Quaternion.Euler(x, y, 0);
    }

    public void playerHere()
    {
        playerNear = !playerNear;
    }
}
