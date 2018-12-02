using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CheckpointSkipper))]
public class SkipCheckpointButton : Editor {

	public override void OnInspectorGUI(){

		base.OnInspectorGUI ();

		CheckpointSkipper skipper = (CheckpointSkipper)target;

		if(GUILayout.Button("Next")){
			skipper.NextCheckpoint ();
		}

		if(GUILayout.Button("Previous")){
			skipper.PreviousCheckpoint ();
		}
	}

}
