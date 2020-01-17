//*********************************
//* MAIN CONTROLLER CLASS FOR TEST
//*********************************
using UnityEngine;

//*********************************
//* NAMESPACE
//*********************************
namespace UnityTests.BestTimeSystem {

	//*********************************
	//* CLASS
	//*********************************
	public class Rotate : MonoBehaviour {

		//*********************************
		//* CONSTANTS
		//*********************************

		//*********************************
		//* PROPERTIES
		//*********************************

		//*********************************
		//* VARIABLES
		//*********************************
		private Vector3 offset;

		//*********************************
		//* UNITY MESSAGES
		//*********************************
		private void Awake(){
			offset.x = Random.Range(-40, 40);
			offset.y = Random.Range(-40, 40);
			offset.z = Random.Range(-40, 40);
		}

		private void Update() {
			Vector3 rotation = transform.eulerAngles;
			rotation += offset * Time.deltaTime;
			transform.eulerAngles = rotation;
		}

		//*********************************
		//* MAIN METHODS
		//*********************************
	}
}