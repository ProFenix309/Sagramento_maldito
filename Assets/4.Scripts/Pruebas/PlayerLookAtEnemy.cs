using UnityEngine;

public class PlayerLookAtEnemy : MonoBehaviour
{
    [Header("Look At Settings")]
    public float lookSpeed = 5f; // Velocidad de rotación
    public bool isBeingAttacked = false; // Se activará cuando el enemigo ataque

    private Transform currentEnemy; // Enemigo que está atacando
    private Quaternion originalRotation; // Rotación original del jugador

    void Update()
    {
        if (isBeingAttacked && currentEnemy != null)
        {
            // Calcula la dirección hacia el enemigo
            Vector3 direction = (currentEnemy.position - transform.position).normalized;
            direction.y = 0; // Mantiene la rotación solo en el eje Y (horizontal)

            // Calcula la rotación objetivo
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Rota suavemente hacia el enemigo
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
        }
    }

    // Método llamado por el enemigo cuando comienza a atacar
    public void StartLookingAtEnemy(Transform enemy)
    {
        currentEnemy = enemy;
        isBeingAttacked = true;
        originalRotation = transform.rotation;
    }

    // Método llamado cuando el ataque termina
    public void StopLookingAtEnemy()
    {
        isBeingAttacked = false;
        currentEnemy = null;
    }
}