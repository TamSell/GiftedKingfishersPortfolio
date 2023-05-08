using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public Vector3 direction;
    public float projSpeed = 0;
    Tracker objectTracker;

    private void Start()
    {
        objectTracker = GetComponent<Tracker>();
        Destroy(this.gameObject, 5);
    }
    void Update()
    {
        this.transform.position += direction * projSpeed * Time.deltaTime;
    }

    public void Shoot(Vector3 Idirection, float Ispeed)
    {
        this.projSpeed = Ispeed;
        this.direction = Idirection;
    }


}