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


    enum BedroomState{
        MainMenu,
        Thoughts,
        LoadingGame
    }

    BedroomState bedroomState;
    bool startTimer = false;

    void Start(){
        bedroomState = BedroomState.MainMenu;    
    }

    void Update(){
        if(bedroomState == BedroomState.LoadingGame) return;

        if(bedroomState == BedroomState.MainMenu){
            WaitForClick();
            return;
        }

        if(startTimer){
            StartTime -= Time.deltaTime;
            UtilityManager.Instance.SetText(StartTimer, "Deep sleep starts in " + StartTime.ToString("F0"));
            if(StartTime <= 0) {
                bedroomState = BedroomState.LoadingGame;
                StartCoroutine(UtilityManager.Instance.ScreenFade(0.6f, true, 0f));
                StartCoroutine(UtilityManager.Instance.ChangeScene(1, 3f));
            }
        }
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