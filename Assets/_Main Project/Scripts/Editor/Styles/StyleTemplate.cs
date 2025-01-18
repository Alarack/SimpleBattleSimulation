using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Style Template", menuName = "Scriptable Objects/Editor Styles/Style Template")]

public class StyleTemplate : ScriptableObject
{
    public GUIStyle style;
    public Color fontColor = Color.white;
    public int fontSize;
    public bool bold;

}
