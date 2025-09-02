using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFood : MonoBehaviour
{
    public GameObject food;
    public Transform[] spawner;
    private float currentTime;
    int i = 0;
    [SerializeField] private float cooldown = 2f;
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= cooldown)
        {
            Instantiate(food, spawner[i].position, Quaternion.identity);
            currentTime = 0;
            i++;
        }
        if (i == spawner.Length)
        {
            i = 0;
        }
    }
}
