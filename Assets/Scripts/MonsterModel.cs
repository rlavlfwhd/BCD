using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "ScriptableObjects/MonsterModel")]
public class MonsterModel : ScriptableObject
{
    public int MonsterNumber;

    public Stats stats;

}
