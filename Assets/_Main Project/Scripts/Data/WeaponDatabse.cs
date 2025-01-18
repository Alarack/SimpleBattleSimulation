using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeponDatabse", menuName = "Scriptable Objects/WeponDatabse")]
public class WeaponDatabse : ScriptableObject
{

    public List<WeaponData> weaponData = new List<WeaponData>();
    


    public WeaponData GetRandomWeaponData() {
        return weaponData[Random.Range(0, weaponData.Count)];
    }


}
