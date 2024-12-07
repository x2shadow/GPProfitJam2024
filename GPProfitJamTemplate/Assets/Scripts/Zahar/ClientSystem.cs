using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawnSystem : MonoBehaviour
{
    Queue<GameObject> queue; // Очередь клиентов
    public int clientsNumber;
    public float spawnrate;

    [SerializeField] GameObject waitPoint;           // Белая точка
    [SerializeField] GameObject spawnPoint;          // Черная точка
    [SerializeField] List<GameObject> queuePoints;  // Красные точки
    
    GameObject waitingClient; // Клиент, ожидающий выдачи заказа

    int farthest_Free_queuePoints_Index; // Индекс самой дальней свободной точки

    [SerializeField] List<GameObject> clientsPrefabs; // Префабы клиентов

    void Start()
    {
        farthest_Free_queuePoints_Index = 0;
        queue = new Queue<GameObject>();

        StartCoroutine(SpawnVisitors());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) TakeOrder();
        if (Input.GetKeyDown(KeyCode.G)) GiveOrder();
    }

    IEnumerator SpawnVisitors() // Корутина для спавна клиентов
    {
        int i = 0;

        while (i < clientsNumber)
        {
            yield return new WaitForSeconds(spawnrate);

            if (farthest_Free_queuePoints_Index < queuePoints.Count)
            {
                int randomIndex = Random.Range(0, clientsPrefabs.Count); // Выбор случайного префаба клиента
                GameObject selectedPrefab = clientsPrefabs[randomIndex];

                GameObject client = Instantiate(selectedPrefab, spawnPoint.transform.position, Quaternion.identity);
                queue.Enqueue(client); // Добавляем клиента в очередь

                SmoothTranslate(client);

                farthest_Free_queuePoints_Index++;
                i++;
            }
            else
            {
                Debug.LogWarning("Все точки очереди заняты. Новый клиент не может быть создан.");
            }
        }
    }

    void SmoothTranslate(GameObject client) // Перемещение клиента на свободную точку
    {
        if (farthest_Free_queuePoints_Index < queuePoints.Count)
        {
            GameObject target = queuePoints[farthest_Free_queuePoints_Index];
            client.transform.position = target.transform.position;
        }
        else
        {
            Debug.LogError("Попытка перемещения клиента на недоступную точку.");
        }
    }

    void TakeOrder() // Удаление клиента из очереди и перемещение его на выдачу
    {
        if (queue.Count > 0 && waitingClient == null)
        {
            GameObject client = queue.Dequeue(); // Убираем первого клиента из очереди
            client.transform.position = waitPoint.transform.position;
            waitingClient = client;

            MoveQueue();
            farthest_Free_queuePoints_Index--;
            Debug.Log("Order Taken");
        }
        else
        {
            Debug.LogWarning(queue.Count == 0 ? "Очередь пуста." : "Место на выдаче уже занято.");
        }
    }

    void MoveQueue() // Сдвиг очереди на одну позицию вперед
    {
        if (queue.Count > 0)
        {
            int i = 0;
            foreach (GameObject client in queue)
            {
                if (i < queuePoints.Count)
                {
                    client.transform.position = queuePoints[i].transform.position;
                }
                else
                {
                    Debug.LogError("Попытка сдвига очереди с превышением индекса.");
                }
                i++;
            }
        }
        else
        {
            Debug.Log("Очередь пуста, сдвиг не требуется.");
        }
    }

    void GiveOrder() // Выдача заказа клиенту
    {
        if (waitingClient != null)
        {
            Destroy(waitingClient);
            waitingClient = null;
            Debug.Log("Order Given");
        }
        else
        {
            Debug.LogWarning("Нет клиента на выдаче для выдачи заказа.");
        }
    }
}
