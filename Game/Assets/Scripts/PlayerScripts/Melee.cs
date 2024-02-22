using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public void Attack()
    {
        GetComponent<Animator>().Play("Swing");
    }
}
