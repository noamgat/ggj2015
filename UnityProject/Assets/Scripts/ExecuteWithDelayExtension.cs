using UnityEngine;
using System.Collections;

public static class ExecuteWithDelayExtension {

    public static void ExecuteWithDelay(this MonoBehaviour behaviour, float seconds, System.Action action) {
        behaviour.StartCoroutine(ExecuteWithDelayCorutine(seconds, action));
    }

    private static IEnumerator ExecuteWithDelayCorutine(float seconds, System.Action action) {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
