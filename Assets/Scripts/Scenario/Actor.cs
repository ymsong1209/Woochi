using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plot_", menuName = "Scriptable Objects/Actor")]
public class Actor : ScriptableObject
{
    public string actorName;
    public Sprite actorImage;
}
