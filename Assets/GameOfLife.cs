using UnityEngine;

public class GameOfLife : ProcessingLite.GP21
{
    GameCell[,] cells; //Our game grid matrix
    GameCell[,] copyCells;
    float cellSize = 0.25f; //Size of our cells
    int numberOfColums;
    int numberOfRows;
    int spawnChancePercentage = 15;

    void Start()
    {
        //Lower framerate makes it easier to test and see whats happening.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 4;

        //Calculate our grid depending on size and cellSize
        numberOfColums = (int)Mathf.Floor(Width / cellSize);
        numberOfRows = (int)Mathf.Floor(Height / cellSize);

        //Initiate our matrix array
        cells = new GameCell[numberOfColums, numberOfRows];
        copyCells = new GameCell[numberOfColums, numberOfRows];

        //Create all objects

        //For each row
        for (int y = 0; y < numberOfRows; ++y)
        {
            //for each column in each row
            for (int x = 0; x < numberOfColums; ++x)
            {
                //Create our game cell objects, multiply by cellSize for correct world placement
                cells[x, y] = new GameCell(x * cellSize, y * cellSize, cellSize);
                copyCells[x, y] = new GameCell(x * cellSize, y * cellSize, cellSize);

                //Random check to see if it should be alive
                if (Random.Range(0, 100) < spawnChancePercentage)
                {
                    cells[x, y].alive = true;
                }
            }
        }
    }

    void Update()
    {

        //Clear screen
        Background(0);

        //TODO: Calculate next generation
        Count();

        //TODO: update buffer
        Buffer();

        //Draw all cells.
        for (int y = 0; y < numberOfRows; ++y)
        {
            for (int x = 0; x < numberOfColums; ++x)
            {
                //Draw current cell
                cells[x, y].Draw();
            }
        }
    }

    void Count()
    {
        for (int i = 0; i < numberOfColums; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                int aliveCheck = 0;
                int column = -1;
                int row = -1;

                while (column <= 1)
                {
                    while (row <= 1)
                    {
                        if (i + column < 0)
                        {
                            column++;
                        }
                        else if (i + column > numberOfColums - 1)
                            break;

                        if (j + row < 0)
                        {
                            row++;
                        }
                        else if (j + row > numberOfRows - 1)
                            break;

                        if (cells[i + column, j + row].alive == true)
                        {
                            aliveCheck++;
                        }
                        row++;
                    }

                    column++;
                    row = -1;
                }

                if (cells[i, j].alive == true)
                    aliveCheck--;

                if (aliveCheck < 2 && cells[i, j].alive == true)
                {
                    copyCells[i, j].alive = false;
                }
                else if(aliveCheck > 1 && aliveCheck < 4 && cells[i, j].alive == true)
                {
                    copyCells[i, j].alive = true;
                }
                else if (aliveCheck > 3 && cells[i, j].alive == true)
                {
                    copyCells[i, j].alive = false;
                }
                else if (aliveCheck == 3 && cells[i, j].alive == false)
                {
                    copyCells[i, j].alive = true;
                }
            }
        }
    }

    void Buffer()
    {
        for(int i = 0; i < numberOfColums; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                cells[i, j].alive = copyCells[i, j].alive;
            }
        }
    }

}

//You will probebly need to keep track of more things in this class
public class GameCell : ProcessingLite.GP21
{
    float x, y; //Keep track of our position
    float size; //our size

    //Keep track if we are alive
    public bool alive = false;

    //Constructor
    public GameCell(float x, float y, float size)
    {
        //Our X is equal to incoming X, and so forth
        //adjust our draw position so we are centered
        this.x = x + size / 2;
        this.y = y + size / 2;

        //diameter/radius draw size fix
        this.size = size / 2;
    }

    public void Draw()
    {
        //If we are alive, draw our dot.
        if (alive)
        {
            //draw our dots
            Circle(x, y, size);
        }
    }
}