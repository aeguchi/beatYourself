using Academy.HoloToolkit.Sharing;
using Academy.HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
public class UIscript : Singleton<UIscript>
{
    /// <summary>
    /// Tracks if we have been sent a transform for the model.
    /// The model is rendered relative to the actual anchor.
    /// </summary>



    void Start()
    {
        // Start by making the model as the cursor.
        // So the user can put the hologram where they want.
        GestureManager.Instance.OverrideFocusedObject = this.gameObject;
    }


    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
    }

    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        //Vector3 retval = Camera.main.transform.position + (new Vector3(2, 0, 0)).normalized * 2;//Camera.main.transform.forward * 2;//(new Vector3(2,0,0));//
        Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2 + (new Vector3(0, 1, 0)).normalized;

        return retval;
    }

    public void ResetStage()
    {
        // We'll use this later.
    }
}