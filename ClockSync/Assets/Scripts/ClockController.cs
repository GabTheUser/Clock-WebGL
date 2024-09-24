using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ClockController : MonoBehaviour
{
    public ClockHandControl clockHandController;
    private string timeApiUrl = "https://worldtimeapi.org/api/timezone/Europe/Moscow";
    private DateTime currentTime;

    async void Start()
    {
        await GetInitialTimeFromServer();
        InvokeRepeating(nameof(UpdateTimeFromServer), 3600f, 3600f);
    }

    private async Task GetInitialTimeFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get(timeApiUrl);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            TimeResponse timeResponse = JsonUtility.FromJson<TimeResponse>(json);

            // Преобразуем строку времени из ISO 8601 формата в объект DateTime
            currentTime = DateTime.Parse(timeResponse.datetime);
            Debug.Log("Время получено: " + currentTime);

            clockHandController.SetInitialTime(currentTime);
        }
        else
        {
            Debug.LogError("Ошибка при получении времени с сервера.");
        }
    }

    // Обновление времени каждый час
    public async void UpdateTimeFromServer()
    {
        await GetInitialTimeFromServer();
    }
}

[Serializable]
public class TimeResponse
{
    public string datetime;  // строка с временем в формате ISO 8601
}
