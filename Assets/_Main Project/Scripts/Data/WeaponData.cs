using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeponData")]
public class WeaponData : ScriptableObject
{

    public List<StatData> statData = new List<StatData>();

    public Projectile payload;


    public string weaponName;
    public string weaponDescription;



}
