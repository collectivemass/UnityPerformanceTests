using UnityEngine;

public class localVariableAssignment : MonoBehaviour {

    public Transform assetToRotate;

    private void Update () {
        RotateWithLocalAssignment();
    }

    private void RotateWithLocalAssignment(){
        Vector3 eularRotation = assetToRotate.localEulerAngles;
        eularRotation.y += Time.smoothDeltaTime * 10f;
        assetToRotate.localEulerAngles = eularRotation;
    }
}
