using Academy.HoloToolkit.Sharing;
using Academy.HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
public class HologramPlacement : Singleton<HologramPlacement>
{
    /// <summary>
    /// Tracks if we have been sent a transform for the model.
    /// The model is rendered relative to the actual anchor.
    /// </summary>
    public bool GotTransform { get; private set; }

    public List<float> recordedCoordinates = new List<float>();
    public bool isRecording = false;
    public bool isPlaying = false;
    public bool hasBeenRecorded = false;
    public int frameCount = 0;
    public float timeOut = 0.1f;
    public float timeElapsed = 0.0f;
    public float remTime = 0.0f;
    public float recTime = 10.0f;
    Vector3 newPosition;



    void Start()
    {
        // Start by making the model as the cursor.
        // So the user can put the hologram where they want.
        GestureManager.Instance.OverrideFocusedObject = this.gameObject;
    }


    void startRecording()
    {
        isRecording = true;
        timeElapsed = 0.0f;
        remTime = recTime;
        Debug.Log("Recoring Started");
    }

    void startPlaying()
    {
        timeElapsed = 0.0f;
        isPlaying = true;
        frameCount = 0;
        newPosition = new Vector3(recordedCoordinates[frameCount], recordedCoordinates[frameCount + 1], recordedCoordinates[frameCount + 2]) + (new Vector3(0, 0, 2)).normalized * 2;
        frameCount += 3;
        Debug.Log("Playing Started");
    }




    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (isPlaying)
        {
            if (timeElapsed >= timeOut)
            {
                newPosition = new Vector3(recordedCoordinates[frameCount], recordedCoordinates[frameCount + 1], recordedCoordinates[frameCount + 2]) + (new Vector3(0, 0, 2)).normalized * 2;
                
                frameCount += 3;
                if (frameCount >= recordedCoordinates.Count)
                {
                    isPlaying = false;
                    frameCount = 0;
                    Debug.Log("Playing Ended");
                }
                timeElapsed = 0;
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.2f);
        }
        else {
            if (!GotTransform)
            {
                //transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
            }
            if (isRecording)
            {
                //recordedCoordinates.Add(transform.position.x);
                //recordedCoordinates.Add(transform.position.y);
                //recordedCoordinates.Add(transform.position.z);




                
                if (timeElapsed >= timeOut)
                {
                    recordedCoordinates.Add(Camera.main.transform.position.x);
                    recordedCoordinates.Add(Camera.main.transform.position.y);
                    recordedCoordinates.Add(Camera.main.transform.position.z);
                    timeElapsed = 0.0f;
                }

                remTime -= Time.deltaTime;
                if (remTime < 0)
                {
                    isRecording = false;
                    hasBeenRecorded = true;
                    Debug.Log("Recording Ended");
                }


                //recordedCoordinates.Add(Camera.main.transform.position.x);
                //recordedCoordinates.Add(Camera.main.transform.position.y);
                //recordedCoordinates.Add(Camera.main.transform.position.z);
                //frameCount -= 3;
                //if (frameCount < 0)
                //{
                //    isRecording = false;
                //    hasBeenRecorded = true;
                //    Debug.Log("Ended recording");
                //}
            }
        }
    }

    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        //Vector3 retval = Camera.main.transform.position + (new Vector3(2, 0, 0)).normalized * 2;//Camera.main.transform.forward * 2;//(new Vector3(2,0,0));//
        Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2;

        return retval;
    }

    public void OnSelect()
    {
        if (!hasBeenRecorded)
        {
            startRecording();
        }
        else
        {
            startPlaying();
        }




        //// Note that we have a transform.
        //GotTransform = true;

        //// The user has now placed the hologram.
        //// Route input to gazed at holograms.
        //GestureManager.Instance.OverrideFocusedObject = null;
    }

    public void ResetStage()
    {
        // We'll use this later.
    }
}