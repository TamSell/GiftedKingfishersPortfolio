using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeed : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float timer;
    float timeToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame

    public void Update()
    {
        this.transform.Translate(0, 0, Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Destroy(gameObject);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            Destroy(gameObject);

    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }
        Destroy(gameObject);
    }
}
