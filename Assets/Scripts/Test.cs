using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Student
{
    public string name;
    public Dictionary<string, Child> childs;
}

[Serializable]
public class Child
{
    public int age;
}

public class Test : MonoBehaviour
{
    [ContextMenu("func")]
    public void Func()
    {
        var s = new Student();
        s.name = "zhangsan";
        s.childs = new Dictionary<string, Child>();
        Child c = new Child();
       c.age = 1;
        s.childs.Add("zhangsan", c);

        string output = JsonUtility.ToJson(s);

    }
}
