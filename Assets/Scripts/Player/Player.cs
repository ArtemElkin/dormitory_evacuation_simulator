using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Hp { get; private set; }
    public int Oxygen { get; private set; }
    public int Temperature { get; private set; }

    public void GetDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Debug.Log("You died");
        }
    }
    public void Heal(int value)
    {
        Hp += value;
        if (Hp > 100)
            Hp = 100;
    }
}
