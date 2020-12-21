using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static GameObject[] Cubes;
    private void Awake()
    {
        Cubes = GameObject.FindGameObjectsWithTag("Cube");
        if (Cubes.Length == 169)
        {
            print("获取到所有的Cube!");
        }
        else { print("没有获取到Cube!"); }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
