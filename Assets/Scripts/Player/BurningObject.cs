using UnityEngine;

public class BurningObject : MonoBehaviour {
    [SerializeField] private float minimumDistance = 2f;
    [SerializeField] private float damagePerTimeStep = 5f;
    [SerializeField] private float timeStep = .5f;

    private float elapsed = 0f;
    private PlayerHealth playerHealth = null;

    private void Awake() {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update() {
        if (elapsed > timeStep && Vector3.Distance(transform.position, playerHealth.transform.position) < minimumDistance) {
            playerHealth.TakeDamage(damagePerTimeStep);
            elapsed = 0;
        }
        else elapsed += Time.deltaTime;
    }
}
