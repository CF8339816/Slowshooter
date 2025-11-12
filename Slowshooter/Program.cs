using System;
using System.Linq;

namespace Slowshooter
{

    /*Inital code Simon Forsythe
     * Additinal code  Aiden Buzsik, Chris French
     */


    internal class Program
    {

        static string playField =
@"+-----+   +-----+
|     |   |     |
|     |   |     |
|     |   |     |
+-----+   +-----+";

        static bool isPlaying = true;
//Defining Player 1 pickup random A.B.
        #region Player 1 Pickup Random   

        //player 1 pickup X and Y random
        static Random Player1PickUpsXRnD = new Random();
        static Random Player1PickUpsYRnD = new Random();

        // player 1 pickup positions
        static int pickup1_x_pos;
        static int pickup1_y_pos;

        #endregion
        //Defining Player 2 pickup random A.B.
        #region Player 2 Pickup

        //player 2 pickup X and Y random
        static Random Player2PickUpsXRnD = new Random();
        static Random Player2PickUpsYRnD = new Random();

        // player 2 pickup positions
        static int pickup2_x_pos;
        static int pickup2_y_pos;

        #endregion

        static (int, int) pickup1_position;
        static (int, int) pickup2_position;

        static (int, int) player1_position;
        static (int, int) player2_position;

        static (int, int) player1_positionPROXY; //Initalizing Player 1 Proxy turple for pickup compairason C.F.
        static (int, int) player2_positionPROXY; //Initalizing Player 2 Proxy turple for pickup compairason C.F.



        static int p1_x_input;
        static int p1_y_input;

        static int p2_x_input;
        static int p2_y_input;

        // player 1 pos
        static int p1_x_pos = 3;
        static int p1_y_pos = 2;

        // player 2 pos
        static int p2_x_pos = 13;
        static int p2_y_pos = 2;

        // bounds for player movement
        static (int, int) p1_min_max_x = (1, 5);
        static (int, int) p1_min_max_y = (1, 3);
        static (int, int) p2_min_max_x = (11, 15);
        static (int, int) p2_min_max_y = (1, 3);

        static int player1_score = 0;
        static int player2_score = 0;

        static bool generatePickup1 = true;
        static bool generatePickup2 = true;

        // what turn is it? will be 0 after game is drawn the first time
        static int turn = -1;

        // contains the keys that player 1 and player 2 are allowed to press
        static (char[], char[]) allKeybindings = (new char[] { 'W', 'A', 'S', 'D' }, new char[] { 'J', 'I', 'L', 'K' });
        static ConsoleColor[] playerColors = { ConsoleColor.Red, ConsoleColor.Blue };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while (isPlaying)
            {
                ProcessInput();
                Update();
                Draw();

            }
        }

        static void ProcessInput()
        {
            // if this isn't here, input will block the game before drawing for the first time
            if (turn == -1) return;

            // reset input
            p1_x_input = 0;
            p1_y_input = 0;
            p2_x_input = 0;
            p2_y_input = 0;

            char[] allowedKeysThisTurn; // different keys allowed on p1 vs. p2 turn

            // choose which keybindings to use
            if (turn % 2 == 0) allowedKeysThisTurn = allKeybindings.Item1;
            else allowedKeysThisTurn = allKeybindings.Item2;

            // get the current player's input
            ConsoleKey input = ConsoleKey.NoName;
            while (!allowedKeysThisTurn.Contains(((char)input)))
            {
                input = Console.ReadKey(true).Key;
            }

            // check all input keys
            if (input == ConsoleKey.A) p1_x_input = -1;
            if (input == ConsoleKey.D) p1_x_input = 1;
            if (input == ConsoleKey.W) p1_y_input = -1;
            if (input == ConsoleKey.S) p1_y_input = 1;

            if (input == ConsoleKey.J) p2_x_input = -1;
            if (input == ConsoleKey.L) p2_x_input = 1;
            if (input == ConsoleKey.I) p2_y_input = -1;
            if (input == ConsoleKey.K) p2_y_input = 1;

        }

        static void Update()
        {
            // update players' positions based on input
            p1_x_pos += p1_x_input;
            p1_x_pos = p1_x_pos.Clamp(p1_min_max_x.Item1, p1_min_max_x.Item2);
                    
            p1_y_pos += p1_y_input;
            p1_y_pos = p1_y_pos.Clamp(p1_min_max_y.Item1, p1_min_max_y.Item2);

            p2_x_pos += p2_x_input;
            p2_x_pos = p2_x_pos.Clamp(p2_min_max_x.Item1, p2_min_max_x.Item2);

            p2_y_pos += p2_y_input;
            p2_y_pos = p2_y_pos.Clamp(p2_min_max_y.Item1, p2_min_max_y.Item2);

            player1_position = (p1_y_pos, p1_x_pos);
            player2_position = (p2_y_pos, p2_x_pos);

            //defining ProxyPlayer Position for the compairason to pickup location to reverse the turple for compairason. C.F.
            player1_positionPROXY = (p1_x_pos, p1_y_pos);
            player2_positionPROXY = (p2_x_pos, p2_y_pos);

            if (player1_positionPROXY == pickup1_position) //Redefining the pickup conditions for points  C.F.
            //if (player1_position == pickup1_position)
                {
                player1_score++;
                generatePickup1 = true;
            }

            if (player2_positionPROXY == pickup2_position) //Redefining the pickup conditions for points  C.F.
            //if (player2_position == pickup2_position)
            {
                player2_score++;
                generatePickup2 = true;
            }
            
            
            turn += 1;


        }

