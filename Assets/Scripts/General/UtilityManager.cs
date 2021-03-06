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

    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI objectiveText;
    [SerializeField] AudioSource music;
    [SerializeField] AudioClip dreamMusic;
    [SerializeField] AudioClip nightmareMusic;

    public int winConditionstoWin = 1;
    int currentWinConditions = 0;

    private static GameData gd = new GameData();
    public ThoughtBubble[] bubbles;
    public List<UnlockData> unlockDatas = new List<UnlockData>();

    

    [SerializeField] bool inMenu = false;
    [SerializeField] bool gameover = false;

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

        if(!inMenu){
            if(PlayerPrefs.GetInt("Dream") == 0){
                music.clip = dreamMusic;
            }else{
                 music.clip = nightmareMusic;
             }
            music.Play();
        }   
    }

    public void UpdateTimer(float time){

        if(gameover) return;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timer.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        if(time <= 0f){

            string objEffect = PlayerPrefs.GetString("ObjectiveChoice");
            if(objEffect == "Survive"){
                UtilityManager.Instance.WinConditionModify(1);
            }else{
                gameover = true;
                EndGame();
            }
                 
      
        }
    }

    void EndGame(){
        StartCoroutine(UtilityManager.Instance.ScreenFade(1f, true, 0f));
        StartCoroutine(UtilityManager.Instance.ChangeScene(0, 3f));         
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

        if(CurrentEffects.Count == 2){
            CurrentEffects[0].Deselect();
            RemoveFromEffectList(CurrentEffects[0]);
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

    public void ChangeObjectiveText(string _text){
        objectiveText.text = _text;
    }

    public void LoadGame(){
        if(SaveLoad.GetCurrentScene() == 0){
            unlockDatas = GameData.current.unlockDatas;
            int i = 0;
            foreach(ThoughtBubble bubble in bubbles){
                if(i == bubbles.Length - 2) return;
                Debug.Log("Loading " + bubble.name);
                bubble.unlocked = unlockDatas[i].unlocked;
                if(bubble.random) bubble.unlocked = true;
                bubble.Setup();
                i++;
            }
        }
    }


    public void SetupData(){
        unlockDatas.Clear();
         int i = 0;
        foreach(ThoughtBubble bubble in bubbles){
            if(i >= bubbles.Length - 2) break;
            UnlockData newData = new UnlockData(bubble.effectName, bubble.unlocked);
            unlockDatas.Add(newData);
            bubble.Setup();
            i++;
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

    public event Action onGameEnd;



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

    public void GameEnd()
    {
        if (onGameEnd != null)
        {
            onGameEnd();
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