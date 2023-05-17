using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGrid 
{
    private Vector2Int foodGridPosition;
    private GameObject foodObject;
    private int width;
    private int height;
    private Snake snake;
    public LevelGrid(int width,int height){
        this.height = height;
        this.width = width;
    }
    //Snake setup for the reference to Snake Script
    public void Setup(Snake snake)
    {
        this.snake = snake;
        Debug.Log("this"+snake);
        //for(int i = 0; i < 75000; i++)
        //{
        //    foodObject = new GameObject("Apple", typeof(SpriteRenderer));
        //    foodObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        //}
        SpawnFood();
    }
    private void SpawnFood()
    {
        do{
            foodGridPosition = new Vector2Int(Random.Range(1,width-1), Random.Range(1, height-1));
        } while( snake.GetSnakePositionList().IndexOf(foodGridPosition)!=-1);
        Debug.Log("Food At :" + foodGridPosition);
        createFoodObject();
    }
    private void createFoodObject()
    {
        foodObject = new GameObject("Apple",typeof(SpriteRenderer));
        foodObject.GetComponent<SpriteRenderer>().sprite=GameAssets.i.foodSprite;
        foodObject.transform.position = new Vector3(foodGridPosition.x,foodGridPosition.y);
    }
    public bool SnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodObject);
            SpawnFood();
            GameHandler.AddScore();
            Debug.Log("snake ate food");
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width-1)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }

}
