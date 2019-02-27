using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thebitchwillnotmovewhatthefuck : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 100;
        print(GetComponent<Rigidbody>().velocity);
    }

    public void tester()
    {
        print("fuckall");
    }
}
