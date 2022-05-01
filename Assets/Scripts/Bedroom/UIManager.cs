using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenuObjects;
    [SerializeField] GameObject ThoughtBubbles;
    Vector3 timerStartPos;

    [SerializeField] AudioClip Nightmare;
    [SerializeField] AudioClip Dream;
    [SerializeField] AudioSource Transition;

    [SerializeField] GameObject BedroomImage;
    [SerializeField] float BedroomZoomScale;
    [SerializeField] float TransitionSpeed;


    enum BedroomState{
        Thoughts,
        LoadingGame
    }

    BedroomState bedroomState;
    bool startTimer = false;

    void Start(){
        Cursor.lockState = CursorLockMode.None;
        bedroomState = BedroomState.Thoughts;    
        UtilityManager.Instance.OpenText(ThoughtBubbles, TransitionSpeed);
    }

    void Update(){
        if(bedroomState == BedroomState.LoadingGame) {
            float currentScale = BedroomImage.GetComponent<RectTransform>().localScale.x;
            float newScale = Mathf.Lerp(currentScale, BedroomZoomScale, Time.deltaTime);
            BedroomImage.GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);
            return;
        }

        // if(startTimer){
        //     StartTime -= Time.deltaTime;
        //     UtilityManager.Instance.SetText(StartTimer, "Deep sleep starts in " + StartTime.ToString("F0"));
        //     if(StartTime <= 0) {
        //         bedroomState = BedroomState.LoadingGame;
        //         StartCoroutine(UtilityManager.Instance.ScreenFade(0.9f, true, 0f));
        //          UtilityManager.Instance.CloseText(ThoughtBubbles, TransitionSpeed);
        //          UtilityManager.Instance.MoveText(StartTimer, timerStartPos);
        //         DecideGameChoices();
        //         StartCoroutine(UtilityManager.Instance.ChangeScene(1, 10f));
        //     }
        // }
    }

    void DecideGameChoices(){
        float DreamChance = 50f;
      
        if(PlayerPrefs.HasKey("Win")){
            if(PlayerPrefs.GetInt("Win") == 0){
                Debug.Log("Last Previous Dream -30%");
                DreamChance -= 30f;
            }else{
                 DreamChance += 30f;
                 Debug.Log("Won Previous Dream 30%");
            }
        }

        bool cameraChoice = false;
        bool modifierChoice = false;
        bool objectiveChoice = false;


        foreach(ThoughtBubble bubble in UtilityManager.Instance.CurrentEffects){
            Debug.Log(bubble.effectName + " " + bubble.dreamChance + "%");       
            switch(bubble.cat){
                case ThoughtBubble.ThoughtCategory.Camera:
                    if(bubble.random) break;
                    PlayerPrefs.SetString("CameraChoice", bubble.effectName);
                    DreamChance += bubble.dreamChance;
                    cameraChoice = true;
                    break;
                case ThoughtBubble.ThoughtCategory.Modifier:
                    if(bubble.random) break;
                    PlayerPrefs.SetString("ModifierChoice", bubble.effectName);
                    DreamChance += bubble.dreamChance;
                    modifierChoice = true;
                    break;
                case ThoughtBubble.ThoughtCategory.Objective:
                    if(bubble.random) break;
                    PlayerPrefs.SetString("ObjectiveChoice", bubble.effectName);
                    DreamChance += bubble.dreamChance;
                    objectiveChoice = true;
                    break;
            }
        }

        bool readyToLaunch = false;



        while(readyToLaunch == false){

            if(objectiveChoice && modifierChoice && cameraChoice){
                readyToLaunch = true;
                break;
            }

            int randomEffect = Random.Range(0, UtilityManager.Instance.bubbles.Length);
            ThoughtBubble tempBubble = UtilityManager.Instance.bubbles[randomEffect];

            if(tempBubble.cat == ThoughtBubble.ThoughtCategory.Camera && !cameraChoice && !tempBubble.random){
                DreamChance += tempBubble.dreamChance;
                PlayerPrefs.SetString("CameraChoice", tempBubble.effectName);
                cameraChoice = true;
            }

            if(tempBubble.cat == ThoughtBubble.ThoughtCategory.Modifier && !modifierChoice && !tempBubble.random){
                DreamChance += tempBubble.dreamChance;
                modifierChoice = true;
                PlayerPrefs.SetString("ModifierChoice", tempBubble.effectName);
            }

            if(tempBubble.cat == ThoughtBubble.ThoughtCategory.Objective && !objectiveChoice && !tempBubble.random){
                DreamChance += tempBubble.dreamChance;
                objectiveChoice = true;
                PlayerPrefs.SetString("ObjectiveChoice", tempBubble.effectName);
            }
        }

        bool dream;

        if(Random.Range(0f, 100f) <= DreamChance){
            dream  = true;
            PlayerPrefs.SetInt("Dream", 1);
        } 
        else {
            dream = false;
            PlayerPrefs.SetInt("Dream", 0);
        }

        

        Debug.Log("Dream Chance = " + DreamChance + "%");
        Debug.Log("Player is having a dream: " + dream);
        Debug.Log("Camera Effect = "+ PlayerPrefs.GetString("CameraChoice"));
        Debug.Log("Modifier Effect = "+ PlayerPrefs.GetString("ModifierChoice"));
        Debug.Log("Objective = " + PlayerPrefs.GetString("ObjectiveChoice"));

        if(dream){
            Transition.clip = Dream;
        }else Transition.clip = Nightmare;


        Transition.Play();
        UtilityManager.Instance.CurrentEffects.Clear();
    }

    public void Play(){
        bedroomState = BedroomState.LoadingGame;
        StartCoroutine(UtilityManager.Instance.ScreenFade(0.9f, true, 0f));
        UtilityManager.Instance.CloseText(ThoughtBubbles, TransitionSpeed);
        DecideGameChoices();
        StartCoroutine(UtilityManager.Instance.ChangeScene(1, 10f));         
    }
}