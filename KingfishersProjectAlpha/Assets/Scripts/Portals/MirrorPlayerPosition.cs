using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPlayerPosition : MonoBehaviour
{
    [SerializeField] GameObject current;
    [SerializeField] GameObject mirror;
    [SerializeField] GameObject TransportObject;
    [SerializeField] Transform playerPos;
    [SerializeField] Collider Transport;

    public Vector3 playerRelation;
    public Vector3 localPos;
    public bool playerNear;
    public bool active;

    private void Update()
    {
        if(playerNear)
        {
            StartCoroutine(MirrorIt());
        }
    }

    IEnumerator MirrorIt()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, 10))
        {
            localPos = hit.point;
        }
        mirror.transform.localPosition = localPos;
        current.transform.localPosition = localPos;
        yield return new WaitForEndOfFrame();
    }

    public void PlayerNear()
    {
        playerNear = !playerNear;
    }
}
