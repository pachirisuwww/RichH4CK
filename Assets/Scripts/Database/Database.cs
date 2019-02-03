using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Database : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public string name;
        public Color color = Color.white;
        public Sprite Icon;
    }
    public Data[] datas;
    public Color[] CustomColor;
}
