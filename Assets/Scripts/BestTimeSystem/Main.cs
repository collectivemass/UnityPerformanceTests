/******************************************************************
 * BEST TIME/R TEST
 * In this test we are going to figuring out the best time tracking
 * method for the most accurate analytics. We are mainly interested
 * in in intervals. we are only looking for millisecond accuracy.
 * We will be evaluating:
 * Stopwatch
 * Time.time
 * Time.realtime
 * DateTime
 * DateTimeOffset
 *
 * We will be testing with and with out renderables on screen to
 * see the impact of engine stress on time accuracy
 *****************************************************************/
using System.IO;
using UnityEngine;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

//*********************************
//* NAMESPACE
//*********************************
namespace UnityTests.BestTimeSystem {

	//*********************************
	//* CLASS
	//*********************************
	public class Main : MonoBehaviour {

		//*********************************
		//* STRUCTS
		//*********************************
		public struct TimeSlice {
			public float time;
			public float realtime;
			public long dateTimeOffset;
			public long windowsFileTime;
			public long stopWatch;
		}

		//*********************************
		//* CONSTANTS
		//*********************************
		private const int SAMPLES = 1000;

		//*********************************
		//* PROPERTIES
		//*********************************

		//*********************************
		//* VARIABLES
		//*********************************
		private bool testStarted = false;

		private Stopwatch stopwatch = new Stopwatch();

		private TimeSlice[] samples = new TimeSlice[SAMPLES];
		private int samplesIndex = 0;

		//*********************************
		//* UNITY MESSAGES
		//*********************************
		private void Awake(){
			Create();
		}

		private void Start() {
			testStarted = true;
			stopwatch.Start();
		}

		private void Update() {
			if (testStarted) {

				if (samplesIndex < samples.Length) {
					samples[samplesIndex].time = Time.time;
					samples[samplesIndex].realtime = Time.realtimeSinceStartup;
					samples[samplesIndex].dateTimeOffset = DateTimeOffset.Now.ToUnixTimeMilliseconds();
					samples[samplesIndex].windowsFileTime = DateTime.Now.ToFileTime();
					samples[samplesIndex].stopWatch = stopwatch.ElapsedMilliseconds;
					samplesIndex++;
				} else {
					testStarted = false;
					WriteMetrics();
				}
			}
		}
		private void OnDestroy() {
		}

		//*********************************
		//* MAIN METHODS
		//*********************************
		private void Create(){

			for (int i=0; i<SAMPLES; i++) {
				samples[i] = new TimeSlice();
			}
		}

		//*********************************
		//* TIME & LOGGING METHODS
		//*********************************
		private void WriteMetrics() {
#if UNITY_EDITOR
			string filePath = Application.dataPath + "/output.txt";
#else
			string filePath = Application.persistentDataPath + "/output.txt";
#endif


			StreamWriter metricStream = File.CreateText(filePath);
			Debug.Log(filePath);

			string output = string.Empty;

			// Skip the first slice to allow stop watch to be accurate
			for (int i=2; i<samples.Length; i++) {
				output = string.Empty;
				output += ((samples[i].time - samples[i - 1].time) * 1000).ToString() + "\t";
				output += ((samples[i].realtime - samples[i - 1].realtime) * 1000).ToString() + "\t";
				output += (samples[i].dateTimeOffset - samples[i - 1].dateTimeOffset).ToString() + "\t";
				output += ((samples[i].windowsFileTime - samples[i - 1].windowsFileTime) * 0.0001).ToString() + "\t";
				output += (samples[i].stopWatch - samples[i - 1].stopWatch).ToString() + "\t";
				metricStream.WriteLine(output);
			}

			metricStream.Close();
			metricStream = null;
		}
	}
}