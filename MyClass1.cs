using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyClass1
{
    [HideInInspector] public float elementHeight;
    public int myInt;
    public List<MyClass2> MyClass2List = new List<MyClass2>();
}
