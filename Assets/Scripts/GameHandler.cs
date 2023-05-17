using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;
    private static int score=0;
    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;
    private void Awake()
    {
       
        instance = this;
        InitializeStatic();
    }
    void Start()
    {
        //Debug.Log("Start");

        levelGrid = new LevelGrid(20, 20);
        Debug.Log(snake);
        Debug.Log(levelGrid);
        snake.Setup(levelGrid); 
        levelGrid.Setup(snake);
        Debug.Log("Score" + score);
        

    }
    private void createSnakeHead()
    {
        GameObject snakeHeadGameObject = new GameObject();
        SpriteRenderer snakeSpriteRenderer =snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.i.SnakeHeadSprite;
    }
    private static void InitializeStatic()
    {
        score = 0;
    }
    public static int GetScore()
    {
        
        return score;
    }
    public static void AddScore() 
    {
        score+=100;
        
    }
    public static void GameOver()
    {
        GameOverWindow.ShowStatic();
    }


    private void TextPopUpAtMouse()
    {
    int number = 0;
    FunctionPeriodic.Create(()=> {
       TextPopupMouse("Ding!" + number);
        number++;
    },0.3f);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    


    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    public static Vector3 GetDirToMouse(Vector3 fromPosition)
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        return (mouseWorldPosition - fromPosition).normalized;
    }
    //
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    // Create a Text Popup in the World, no parent
    public static void CreateWorldTextPopup(string text, Vector3 localPosition, float popupTime = 1f)
    {
        UtilsClass.CreateWorldTextPopup(null, text, localPosition, 40, Color.white, localPosition + new Vector3(0, 20), popupTime);
    }
    //Create a Text Popup in the World
    //public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime)
    //{
    //    TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
    //    Transform transform = textMesh.transform;
    //    Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
    //    FunctionUpdater.Create(delegate () {
    //        transform.position += moveAmount * Time.unscaledDeltaTime;
    //        popupTime -= Time.unscaledDeltaTime;
    //        if (popupTime <= 0f)
    //        {
    //            UnityEngine.Object.Destroy(transform.gameObject);
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }, "WorldTextPopup");
    //}
    //Makes Texts Popup Where the Mouse Pointer is
    public static void TextPopupMouse(string text, Vector3? offset = null)
    {
        if (offset == null)
        {
            offset = Vector3.one;
        }
        CreateWorldTextPopup(text, GetMouseWorldPosition() + (Vector3)offset);
    }
}
