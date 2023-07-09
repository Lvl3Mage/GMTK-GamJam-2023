using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : Monster
{

	[SerializeField] Tentacle[] tentacles;
	protected override void ToggleEmerged(bool toggleVal){
		foreach(Tentacle tentacle in tentacles){
			tentacle.ToggleState(toggleVal);
		}
	}
	protected override void OnTargetDetected(Transform target){
		foreach(Tentacle tentacle in tentacles){
			tentacle.SetTarget(target);
		}
	}
}
