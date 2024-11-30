using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWhenDestroy : MonoBehaviour {
    [Tooltip("Every object tagged with this tag will trigger the destruction of this object")]
    [SerializeField] string triggeringTag;

   private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == triggeringTag && enabled) {
        Health.health--;

        if (Health.health < 0) {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
    }
}

    private void Update() {

    }
}