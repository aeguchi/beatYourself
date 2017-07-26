using Academy.HoloToolkit.Sharing;
using Academy.HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;
public class HologramPlacement : Singleton<HologramPlacement>
{
    /// <summary>
    /// Tracks if we have been sent a transform for the model.
    /// The model is rendered relative to the actual anchor.
    /// </summary>
    public bool GotTransform { get; private set; }

    public List<float> recordedCoordinates = new List<float>();
    public List<float> recordedRotations = new List<float>();
    public float recTime = 10.0f;
    public Text UILeft;
    public Text UIRight;
    public Text UITopLeft;
    public Text UITopRight;
    public Text UIBottomLeft;
    public Text UIBottomRight;

    private bool isRecording = false;
    private bool isPlaying = false;
    private bool hasBeenRecorded = false;
    private int frameCount = 0;
    private int frameCountRot = 0;
    private float timeOut = 0.1f;
    private float timeElapsed = 0.0f;
    private float remTime = 0.0f;
    private Vector3 newPosition;
    private Quaternion newRotation;
    private float maxVal = 0;
    private float minVal = -100;
    private float diff_y_squat = 0;
    private float initialY = 0;
    private bool wentLowEnough_ghost = false;
    private bool wentHighEnough_ghost = true;
    private bool wentLowEnough_you = false;
    private bool wentHighEnough_you = true;
    private int squatCount_ghost = 0;
    private int squatCount_you = 0;



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


