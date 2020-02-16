using UnityEngine;

public class PlayerHealth : PlayerBehaviour, IHealth {
    private float health = 0;

    protected override void Begin() {
        health = settings.MaxHealth;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health > 0) return;
        Die();
    }

    public void Die() {
        float closest = float.MaxValue;
        Vector3? closestWaypoint = null;
        foreach (Transform t in RespawnPointCollection.Waypoints) {
            float distance = Vector3.Distance(t.position, transform.position);
            if (distance < closest) {
                closestWaypoint = t.position;
                closest = distance;
            }
        }
        transform.position = closestWaypoint.Value;
    }
}
