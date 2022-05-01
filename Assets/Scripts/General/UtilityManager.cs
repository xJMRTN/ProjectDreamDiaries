using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public sealed class UtilityManager : MonoBehaviour
{
    public static UtilityManager instance = new UtilityManager();
    public List<ThoughtBubble> CurrentEffects = new List<ThoughtBubble>();
    public Color SelectedColour;
    public Color NormalColour;
    [SerializeField] float moveTextSpeed;

    public int winConditionstoWin = 1;
    int currentWinConditions = 0;

    private static GameData gd = new GameData();
    [SerializeField] ThoughtBubble[] bubbles;
    public List<UnlockData> unlockDatas = new List<UnlockData>();

    public RawImage blackBox;

    public void Awake(){
        if(instance == null) {
            instance = this;        
        }
        SaveLoad.Load();
    }

    static UtilityManager(){
    }

    private UtilityManager(){

    }

    public static UtilityManager Instance{
        get{return instance;}   
    }

    public void Start(){
        if(GameData.current != null) LoadGame();
        else SetupData();
    }
    
    public void CloseText(GameObject text, float speed){
        LeanTween.scale(text, new Vector3(0.0f, 0.0f, 0.0f), .2f/ speed).setIgnoreTimeScale(true);
    }

    public void OpenText(GameObject text, float speed){
        LeanTween.scale(text, new Vector3(1.0f, 1.0f, 1.0f), .2f / speed).setIgnoreTimeScale(true);
    }

    public void MoveText(GameObject text, Vector3 movePos){
        LeanTween.moveLocal(text, new Vector3(movePos.x, movePos.y, movePos.z), moveTextSpeed ).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutElastic);
    }

    public void AddToEffectList(ThoughtBubble effect){
        foreach(ThoughtBubble bubble in CurrentEffects){
            if(bubble.cat ==  effect.cat) {
                bubble.Deselect();
                RemoveFromEffectList(bubble);
                break;
            }         
        }
        CurrentEffects.Add(effect);     
    }

    public void RemoveFromEffectList(ThoughtBubble effect){
        CurrentEffects.Remove(effect);
    }

    public void SetText(GameObject _text, string value){
        _text.GetComponent<TextMeshProUGUI>().text = value;
    }

     public IEnumerator ScreenFade(float FadeRate, bool fadeIn, float delay){
        float targetAlpha;
        if(fadeIn) targetAlpha = 1.0f;
        else targetAlpha = 0.0f;

        yield return new WaitForSeconds(delay);
          
        Color curColor = blackBox.color;
        while(Mathf.Abs(curColor.a - targetAlpha) > 0.0001f) {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, FadeRate * Time.deltaTime);
            blackBox.color = curColor;
            yield return null;
        }
    }

    public Vector3 ShootRayCastDown(Vector3 startPos){
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(startPos, -Vector3.up, out hit)){
            if(hit.transform.tag == "World"){
                return hit.point;
            }
        }
        return Vector3.zero;
    }

    public IEnumerator ChangeScene(int sceneID, float delay){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneID);
    }

    public void LoadGame(){
        if(SaveLoad.GetCurrentScene() == 0){
            unlockDatas = GameData.current.unlockDatas;
            int i = 0;
            foreach(ThoughtBubble bubble in bubbles){
                bubble.unlocked = unlockDatas[i].unlocked;
                bubble.Setup();
                i++;
            }
        }
    }

    public void SetupData(){
        unlockDatas.Clear();
        foreach(ThoughtBubble bubble in bubbles){
            UnlockData newData = new UnlockData(bubble.effectName, bubble.unlocked);
            unlockDatas.Add(newData);
            bubble.Setup();
        }
        SaveGame();
    }

    public void SaveGame(){
        gd.SaveGame(); 
    }

    ///EVENTS
    ///

    public event Action<int> onDoorAoeTriggerEnter;
    public event Action onWinConditionMet;

    public event Action<int> onWinConditionModified;
    public event Action onPlayerDie;
    public event Action onPlayerTakeDamage;


    public void PlayerDie()
    {
        if (onPlayerDie != null)
        {
            onPlayerDie();
        }
    }
    public void PlayerTakeDamge()
    {
        if (onPlayerTakeDamage != null)
        {
            onPlayerTakeDamage();
        }
    }


    public void DoorAoeTriggerEnter(int ID)
    {
        if (onDoorAoeTriggerEnter != null)
        {
            Debug.Log("utility event");
            onDoorAoeTriggerEnter(ID);
        }
    }

    public void WinConditionModify(int amount)
    {
        currentWinConditions += amount;
        if (currentWinConditions >= winConditionstoWin)
        {
            Debug.Log("Win Condition has been met");
            if (onWinConditionMet != null)
            {
                onWinConditionMet();
            }
        }
        if (onWinConditionModified != null)
        {
            onWinConditionModified(currentWinConditions);
        }
    }
}