using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private void OnParticleTrigger()
    {


        Debug.Log("Particle Hit");
        Destroy(gameObject);
    }
}