        static void Draw()
        {

            // draw the background (playfield)
            Console.SetCursorPosition(0, 0);
            Console.Write(playField);

            // draw player 1
            Console.SetCursorPosition(p1_x_pos, p1_y_pos);
            Console.ForegroundColor = playerColors[0];
            Console.Write("O");

            // draw player 2
            Console.SetCursorPosition(p2_x_pos, p2_y_pos);
            Console.ForegroundColor = playerColors[1];
            Console.Write("O");

            //draw pickups  A.B.
            if(generatePickup1)
            {
               
                generatePickup1 = false;
                pickup1_x_pos = Player1PickUpsXRnD.Next(1, 6);
                pickup1_y_pos = Player1PickUpsYRnD.Next(1, 4);
                pickup1_position = (pickup1_x_pos, pickup1_y_pos);
            }
           
            Console.ForegroundColor = playerColors[0]; //uses predefined player1 color C.F.
            //Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(pickup1_position.Item1, pickup1_position.Item2);
            Console.Write("*"); // player write function uses write not writeline, trying it to see if it makes a difference with the pickup issues ww were having C.F.
            //Console.WriteLine("*");

            if (generatePickup2)
            {
                generatePickup2 = false;
                pickup2_x_pos = Player1PickUpsXRnD.Next(11, 16);
                pickup2_y_pos = Player1PickUpsYRnD.Next(1, 4);
                pickup2_position = (pickup2_x_pos, pickup2_y_pos);
            }


            Console.ForegroundColor = playerColors[1]; //uses predefined player2 color  C.F.
            //Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(pickup2_position.Item1, pickup2_position.Item2);
            Console.Write("*"); // player write function uses write not writeline, trying it to see if it makes a difference with the pickup issues ww were having C.F.
            //Console.WriteLine("*");

            // draw the Turn Indicator  Modified  to better define key usage  A.B.
            Console.SetCursorPosition(0, 5);
            Console.ForegroundColor = playerColors[turn % 2];

            Console.Write($"PLAYER {turn % 2 + 1}'S TURN!\n");


            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" To move please use:");// reworked the message text to better use predefined player2 color  C.F.

            if (turn % 2 == 0)
            {
                Console.ForegroundColor = playerColors[0]; //added this code to better use predefined player1 color  C.F.
                Console.WriteLine("\n WASD");
            }
            else
            {
                Console.ForegroundColor = playerColors[1]; //added this code to better use predefined player2 color  C.F.
                Console.WriteLine("\n IJKL");
            }
            //Coded the player score and pickup location debugs below A.B
            //tweaked the colour s to reflect player colours and added Player location and proxy location debgs C.F.

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("debug block\n");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 Score:");
            Console.ForegroundColor = playerColors[0];
            Console.WriteLine(player1_score);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 Pickup:");
            Console.ForegroundColor = playerColors[0];
            Console.WriteLine(pickup1_position);
            
            
            //Found out after adding player position for debugging that the positions display   y, x for  pickup and  x,y for player so not getting the correct location C.F. 
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 position:");
            Console.ForegroundColor = playerColors[0];
            Console.WriteLine(player1_position);
            //checking the new proxy for position compliance C.F.
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 1 position:");
            Console.ForegroundColor = playerColors[0];
            Console.WriteLine(player1_positionPROXY);



            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 2 Score:");
            Console.ForegroundColor = playerColors[1];
            Console.WriteLine(player2_score);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 2 Pickup:");
            Console.ForegroundColor = playerColors[1];
            Console.WriteLine(pickup2_position);


            //Found out after adding player position for debugging that the positions display   y, x for  pickup and  x,y for player so not getting the correct location C.F. 

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 2 position:");
            Console.ForegroundColor = playerColors[1];
            Console.WriteLine(player2_position);
            //checking the new proxy for position compliance C.F.
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Player 2 position:");
            Console.ForegroundColor = playerColors[1];
            Console.WriteLine(player2_positionPROXY);
            Console.WriteLine();
            Console.WriteLine();




            //Defining winn conditions and message.

            Console.ForegroundColor = ConsoleColor.Green;

            if (player1_score == 5)
            {
                Console.WriteLine($" Player 1 has reached 5 points. Player 1 wins.");
               
            }

            if (player2_score == 5)
            {
                Console.WriteLine($" Player 2 has reached 5 points. Player 2 wins.");
               
            }
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}