using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static readonly object @lock = new object();

	public static T Instance
	{
		get
		{
			lock (@lock)
			{
				if (_instance == null)
				{
					_instance = (T)FindObjectOfType(typeof(T));

					if (FindObjectsOfType(typeof(T)).Length > 1)
					{
						Debug.LogError("[Singleton] Something went really wrong " +
									   " - there should never be more than 1 singleton!" +
									   " Reopening the scene might fix it.");
						return _instance;
					}

					if (_instance == null)
					{
						var singleton = new GameObject();
						_instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) " + typeof(T);

						DontDestroyOnLoad(singleton);
					}
				}

				return _instance;
			}
		}
	}
}
