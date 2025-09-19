using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public GameObject playerBody;
    public BetterController player;
    public ScoreCounter scoreCounter;
    public float enemyRallyDepletionPercent = 10;
    public GameObject enemyHealthbarPrefab, statusEffects;
    public static GlobalVars instance { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreCounter = GameObject.Find("ScoreCounter").GetComponent<ScoreCounter>();
        playerBody = GameObject.Find("playerBody");
        player = playerBody.GetComponent<BetterController>();
       // instance = this;
    }
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //gameData = new GameData(Vector3.zero, 0);

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
