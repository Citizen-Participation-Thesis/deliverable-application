using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	private static T _instance;

	private protected static T Instance {
		get {
			if (_instance != null) return _instance;
			
			_instance = (T)FindObjectOfType(typeof(T));
			
			if (_instance != null) return _instance;
			
			var goName = typeof(T).Name;			
			
			var go = GameObject.Find(goName);
			
			if (go == null) {
				go = new GameObject
				{
					name = goName
				};
			}
					
			_instance = go.AddComponent<T>();
			return _instance;
		}
	}
	
	public virtual void OnApplicationQuit()
	{
		// release reference on exit
		_instance = null;
	}
}
