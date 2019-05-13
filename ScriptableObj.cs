using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class ScriptableObj : ScriptableObject
{
    public List<MyClass1> MyClass1List = new List<MyClass1>();
}
