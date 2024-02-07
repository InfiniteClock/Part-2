using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject weapon;

    public void SpawnWeapon()
    {
        Instantiate(weapon);
    }

}
