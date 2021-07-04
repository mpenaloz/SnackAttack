using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Attack2
{
    class Program
    {
        static void Main(string[] args)
        {
        Restart:
            #region Variables
            //Defined variables
            int[] xPos = new int[50];
            xPos[0] = 35;

            int[] yPos = new int[50];
            yPos[0] = 20;


            int appleXDim = 10;
            int appleYDim = 10;
            int applesEaten = 0;
            int slowApplesEaten = 0;

            int appleXSDim = 0;
            int appleYSDim = 0;

            decimal gameSpeed = 120m;

            bool isGameOn = true;
            bool ifWallHit = false;
            bool isAppleEaten = false;
            bool isSlowAppleEaten = false;

            Random random = new Random();

            Console.CursorVisible = false;

            #endregion 

            //Snake appears on screen
            Console.SetCursorPosition(xPos[0], yPos[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((char)3);


            //Build Boundary
            buildWall();
            

            //Get snake to move
            ConsoleKey command = Console.ReadKey().Key;

            #region Change Directions
            
            
            do
            {
                switch (command)
                {
                    case ConsoleKey.LeftArrow:
                        Console.SetCursorPosition(xPos[0], yPos[0]);
                        Console.Write(" ");
                        xPos[0]--;
                        break;

                    case ConsoleKey.RightArrow:
                        Console.SetCursorPosition(xPos[0], yPos[0]);
                        Console.Write(" ");
                        xPos[0]++;
                        break;

                    case ConsoleKey.UpArrow:
                        Console.SetCursorPosition(xPos[0], yPos[0]);
                        Console.Write(" ");
                        yPos[0]--;
                        break;

                    case ConsoleKey.DownArrow:
                        Console.SetCursorPosition(xPos[0], yPos[0]);
                        Console.Write(" ");
                        yPos[0]++;
                        break;
                }
                #endregion

                #region Game Setup
                
                //Paint the snake
                paintSnake(applesEaten,slowApplesEaten, xPos, yPos, out xPos, out yPos);

              
                //Snake hits boundary
                ifWallHit = DidSnakeHitWall(xPos[0], yPos[0]);

                if (ifWallHit)
                {
                    isGameOn = false;
                    
                    Console.SetCursorPosition(20, 20);
                    Console.WriteLine("The snake hit the wall and has died!");
                    Console.WriteLine();
                    Console.SetCursorPosition(28, 22);
                    Console.WriteLine("Your score is " + applesEaten + "!");

                    //This outputs a message based on final score
                    if (applesEaten <= 1)
                    {
                        Console.SetCursorPosition(18, 24);
                        Console.WriteLine("Maybe you shouldn't play games anymore");
                    }
                    else if (applesEaten <= 5)
                    {
                        Console.SetCursorPosition(18, 24);
                        Console.WriteLine("Just terrible, keep practicing");
                    }
                    else if (applesEaten <= 15)
                    {
                        Console.SetCursorPosition(18, 24);
                        Console.WriteLine("You might have potential?");
                    }
                    else if (applesEaten <= 30)
                    {
                        Console.SetCursorPosition(18, 24);
                        Console.WriteLine("Absolute stud");
                    }


                    Console.WriteLine();
                    //Place-Holder
                    Again:
                    Console.SetCursorPosition(15, 26);
                    Console.WriteLine("Press Enter to play again or press E to exit");


                    //This if statement allows the user to choose if they want to exit or continue playing
                    ConsoleKeyInfo next = Console.ReadKey();
                    if (next.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        goto Restart;
                    }
                    else if (next.Key == ConsoleKey.E)
                    {
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.SetCursorPosition(25, 22);
                        Console.WriteLine("You pressed the wrong key");
                        
                        //This allows the user to re-enter a command
                        goto Again;
                    }

                }


                //Detect when apple was eaten
                isAppleEaten = determineIfAppleWasEaten(xPos[0], yPos[0], appleXDim, appleYDim);

                isSlowAppleEaten = determineIfSlowAppleWasEaten(xPos[1], yPos[1], appleXSDim, appleYSDim);

                //Painting the first apple
                PaintApple(appleXDim, appleYDim);


                // Put the apple on the game board. (random spot)
                if (isAppleEaten)
                {
                    setApplePositionOnScreen(random, out appleXDim, out appleYDim);
                    PaintApple(appleXDim, appleYDim);
                    
                    //how many apples have been eaten
                    applesEaten++;

                    //This makes the snake faster
                    gameSpeed *= .925m;
                }

                else if (isSlowAppleEaten)
                {
                    setApplePositionOnScreen(random, out appleXSDim, out appleYSDim);
                    PaintSlowApple(appleXDim + 1, appleYDim + 4);
                    

                    //how many apples have been eaten
                    applesEaten++;

                    //This makes the snake faster
                    gameSpeed -= .500m;
                }


                if (Console.KeyAvailable) command = Console.ReadKey().Key;

                //Controls game speed
                System.Threading.Thread.Sleep(Convert.ToInt32(gameSpeed));

            } while (isGameOn);
            #endregion

        }
        #region Play the Game
        private static void paintSnake(int applesEaten,int slowApplesEaten, int[] xPosIn, int[] yPosIn, out int[] xPosOut, out int[] yPosOut)
        {
            //Paint the head
            Console.SetCursorPosition(xPosIn[0], yPosIn[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((char)3);


            //Paint the body
            for (int i = 1; i < applesEaten + 1; i++)
            {
                Console.SetCursorPosition(xPosIn[i], yPosIn[i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine((char)3);

            }

            for (int i = 1; i < slowApplesEaten + 1; i++)
            {
                Console.SetCursorPosition(xPosIn[i], yPosIn[i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine((char)3);

            }




            //Erase last part of sake
            Console.SetCursorPosition(xPosIn[applesEaten + 1], yPosIn[applesEaten + 1]);
            
            Console.WriteLine(" ");


            //Record each body part location
            for (int i = applesEaten+1; i > 0; i--)
            {
                xPosIn[i] = xPosIn[i - 1];
                yPosIn[i] = yPosIn[i - 1];
            }


            //Return new array
            xPosOut = xPosIn;
            yPosOut = yPosIn;
        }

        private static void PaintSlowApple(int appleXSDim, int appleYSDim)
        {
            Console.SetCursorPosition(appleXSDim, appleYSDim);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine((char)64);
        }

        private static void PaintApple(int appleXDim, int appleYDim)
        {
            Console.SetCursorPosition(appleXDim, appleYDim);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine((char)64);
        }


        private static bool determineIfAppleWasEaten(int xPosition, int yPosition, int appleXDim, int appleYDim)
        {
            if (xPosition == appleXDim && yPosition == appleYDim)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool determineIfSlowAppleWasEaten(int xPosition, int yPosition, int appleXSDim, int appleYSDim)
        {
            if (xPosition == appleXSDim && yPosition == appleYSDim)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void setApplePositionOnScreen(Random random, out int appleXDim, out int appleYDim)
        {
            appleXDim = random.Next(0 + 3, 70 - 3);
            appleYDim = random.Next(0 + 3, 40 - 3);
        }

        private static void setSlowApplePositionOnScreen(Random random, out int appleXSDim, out int appleYSDim)
        {
            appleXSDim = random.Next(0 + 3, 70 - 3);
            appleYSDim = random.Next(0 + 3, 40 - 3);
        }

        private static bool DidSnakeHitWall(int xPos,int yPos)
        {
            if (xPos == 1 || xPos == 70 || yPos == 1 || yPos == 40) return true; return false;
        }



        private static void buildWall()
        {
            
            for (int i = 1; i < 41; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(1, i);
                Console.WriteLine((char)3);
                Console.SetCursorPosition(70, i);
                Console.WriteLine((char)3);
            }

            for (int i = 1; i <71; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(i, 0);
                Console.WriteLine((char)3);
                Console.SetCursorPosition(i, 40);
                Console.WriteLine((char)3);
            }
        }
        #endregion
    }
}
