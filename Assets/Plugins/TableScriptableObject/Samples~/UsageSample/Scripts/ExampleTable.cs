using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Feverfew.TableScriptableObject.Samples.UsageSample
{
    [System.Serializable]
    public class Data
    {
        public string stringField;
        public bool booleanField;
        public int integerField;
        public Vector3 vector3Field;
        public GameObject ObjectField;
        public Test test;
    }

    [System.Serializable]
    public class Test
    {
        public string testValue;
    }


    [CreateAssetMenu(fileName = "ExampleTable", menuName = "TableScriptableObjects/ExampleTable")]
    public class ExampleTable : TableScriptableObject<Data>
    {
    }
}