using UnityEngine;
using System.Collections;
using System;

public class NetworkService : MonoBehaviour {

    private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml";
    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us";

    private const string webImage = "https://en.wikipedia.org/wiki/File:Moraine_Lake_17092005.jpg";

    private const string localApi = "http://localhost/ch9/api.php";

    private bool IsResponseValid(WWW www) // Лучше использовать UnityWebRequest framework
    {
        if(www.error != null)
        {
            Debug.Log("bad connection");
            return false;
        }
        else if(string.IsNullOrEmpty(www.text))
        {
            Debug.Log("bad data");
            return false;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator CallApi(string url, Hashtable args, Action<string> callback)
    {
        WWW www;

        if (args == null)
        {
            www = new WWW(url);
        }
        else
        {
            WWWForm form = new WWWForm(); // Отправляем аргументы вместе с объектом WWW с помощью объекта WWWForm.
            foreach (DictionaryEntry arg in args)
            {
                form.AddField(arg.Key.ToString(), arg.Value.ToString());
            }
            www = new WWW(url, form); // Объект WWWForm автоматически меняет запрос GET на POST
        }

        yield return www; // Пауза в процессе скачивания

        if(!IsResponseValid(www))
        {
            yield break; // Прерывание сопрограммы в случае ошибки
        }

        callback(www.text); // Делегат может быть вызван так же, как и исходная функция
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        return CallApi(xmlApi, null, callback); // Каскад ключевых слов yeld в вызывающих друг друга методах сопрограммы
    }

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallApi(jsonApi, null, callback);
    }

    public IEnumerator DownLoadImage(Action<Texture2D> callback) // Принимает объекты типа Texture2D
    {
        WWW www = new WWW(webImage);

        yield return www;

        callback(www.texture);
    }


    public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback)
    {
        Hashtable args = new Hashtable(); // Определяем таблицу отправляемых аргументов
        args.Add("message", name);
        args.Add("cloud_value", cloudValue);
        args.Add("timestamp", DateTime.UtcNow.Ticks); // Отправляем метку времени вместе с информацией об облачности

        return CallApi(localApi, args, callback);
    }
}
