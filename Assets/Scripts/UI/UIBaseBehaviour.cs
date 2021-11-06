using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIBaseBehaviour : MonoBehaviour
{
	public Coroutine Invoke(Action action,float time) {
		return StartCoroutine(InvokeAfterTime(action,time));
	}

	private IEnumerator InvokeAfterTime(Action action,float time) {
		yield return new WaitForSeconds(time);

		action?.Invoke();
	}
}
