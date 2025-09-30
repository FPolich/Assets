using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f; // Tiempo que el objeto permanece activo

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
