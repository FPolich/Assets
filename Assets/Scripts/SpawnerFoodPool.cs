using System.Collections.Generic;
using UnityEngine;

public class SpawnerFoodPool : MonoBehaviour
{
    [Header("Objeto a pool")]
    [SerializeField] private GameObject foodPrefab;

    [Header("Spawners")]
    [SerializeField] private Transform[] spawners;

    [Header("Parámetros")]
    [SerializeField] private int poolSize = 10;       // Tamaño del pool
    [SerializeField] private float cooldown = 2f;     // Tiempo entre spawns

    private Queue<GameObject> pool;
    private float currentTime;
    private int index = 0;

    void Awake()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(foodPrefab);
            obj.SetActive(false); // No aparece al inicio
            pool.Enqueue(obj);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= cooldown)
        {
            SpawnFood();
            currentTime = 0;
        }
    }

    void SpawnFood()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();

            obj.transform.position = spawners[index].position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);

            pool.Enqueue(obj);

            index++;
            if (index >= spawners.Length)
                index = 0;
        }
    }
}
