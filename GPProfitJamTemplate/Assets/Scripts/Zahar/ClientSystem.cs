using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawnSystem : MonoBehaviour
{
    Queue<GameObject> queue; // Очередь клиентов
    public int clientsNumber;
    public int clientsDone; //Количество обслуженных клиентов
    public float spawnrate;

    [SerializeField] LevelManager levelManager;
    
    [SerializeField] List<GameObject> clientsPrefabs; // Префабы клиентов

    [SerializeField] GameObject waitPoint;           // Точка ожидания заказа
    [SerializeField] GameObject spawnPoint;          // Точка спавна
    [SerializeField] List<GameObject> queuePoints;  //  Точки в очереди
    
    GameObject waitingClient; // Клиент, ожидающий выдачи заказа

    int nextFreeQueuePointIndex; // Индекс самой дальней свободной точки


    void Start()
    {
        if (clientsPrefabs == null || clientsPrefabs.Count == 0)
        {
            Debug.LogError("Список префабов клиентов пуст. Добавьте хотя бы один префаб в 'clientsPrefabs'.");
        }

        if (queuePoints == null || queuePoints.Count == 0)
        {
            Debug.LogError("Список точек очереди пуст. Проверьте настройки.");
        }

        nextFreeQueuePointIndex = 0;
        queue = new Queue<GameObject>();

        StartCoroutine(Spawner());
    }

    IEnumerator Spawner() // Корутина для спавна клиентов
    {
        int i = 0;

        while (i < clientsNumber)
        {
            // Если все точки заняты, ждем, пока освободится место
            while (nextFreeQueuePointIndex >= queuePoints.Count)
            {
                Debug.Log("[ClientSpawnSystem]: Все точки заняты. Ожидание освобождения...");
                yield return new WaitForSeconds(5f); // Ждем 5 секунд перед повторной проверкой
            }

            yield return new WaitForSeconds(spawnrate);

            // Спавним клиента, если есть свободная точка
            int randomIndex = Random.Range(0, clientsPrefabs.Count); // Выбор случайного префаба клиента
            GameObject selectedPrefab = clientsPrefabs[randomIndex];

            GameObject client = Instantiate(selectedPrefab, spawnPoint.transform.position, Quaternion.identity);

            // Назначаем случайное блюдо
            Client clientComponent = client.GetComponent<Client>();
            if (clientComponent != null)
            {
                DishType randomDish = (DishType)Random.Range(1, 5); // от 1 до 5 в enum DishType
                clientComponent.dishType = randomDish;
                Debug.Log($"Клиенту назначено блюдо: {clientComponent.dishType}");
            }
            else
            {
                Debug.LogError("Компонент 'Client' не найден у спавнимого клиента.");
            }

            queue.Enqueue(client); // Добавляем клиента в очередь

            SmoothTranslate(client); // Перемещаем клиента на свободную точку
            nextFreeQueuePointIndex++;
            i++; // Увеличиваем счетчик клиентов
        }
    }

    void SmoothTranslate(GameObject client) // Перемещение клиента на свободную точку
    {
        if (nextFreeQueuePointIndex < queuePoints.Count)
        {
            GameObject target = queuePoints[nextFreeQueuePointIndex];
            client.transform.position = target.transform.position;
        }
        else
        {
            Debug.LogError("[ClientSpawnSystem]: Попытка перемещения клиента на недоступную точку.");
        }
    }

    public void OrderTaken() // Удаление клиента из очереди и перемещение его на выдачу
    {
        if (queue.Count > 0 && waitingClient == null)
        {
            GameObject client = queue.Dequeue(); // Убираем первого клиента из очереди
            client.transform.position = waitPoint.transform.position;
            waitingClient = client;

            MoveQueue();
            nextFreeQueuePointIndex = Mathf.Max(0, nextFreeQueuePointIndex - 1); // Освобождаем одну точку
            Debug.Log("Order Taken");
        }
        else
        {
            Debug.LogWarning(queue.Count == 0 ? "Очередь пуста." : "Место на выдаче уже занято.");
        }
    }

    void MoveQueue() // Сдвиг очереди на одну позицию вперед
    {
        if (queue.Count > queuePoints.Count)
        {
            Debug.LogError("[ClientSpawnSystem]: Очередь превышает количество точек.");
            return;
        }

        int i = 0;
        foreach (GameObject client in queue)
        {
            client.transform.position = queuePoints[i].transform.position;
            i++;
        }
    }

    public void OrderGiven() // Выдача заказа клиенту
    {
        if (waitingClient != null)
        {
            clientsDone++;

            Destroy(waitingClient);
            waitingClient = null;
            

            if(clientsDone == clientsNumber) levelManager.CompleteLevel();
        }
        else
        {
            Debug.LogWarning("Нет клиента на выдаче для выдачи заказа.");
        }
    }
}