    void loadData()
    {
        hasBeenRecorded = true;
        recordedCoordinates = new List<float>() { 0.01206034f, 0.06984471f, 0.670199f, 0.01201819f, 0.06966318f, 0.670507f, 0.01058976f, 0.06946477f, 0.6703635f, 0.008883894f, 0.07890862f, 0.6727313f, 0.007768104f, 0.0852948f, 0.6741968f, 0.00900073f, 0.08477446f, 0.6776198f, 0.01170336f, 0.08479812f, 0.6829865f, 0.01285503f, 0.08119722f, 0.6927516f, 0.01251031f, 0.0412677f, 0.7089494f, 0.01284064f, -0.01940352f, 0.7233171f, 0.01648629f, -0.1035677f, 0.7427977f, 0.01990653f, -0.1512419f, 0.750858f, 0.0235093f, -0.1788517f, 0.7444286f, 0.02588956f, -0.1766243f, 0.7315692f, 0.02621924f, -0.1226582f, 0.7157792f, 0.02566298f, -0.02564939f, 0.7031335f, 0.02464782f, 0.1044705f, 0.7016189f, 0.0254147f, 0.1124182f, 0.7028139f, 0.02313925f, 0.0973525f, 0.7082289f, 0.02166977f, 0.07106984f, 0.7125947f, 0.01957812f, -0.01773788f, 0.7255902f, 0.01602636f, -0.1027534f, 0.7387239f, 0.01467177f, -0.1769676f, 0.7514982f, 0.01696535f, -0.2016478f, 0.756959f, 0.01918586f, -0.2090059f, 0.7565135f, 0.0207025f, -0.1797305f, 0.7532884f, 0.02148624f, -0.1105071f, 0.7475706f, 0.02194282f, 0.0112218f, 0.7414955f, 0.02057566f, 0.1266014f, 0.7376f, 0.01861806f, 0.1093453f, 0.7326989f, 0.01382256f, 0.09644806f, 0.7382241f, 0.01169856f, 0.05642688f, 0.7474539f, 0.01267572f, -0.04766515f, 0.7625253f, 0.01505712f, -0.1530443f, 0.7755615f, 0.02374604f, -0.2044956f, 0.782451f, 0.02884009f, -0.2134509f, 0.7756147f, 0.03328969f, -0.1774044f, 0.7648339f, 0.03524524f, -0.08538345f, 0.7474033f, 0.03131928f, 0.02420587f, 0.7330813f, 0.02664674f, 0.133853f, 0.7182459f, 0.02552254f, 0.1109663f, 0.7102163f, 0.02365318f, 0.1027991f, 0.7109686f, 0.02135311f, 0.08347684f, 0.7109847f, 0.02071104f, 0.02873575f, 0.7227759f, 0.0233737f, -0.0935653f, 0.7503935f, 0.02949646f, -0.2090346f, 0.7753606f, 0.03267598f, -0.2492055f, 0.7847878f, 0.03445987f, -0.2476071f, 0.782966f, 0.0338287f, -0.2117265f, 0.7748765f, 0.03165252f, -0.1223077f, 0.7585266f, 0.02500076f, -0.0084031f, 0.7471143f, 0.01749072f, 0.1220503f, 0.7368337f, 0.01749079f, 0.1180334f, 0.7242148f, 0.0172502f, 0.09933832f, 0.7182983f, 0.01632613f, 0.06996759f, 0.7195994f, 0.02057751f, -0.03812206f, 0.7315159f, 0.02449392f, -0.1472359f, 0.7468597f, 0.02870702f, -0.2184584f, 0.757103f, 0.02977781f, -0.230114f, 0.7566361f, 0.02786859f, -0.203306f, 0.7525131f, 0.0262265f, -0.1266966f, 0.7406092f, 0.02560322f, 0.02008986f, 0.7327654f, 0.02267622f, 0.109147f, 0.7301511f, 0.02026654f, 0.1248332f, 0.7207392f, 0.01912269f, 0.1048988f, 0.7177121f, 0.02036954f, 0.09358568f, 0.7233176f, 0.02074845f, 0.04943609f, 0.737757f, 0.02123045f, -0.08324629f, 0.7642417f, 0.02186699f, -0.2030747f, 0.7930845f, 0.0247667f, -0.2367079f, 0.8006949f, 0.0208088f, -0.2225375f, 0.795325f, 0.01593412f, -0.1632548f, 0.7880287f, 0.007210365f, -0.02013913f, 0.7756926f, -0.0004477929f, 0.1042927f, 0.7693979f, -0.001404874f, 0.1181403f, 0.769055f, -0.00128028f, 0.09967579f, 0.7703599f, 0.00258388f, 0.08739079f, 0.7814994f, 0.006167918f, 0.04098198f, 0.7939001f, 0.007833129f, -0.07506701f, 0.8129814f, 0.01052279f, -0.2039389f, 0.82776f, 0.01099266f, -0.2349693f, 0.8301375f, 0.009826265f, -0.2089235f, 0.8235154f, 0.006119341f, -0.1311802f, 0.8146718f, 0.002668861f, 0.0331208f, 0.805171f, 0.0001274571f, 0.1177625f, 0.8003029f, -0.0005409196f, 0.1085055f, 0.7952664f };
        recordedRotations = new List<float>() { 0.1020748f, 0.0008644769f, -0.001440137f, 0.9947754f, 0.10363f, 0.005164638f, -0.002793372f, 0.9945986f, 0.1000624f, 0.004693067f, 0.002114925f, 0.9949679f, 0.06629698f, 0.002863487f, 0.01101155f, 0.9977351f, 0.05751949f, 0.004989253f, 0.002013948f, 0.9983299f, 0.06304435f, 0.01679813f, -0.00611924f, 0.9978506f, 0.06356875f, 0.02339169f, -0.002656399f, 0.9976998f, 0.06246784f, 0.02501504f, 5.666441E-05f, 0.9977335f, 0.05645395f, 0.02639552f, -0.0002033036f, 0.9980562f, 0.04823663f, 0.02364216f, 0.008408909f, 0.9985207f, 0.04479155f, 0.02211928f, 0.01644585f, 0.998616f, 0.05537898f, 0.02665729f, 0.02174804f, 0.9978725f, 0.06741409f, 0.03320134f, 0.02341224f, 0.9968976f, 0.07970598f, 0.03756285f, 0.0181999f, 0.9959442f, 0.07365406f, 0.04309032f, 0.02075647f, 0.9961363f, 0.05570193f, 0.03958037f, 0.01827875f, 0.9974952f, 0.0236648f, 0.03375269f, 0.02535653f, 0.9988282f, 0.02894788f, 0.03061028f, 0.02118975f, 0.9988874f, 0.04793568f, 0.0215567f, 0.02279133f, 0.9983577f, 0.05340573f, 0.01695365f, 0.02210196f, 0.9981843f, 0.05025628f, 0.02322276f, 0.01971175f, 0.9982718f, 0.06019484f, 0.02373396f, 0.01754597f, 0.9977502f, 0.09279836f, 0.03010362f, 0.01182526f, 0.9951595f, 0.08311838f, 0.03382372f, 0.0151853f, 0.9958497f, 0.07717659f, 0.03560339f, 0.01394764f, 0.9962839f, 0.07471234f, 0.03181034f, 0.008426382f, 0.9966621f, 0.08191025f, 0.03817632f, 0.01066618f, 0.9958512f, 0.06551714f, 0.04493417f, 0.007881446f, 0.9968081f, 0.03463193f, 0.05462493f, 0.01227986f, 0.9978306f, 0.04440929f, 0.04882521f, 0.01114068f, 0.9977574f, 0.05389662f, 0.03567524f, 0.02394677f, 0.9976217f, 0.05429162f, 0.04143209f, 0.01730453f, 0.9975151f, 0.0490551f, 0.04317573f, 0.02005811f, 0.9976609f, 0.07645023f, 0.04326544f, 0.01067886f, 0.9960771f, 0.07162431f, 0.05171916f, 0.01416268f, 0.9959893f, 0.08374325f, 0.05703283f, 0.01349229f, 0.9947625f, 0.07824544f, 0.05203168f, 0.01163241f, 0.9955075f, 0.07203173f, 0.04610173f, 0.006164461f, 0.9963173f, 0.04890196f, 0.03444592f, 0.01743002f, 0.9980572f, 0.02290839f, 0.02707597f, 0.01939594f, 0.9991826f, 0.03961253f, 0.02455825f, 0.02236759f, 0.9986628f, 0.0512154f, 0.0207369f, 0.02882926f, 0.9980561f, 0.06551236f, 0.02704397f, 0.03124925f, 0.9969956f, 0.05790405f, 0.03204887f, 0.03047203f, 0.9973422f, 0.06154026f, 0.03304005f, 0.0278579f, 0.9971685f, 0.06377377f, 0.03396326f, 0.03503529f, 0.9967708f, 0.06013203f, 0.03271938f, 0.02855509f, 0.9972453f, 0.04503999f, 0.03135334f, 0.02247863f, 0.99824f, 0.05812125f, 0.03105539f, 0.02025491f, 0.9976208f, 0.05213133f, 0.03871639f, 0.01899594f, 0.9977087f, 0.05162992f, 0.03288126f, 0.02229309f, 0.9978759f, 0.02601008f, 0.02537089f, 0.02801345f, 0.998947f, 0.01859246f, 0.02412885f, 0.01921278f, 0.9993513f, 0.03455046f, 0.0217245f, 0.02472031f, 0.998861f, 0.04579097f, 0.02595545f, 0.02385283f, 0.9983289f, 0.05883837f, 0.0382393f, 0.02197043f, 0.9972929f, 0.06355635f, 0.03393105f, 0.01360832f, 0.9973084f, 0.05472347f, 0.0381293f, 0.01244945f, 0.9976956f, 0.05294108f, 0.03888552f, 0.0120317f, 0.9977677f, 0.0640066f, 0.02887273f, 0.005380027f, 0.9975172f, 0.04862148f, 0.03367975f, 0.00563303f, 0.9982334f, 0.03746552f, 0.04124402f, -0.0002641589f, 0.9984464f, 0.01749035f, 0.03637845f, -0.001162689f, 0.9991844f, 0.008813915f, 0.03375078f, -0.005541281f, 0.9993761f, 0.03081804f, 0.02964171f, 0.002207582f, 0.999083f, 0.03731893f, 0.03879252f, 0.005396095f, 0.9985356f, 0.02391591f, 0.04095867f, 0.005832088f, 0.9988576f, 0.05408352f, 0.03622105f, 0.008191092f, 0.9978456f, 0.05951338f, 0.02842852f, 0.01446179f, 0.9977179f, 0.0564823f, 0.03546217f, 0.01671965f, 0.9976336f, 0.0626609f, 0.02800766f, 0.01260164f, 0.9975622f, 0.05634338f, 0.03460747f, 0.01232824f, 0.9977354f, 0.03969222f, 0.03625624f, 0.01094705f, 0.998494f, 0.01523883f, 0.02842651f, 0.01596665f, 0.9993522f, 0.02228864f, 0.02721838f, 0.01749466f, 0.9992279f, 0.03812134f, 0.02634469f, 0.01564094f, 0.9988034f, 0.0422495f, 0.03395764f, 0.01602977f, 0.9984012f, 0.02992259f, 0.03986371f, 0.01551265f, 0.9986365f, 0.04627452f, 0.03193096f, 0.02461741f, 0.9981148f, 0.06001766f, 0.02498133f, 0.02620335f, 0.9975406f, 0.06739371f, 0.01945339f, 0.02813581f, 0.9971399f, 0.0531672f, 0.02469439f, 0.02958265f, 0.9978418f, 0.04944414f, 0.01917297f, 0.02511511f, 0.998277f, 0.02134231f, 0.0176239f, 0.02039936f, 0.9994087f, 0.004262396f, 0.016769f, 0.01420116f, 0.9997495f, 0.01070038f, 0.01506054f, 0.01661353f, 0.9996913f };
    }

