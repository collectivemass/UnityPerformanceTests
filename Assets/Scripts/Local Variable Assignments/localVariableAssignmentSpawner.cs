using UnityEngine;

public class localVariableAssignmentSpawner : MonoBehaviour {

    public GameObject assetToSpawn;
    public int totalInstances = 10000;
    public float radius = 20;

    private void Awake () {

        for(int i=0; i<totalInstances; i++){
            Transform instance = Instantiate<GameObject>(assetToSpawn).transform;
            instance.parent = transform;
            instance.localPosition = Random.onUnitSphere * radius;
        }
    }
}
