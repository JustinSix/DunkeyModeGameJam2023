using UnityEngine;

public class Damageable : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        // Implement damage logic here
        Debug.Log($"{gameObject.name} took {damage} damage!");
    }
}
