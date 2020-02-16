using UnityEngine;
using System.Linq;

public class RespawnPointCollection : MonoBehaviour {

    public static Transform[] Waypoints { get; private set; }

    private void Awake() {
        Waypoints = GetComponentsInChildren<Transform>().Where(t => t.gameObject != gameObject).ToArray();
    }
}
