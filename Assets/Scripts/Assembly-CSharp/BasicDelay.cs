using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicDelay : MonoBehaviour
{
    public float delay;
    public UnityEvent toInvoke;
    public bool enable;

    private void Start() => StartCoroutine(this.delayIE(delay, false));
    private void OnEnable() => StartCoroutine(this.delayIE(delay, true));
    private IEnumerator delayIE(float delay, bool enable)
    {
        if (this.enable != enable) yield return null;
        else
        {
            yield return new WaitForSeconds(delay);
            toInvoke.Invoke();
        }
    }
}
