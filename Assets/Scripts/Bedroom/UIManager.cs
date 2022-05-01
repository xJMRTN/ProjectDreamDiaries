using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenuObjects;
    [SerializeField] GameObject ThoughtBubbles;
    [SerializeField] GameObject StartTimer;
    [SerializeField] float TransitionSpeed;
    [SerializeField] float StartTime;
    [SerializeField] Vector3 timerPos;
    Vector3 timerStartPos;

    [SerializeField] AudioClip Nightmare;
    [SerializeField] AudioClip Dream;
    [SerializeField] AudioSource Transition;

    [SerializeField] GameObject BedroomImage;
    [SerializeField] float BedroomZoomScale;


    enum BedroomState{
        MainMenu,
        Thoughts,
        LoadingGame
    }

    BedroomState bedroomState;
    bool startTimer = false;

    void Start(){
        bedroomState = BedroomState.MainMenu;    
        timerStartPos = StartTimer.transform.position;
    }

    void Update(){
        if(bedroomState == BedroomState.LoadingGame) {
            float currentScale = BedroomImage.GetComponent<RectTransform>().localScale.x;
            float newScale = Mathf.Lerp(currentScale, BedroomZoomScale, Time.deltaTime);
            BedroomImage.GetComponent<RectTransform>().localScale = new Vector3(newScale,newScale,newScale);
            return;
        }

        if(bedroomState == BedroomState.MainMenu){
            WaitForClick();
            return;
        }

        if(startTimer){
            StartTime -= Time.deltaTime;
            UtilityManager.Instance.SetText(StartTimer, "Deep sleep starts in " + StartTime.ToString("F0"));
            if(StartTime <= 0) {
                bedroomState = BedroomState.LoadingGame;
                StartCoroutine(UtilityManager.Instance.ScreenFade(0.9f, true, 0f));
                 UtilityManager.Instance.CloseText(ThoughtBubbles, TransitionSpeed);
                 UtilityManager.Instance.MoveText(StartTimer, timerStartPos);
                DecideGameChoices();
                StartCoroutine(UtilityManager.Instance.ChangeScene(1, 10f));
            }
        }
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


        foreach(ThoughtBubble bubble in UtilityManager.Instance.CurrentEffects){
             Debug.Log(bubble.effectName + " " + bubble.dreamChance + "%");
            DreamChance += bubble.dreamChance;
        }

        bool dream;

        if(Random.Range(0f, 100f) <= DreamChance) dream  = true;
        else dream = false;

        Debug.Log("Dream Chance = " + DreamChance + "%");
        Debug.Log("Player is having a dream: " + dream);
        if(dream){
            Transition.clip = Dream;
        }else Transition.clip = Nightmare;


        Transition.Play();
    }

    void WaitForClick(){
        if(Input.GetMouseButton(0)){
            bedroomState = BedroomState.Thoughts;
            StartCoroutine(SwitchToBedroom());
        }
    }  

    IEnumerator SwitchToBedroom(){
        UtilityManager.Instance.CloseText(MainMenuObjects, TransitionSpeed);
        yield return new WaitForSeconds(2f);
        UtilityManager.Instance.OpenText(ThoughtBubbles, TransitionSpeed);
        yield return new WaitForSeconds(2f);
        UtilityManager.Instance.MoveText(StartTimer, timerPos);
        startTimer = true;
    }


}