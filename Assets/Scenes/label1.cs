using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class label1 : MonoBehaviour
{
    public int X;
    public int Y;
    public Kind kind = Kind.Nothing;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum Kind{
dog,
wall,
bone,
Nothing
}