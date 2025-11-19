using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class NavMeshItemSpawner : MonoBehaviour
{
    [Header("Listas de Prefabs")]
    public List<GameObject> prefabList;

    public int cantidadAInstanciar;

    [Header("Área de Spawn")]
    public Vector3 tamanoDelArea; 

    public float distanciaMaxBusqueda = 50.0f;

    void OnEnable()
    {
        if (prefabList.Count == 0)
        {
            Debug.LogWarning("La lista de prefabs está vacía.");
            return;
        }

        GenerarItems();
    }

    void GenerarItems()
    {
        int contador = 0;

        Vector3 centro = transform.position;

        while (contador < cantidadAInstanciar)
        {
            float posX = Random.Range(centro.x - tamanoDelArea.x / 2, centro.x + tamanoDelArea.x / 2);
            float posY = centro.y;
            float posZ = Random.Range(centro.z - tamanoDelArea.z / 2, centro.z + tamanoDelArea.z / 2);

            Vector3 puntoAleatorio = new Vector3(posX, posY, posZ);

            NavMeshHit hit;

            if (NavMesh.SamplePosition(puntoAleatorio, out hit, distanciaMaxBusqueda, NavMesh.AllAreas))
            {
                int prefabIndex = Random.Range(0, prefabList.Count);
                GameObject prefabParaInstanciar = prefabList[prefabIndex];

                Instantiate(prefabParaInstanciar, hit.position, Quaternion.identity);

                contador++;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.5f); 
        Gizmos.DrawCube(transform.position, tamanoDelArea);
    }
}