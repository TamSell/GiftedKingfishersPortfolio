using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeed : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float BulletDrop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        this.transform.Translate(0, 0 , Time.deltaTime* speed);
      
        
    }
}