    void startPlaying()
    {
        timeElapsed = 0.0f;
        isPlaying = true;
        frameCount = 0;
        frameCountRot = 0;
        newPosition = new Vector3(recordedCoordinates[frameCount], recordedCoordinates[frameCount + 1], recordedCoordinates[frameCount + 2]) + (new Vector3(0, 0, 2)).normalized * 2;
        newRotation = new Quaternion(recordedRotations[frameCountRot], recordedRotations[frameCountRot + 1], recordedRotations[frameCountRot + 2], recordedRotations[frameCountRot + 3]);
        initialY = Camera.main.transform.position.y;

        frameCount += 3;
        frameCountRot += 4;
        remTime = recTime;
        Debug.Log("Playing Started\n");

        maxVal = 0;
        minVal = 100;
        for (int i = 1; i < recordedCoordinates.Count; i += 3)
        {
            if (recordedCoordinates[i] < minVal)
                minVal = recordedCoordinates[i];
            if (recordedCoordinates[i] > maxVal)
                maxVal = recordedCoordinates[i];
        }
        diff_y_squat = maxVal - minVal;
        wentHighEnough_ghost = true;
        wentLowEnough_ghost = false;
        wentHighEnough_you = true;
        wentLowEnough_you = false;

        Debug.Log("minVal: " + minVal.ToString() + " maxVal: " + maxVal.ToString());

        squatCount_ghost = 0;
        squatCount_you = 0;

        UIBottomRight.text = "Ghost's Reps: " + squatCount_ghost.ToString();
        UIBottomRight.color = Color.green;
        UIBottomLeft.text = "Your Reps: " + squatCount_you.ToString();
        UIBottomLeft.color = Color.green;
        UILeft.text = "";
        UIRight.text = "";


    }




    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (isPlaying)
        {
            remTime -= Time.deltaTime;

            


            if (timeElapsed >= timeOut)
            {
                UITopRight.text = "Rem.: " + remTime.ToString("0.0");

                newPosition = new Vector3(recordedCoordinates[frameCount], recordedCoordinates[frameCount + 1], recordedCoordinates[frameCount + 2]) + (new Vector3(0, 0, 2)).normalized * 2;
                newRotation = new Quaternion(recordedRotations[frameCountRot], recordedRotations[frameCountRot + 1], recordedRotations[frameCountRot + 2], recordedRotations[frameCountRot + 3]);

                frameCount += 3;
                frameCountRot += 4;
                if (frameCount >= recordedCoordinates.Count)
                {
                    isPlaying = false;
                    frameCount = 0;
                    frameCountRot = 0;
                    Debug.Log("Playing Ended");

                    UITopRight.text = "Time Up!";
                    UITopRight.color = Color.red;

                    if (squatCount_ghost > squatCount_you)
                    {
                        UIRight.text = "Ghost Won!";
                        UIRight.color = Color.blue;
                        UILeft.text = "You Lost!";
                        UILeft.color = Color.red;
                    }
                    if (squatCount_ghost < squatCount_you)
                    {
                        UIRight.text = "Ghost Lost!";
                        UIRight.color = Color.red;
                        UILeft.text = "You Won!";
                        UILeft.color = Color.blue;
                    }
                    if (squatCount_you == squatCount_ghost)
                    {
                        UIRight.text = "EVEN";
                        UIRight.color = Color.green;
                        UILeft.text = "EVEN";
                        UILeft.color = Color.green;
                    }

                }
                timeElapsed = 0;
            }

            if (wentHighEnough_ghost)
            {

                if ((maxVal - recordedCoordinates[frameCount + 1]) > diff_y_squat*0.7)
                {
                    wentLowEnough_ghost = true;
                    wentHighEnough_ghost = false;
                    Debug.Log("wentHighEnough");
                }
            }

            if (wentLowEnough_ghost)
            {
                if (maxVal - recordedCoordinates[frameCount + 1] < diff_y_squat * 0.3)
                {
                    wentHighEnough_ghost = true;
                    wentLowEnough_ghost = false;
                    squatCount_ghost++;
                    UIBottomRight.text = "Ghost's Reps: " + squatCount_ghost.ToString();
                    Debug.Log("wentLowEnough");
                }

            }


            if (wentHighEnough_you)
            {

                if (initialY - Camera.main.transform.position.y > diff_y_squat*0.7)
                {
                    wentLowEnough_you = true;
                    wentHighEnough_you = false;
                    Debug.Log("wentHighEnoughYOU");
                }
            }

            if (wentLowEnough_you)
            {
                if (initialY - Camera.main.transform.position.y < diff_y_squat * 0.3)
                {
                    wentHighEnough_you = true;
                    wentLowEnough_you = false;
                    squatCount_you++;
                    UIBottomLeft.text = "Your Reps: " + squatCount_you.ToString();
                    Debug.Log("wentLowEnoughYOU");
                }

            }

            if (squatCount_ghost > squatCount_you)
            {
                UIBottomLeft.color = Color.red;
            }
            else
            {
                if (squatCount_ghost == squatCount_you)
                    UIBottomLeft.color = Color.green;
                else
                    UIBottomLeft.color = Color.blue;
            }








            transform.position = Vector3.Lerp(transform.position, newPosition, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 0.2f);
        }
        else
        {
            if (!GotTransform)
            {
                //transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
            }
            if (isRecording)
            {
                remTime -= Time.deltaTime;

                if (timeElapsed >= timeOut)
                {
                    recordedCoordinates.Add(Camera.main.transform.position.x);
                    recordedCoordinates.Add(Camera.main.transform.position.y);
                    recordedCoordinates.Add(Camera.main.transform.position.z);


                    recordedRotations.Add(Camera.main.transform.rotation.x);
                    recordedRotations.Add(Camera.main.transform.rotation.y);
                    recordedRotations.Add(Camera.main.transform.rotation.z);
                    recordedRotations.Add(Camera.main.transform.rotation.w);

                    timeElapsed = 0.0f;

                    UITopRight.text = "Rem.: " + remTime.ToString();

                }

                if (remTime < 0)
                {
                    isRecording = false;
                    hasBeenRecorded = true;
                    Debug.Log("Recording Ended");
                    UITopRight.text = "Time Up!";


                    string joinedStr = "coord {";
                    for (int i = 0; i < recordedCoordinates.Count; i++)
                    {
                        joinedStr += recordedCoordinates[i].ToString() + ",";
                    }
                    joinedStr += "}";


                    string joinedStrRot = "rot {";
                    for (int i = 0; i < recordedRotations.Count; i++)
                    {
                        joinedStrRot += recordedRotations[i].ToString() + ",";
                    }
                    joinedStrRot += "}";

                    Debug.Log(joinedStr);
                    Debug.Log(joinedStrRot);

                }
            }else
            {
                //UILeft.text = "min: " + minVal.ToString() + " max:" + maxVal.ToString() + " y:" + Camera.main.transform.position.y.ToString();
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

        loadData();
        startPlaying();

        //if (!hasBeenRecorded)
        //{
        //    startRecording();
        //}
        //else
        //{
        //    startPlaying();
        //}




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