using UnityEngine;

// Singleton Templete class
/* 사용법
 * public class MyClass : Singleton<MyClass> {
 *      protected MyClass()
 *      {
 *      }
 * }*/
// 생성자에 protected 꼭 붙여주기!!
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    // Destroy 여부 확인용
    private static bool _ShuttingDown = false;
    private static object _Lock = new object();
    private static T _Instance;

    public static T Instance
    {
        get
        { 
            // 해당 싱글톤을 gameObject.Ondestory() 에서는 사용하지 않거나 사용한다면 null 체크
            if (_ShuttingDown)
            {
                Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                return null;
            }

            lock (_Lock)    //Thread Safe
            {
                if (_Instance == null)
                {
                    // 인스턴스 존재 여부 확인
                    _Instance = (T)FindObjectOfType(typeof(T));

                    // 아직 생성되지 않았다면 인스턴스 생성
                    if (_Instance == null)
                    {
                        var singletonObject = new GameObject();
                        _Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _Instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        _ShuttingDown = true;
    }

    private void OnDestroy()
    {
        _ShuttingDown = true;
    }
}