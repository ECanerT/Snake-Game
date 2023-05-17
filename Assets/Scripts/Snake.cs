using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private enum State
    { 
        Alive,
        Dead
    }
    private State state;
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private float speedOfSnake;
    
    // LevelGrid setup for the reference
    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
        Debug.Log("Snake this "+levelGrid);
    }
    // Starting Position of the Snake
    #region meta functions
    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        speedOfSnake = 0.2f;
        gridMoveTimerMax = speedOfSnake;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodyPartList = new List<SnakeBodyPart>();
        snakeBodySize = 0;
        state = State.Alive;
        Debug.Log(levelGrid);
    }
    // New Position of the Snake every Update
    private void Update()
    {
        switch (state)
        {
        case State.Alive:
            HandleInput();
            HandleGridMovement();break;
            case State.Dead:
                break;
        }
    }
    #endregion
    // Allows User Input for Snake Movement
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
    } 
    // Changes Movement Direction based on Input
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SnakeMovePosition preSnakeMovePosition = null;
            if (snakeMovePositionList.Count>0)
            {
                preSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(preSnakeMovePosition, gridPosition,gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);
            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right:   gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }
            gridPosition += gridMoveDirectionVector; //gridPosition keeps Snakes Position and gMDV keeps the movement
                                                     //Movements are added to gridposition move snake the current position

            gridPosition= levelGrid.ValidateGridPosition(gridPosition);

            Debug.Log("Snake Head Grid Position: " + gridPosition);
            bool snakeAteFood=levelGrid.SnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                //Snake ate food Body Size grows
                snakeBodySize++;
                CreateSnakeBody();
                Debug.Log("Snake Size:" + snakeBodySize);
                
            }
                if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }
            if (!snakeAteFood )
            {
                
                foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
                {
                    if (snakeBodyPart == null)
                    {
                        Debug.Log("NULL");
                    }
                    Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                    Debug.Log("Snake Body Part:" + snakeBodyPartGridPosition);
                    if (gridPosition == snakeBodyPartGridPosition)
                    {
                        //Game Over!
                        state= State.Dead;
                        GameHandler.GameOver();
                    }
                };
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector)-90);

            UpdateSnakeBodyParts();
        }
        
    }
    private void CreateSnakeBody()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
        
    }
    private void UpdateSnakeBodyParts()
    {
        for(int i = 0; i < snakeBodyPartList.Count; i++)
            {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
            
        }
        levelGrid.SnakeEatFood(gridPosition);
    }
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    // returns the current position of the snakes head in the grid
    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }
    // returns a list of position of the entire snake body
    public List<Vector2Int> GetSnakePositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach(SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        };
        
        return gridPositionList;
    }

    // Single Snake body part created and set on grid
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }
        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 0; break;
                        case Direction.Right: angle = -45; break;
                        case Direction.Left: angle = -135; break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 180; break;
                        case Direction.Right: angle = 45; break;
                        case Direction.Left: angle = 135; break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 90; break;
                        case Direction.Up: angle = 135; break;
                        case Direction.Down: angle = -135; break;
                    } break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = -90; break;
                        case Direction.Up: angle = 45; break;
                        case Direction.Down: angle = -45; break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }


    //Single Move position from the snake
    private class SnakeMovePosition
    {
        private SnakeMovePosition preSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition preSnakeMovePosition, Vector2Int gridPosition,Direction direction)
        {
            this.preSnakeMovePosition = preSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }
        public Direction GetDirection()
        {
            return direction; 
        }
        public Direction GetPreviousDirection()
        {
            if (preSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return preSnakeMovePosition.direction;
            }
            
        }
    }
}
