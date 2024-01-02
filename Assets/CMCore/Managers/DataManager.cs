using System.Collections;
using System.IO;
using CMCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace CMCore.Managers
{
    public class DataManager
    {
        public string RemoteData { get; private set; }
        private int _attemptCount;
        public int Attempts => _attemptCount;

        public DataManager()
        {
            JsonConvert.DefaultSettings = () => Constants.Defaults.JsonSerializerSettings;
            GameManager.Instance.StartCoroutine(
                RequestForRemoteData(GameManager.Instance.Core.GameplaySettings.FullURL));
        }

        private IEnumerator RequestForRemoteData(string fullURL)
        {
            var requestTimeout = 1;
            using var request = UnityWebRequest.Get(fullURL);
            var requestOperation = request.SendWebRequest();
            if (!string.IsNullOrEmpty(RemoteData)) yield break;
            
            var elapsedTime = 0.0f;
            while (!requestOperation.isDone && elapsedTime < requestTimeout)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
                
            if (requestOperation.isDone &&
                request.result == UnityWebRequest.Result.Success)
            {
                RemoteData = request.downloadHandler.text;
                Debug.Log("<b><color=green>" + "Request is successfully retrieved!" + "</color></b>");
            }
            else
            {
                request.Abort();
                _attemptCount++;
                if (_attemptCount-1 < GameManager.Instance.Core.GameplaySettings.MaximumAttemptToRequestData)
                {
                    GameManager.Instance.StartCoroutine(RequestForRemoteData(fullURL));
                }
                Debug.Log("<b><color=red>" + "Web request failed. Error: " + request.error + "</color></b>");
            }
        }


        #region PlayerPrefs

        public static int GetPrefData(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static string GetPrefData(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static float GetPrefData(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static void SetPrefData(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static void SetPrefData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static void SetPrefData(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        #endregion


        private static T Deserialize<T>(object obj, string suffix)
        {
            return JsonConvert.DeserializeObject<T>(
                ReadFromFile(CombinePath(suffix + GetPropertyName(obj),
                    Constants.FileExtensions.Json)));
        }

        private static T Serialize<T>(object obj, string suffix)
        {
            var json = JsonConvert.SerializeObject(obj);
            var path = CombinePath(suffix + GetPropertyName(obj),
                Constants.FileExtensions.Json);
            WriteToFile(path, json);

            return Deserialize<T>(obj, suffix);
        }


        #region Utility

        public static T GetValueFromJson<T>(string json, string key, T defaultValue)
        {
            try
            {
                var jsonObject = JObject.Parse(json);
                var token = jsonObject[key];

                return token != null ? token.ToObject<T>() : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        
        public T GetRemoteValue<T>(string key, T defaultValue)
        {
            return GetValueFromJson(RemoteData, key, defaultValue);
        }
        
        private static string ReadFromFile(string path)
        {
            return File.ReadAllText(path);
        }

        private static void WriteToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        private static string GetPropertyName(object obj)
        {
            var objName = obj.GetType().Name;
            return objName;
        }

        public static bool CheckFileExists(string path)
        {
            return File.Exists(path);
        }

        private static bool CheckFileExistsByProperty(object obj, string suffix)
        {
            return CheckFileExists(CombinePath(suffix + GetPropertyName(obj),
                Constants.FileExtensions.Json));
        }

        private static string CombinePath(string fileName, string extension)
        {
            return Path.Combine(Application.persistentDataPath + Constants.Chars.Slash + fileName + extension);
        }

        #endregion
    }
}