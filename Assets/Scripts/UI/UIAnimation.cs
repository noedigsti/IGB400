using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimation : UIBaseBehaviour
{
	public Vector3 punchScale = new Vector3(0.3f,0.3f,0.3f);
	public float duration = 0.8f;
	public int vibrato = 6;
	public float elastic = 0.7f;
    public Button button;
    public Transform buttonContainer;
    public EventTrigger eventTrigger;

    void OnEnable() {
		button = GetComponent<Button>();
		eventTrigger = GetComponent<EventTrigger>();
		buttonContainer = transform;
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener((data) => OnStartPress());
		eventTrigger.triggers.Add(entry);
    }
    void OnDisable()
    {
        foreach(var trigger in eventTrigger.triggers) {
			trigger.callback.RemoveAllListeners();
        }
    }
	private void AnimateButton(int index,float delay) {
		List<Sequence> animationSequences = new List<Sequence>();
		if(animationSequences.Count <= index) {

			// Add +1
			animationSequences.Add(DOTween.Sequence());
		} else {
			if(animationSequences[index].IsPlaying()) {
				animationSequences[index].Kill(true);
			}
		}

		var seq = animationSequences[index];
		//var button = buttons[index];

		seq.Append(button.transform.DOScale(1,0.1f));
		seq.Append(button.transform.DOPunchScale(
			punchScale, // scale
			duration, // duration
			vibrato, // vibrato
			elastic // elasticity
			).SetEase(Ease.OutCirc));
		seq.PrependInterval(delay);
	}


	public void OnStartPress() {
		AnimateButton(0,0);
	}

}
