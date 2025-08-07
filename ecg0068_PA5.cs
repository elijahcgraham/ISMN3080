// ecg0068_PA5 - Elijah Graham - Battleship Game

using System;
using System.Runtime.ExceptionServices;

class GameBoards
{
    static string statsFile = "stats.txt";      // declaring the stats file
    static int totalGames = 0;  // declaring total games variable
    static int userWins = 0;    // declaring total wins variable
    static int userLosses = 0;  // declaring total losses variable


    static void Main (string[] args)
    {
        File.WriteAllText(statsFile, "");              // clears out the stats file upon starting the program
        do
        {   
            displayIntro();             // introduce the game and instructions to the user before beginning the game
            
            bool userStatus = playGame();                  // runs the entire playGame module

            totalGames++;                                   // adds total game count
            if (userStatus) userWins++;                     // true if user won
            else userLosses++;

            statsFileUpdate(statsFile, totalGames, userWins, userLosses);

            Console.WriteLine("Would you like to play again? (Y/N):");          // prompts user if they'd like to play again or not
            string replay = Console.ReadLine().Trim().ToUpper();                // converts the user's response to uppercase to easily assess for Y or N 
        
            if (replay != "Y")          // exits the game if the user does NOT respond with Y (yes)
            {
                Console.WriteLine("Hope you had fun playing. Have a great day!");
                break;
            }
        }
        while (true);
    }



    static void statsFileUpdate(string filePath, int totalGames, int userWins, int userLosses)
    {
        File.WriteAllText(filePath, $"Total Games: {totalGames}\nWins: {userWins}\nLosses: {userLosses}");
    }



    static bool playGame()
    { 
        char[,] userBoard = new char[10, 10];   // Sets a 2D array for the userboard
        for (int ur = 0; ur < 10; ur++)         // Declares 'ur' (user row) 
        {
            for (int uc = 0; uc < 10; uc++)     // Declares 'uc' (user column)
            {
                userBoard[ur, uc] = '~';        // Sets the entire array as '~' for the water
            }
        }

        char[,] compBoard = new char[10, 10];   // Sets a 2D array for CPU
        for (int cr = 0; cr < 10; cr++)         // Declares 'cr' (cpu row)
        {
            for (int cc = 0; cc < 10; cc++)     // Declares 'cc' (cpu column)
            {
                compBoard[cr, cc] = '~';        // Sets the entire array as '~' for the water
            }            
        }
        // Place ships on userBoard
        PlaceShip(userBoard, 5, 'S'); // Carrier
        PlaceShip(userBoard, 4, 'S'); // Battleship
        PlaceShip(userBoard, 3, 'S'); // Cruiser
        PlaceShip(userBoard, 3, 'S'); // Submarine
        PlaceShip(userBoard, 2, 'S'); // Destroyer

        // Place ships on compBoard
        PlaceShip(compBoard, 5, 'S'); // Carrier
        PlaceShip(compBoard, 4, 'S'); // Battleship
        PlaceShip(compBoard, 3, 'S'); // Cruiser
        PlaceShip(compBoard, 3, 'S'); // Submarine
        PlaceShip(compBoard, 2, 'S'); // Destroyer

        printUserBoard(userBoard);                                  // runs and displays the user's board for initial setup
        printCompBoard(compBoard);                                  // runs and displays the cpu's board for initial setup
        Console.WriteLine("\nPress any key to continue...");        // prompts user to press any key to continue to regulate flow
        Console.ReadKey();

        int turn = 1;                                  // sets a 'turn' variable so we can count each turn of the game
        while (true)
        {
            Console.WriteLine($"\n====== TURN {turn} ======");
            userShot(compBoard);                   // user takes turn
            printCompBoard(compBoard);             // return the updated board after the hit
            PauseQuitOption();                     // pauses game to regulate flow & provide option to quit game

            if (!HasRemainingShips(compBoard))        // if cpu's board has no ships remaining, announce the user wins
            {
                Console.WriteLine("Congrats! You win - all CPU ships have been sunk!");    
                return true;                    // indicates user won
            }

            cpuShot(userBoard);                  // cpu takes turn
            printUserBoard(userBoard);           // return the updated board after the hit
            PauseQuitOption();                   // pauses the game to regulate flow & provide option to quit game

            if (!HasRemainingShips(userBoard))       // if user's board has no ships remaining, announce the cpu wins
            {
                Console.WriteLine("Sorry! You lost - all your ships have been sunk!");
                return false;                    // indicates user lost
            }

        turn++;
        }

        Console.WriteLine("\nGame Over.");
    }


    static void printUserBoard(char[,] userBoard)
    {
        Console.WriteLine("\nUSER BOARD:\n");
        Console.Write("   "); // Top-left corner of the board
        for (int uc = 0; uc < userBoard.GetLength(1); uc++)  // repeat the loop until the columns are fully labeled
        {
            Console.Write(uc + " "); // writing the column header and spacing them out
        }
            Console.WriteLine();

        for (int ur = 0; ur < userBoard.GetLength(0); ur++)
        {
            char rowLabel = (char)('A' + ur); // labeling rows 
            Console.Write(rowLabel + "  "); // row numbering + spacing between each label

        for (int uc = 0; uc < userBoard.GetLength(1); uc++)
        {
            Console.Write(userBoard[ur, uc] + " "); // writes out the 2D arrays as the actual board
        }
            Console.WriteLine();
        }
    }



    static void printCompBoard(char[,] compBoard)
    {
        Console.WriteLine("\nCPU BOARD:\n");
        Console.Write("   "); // Top-left corner of the board
        for (int cc = 0; cc < compBoard.GetLength(1); cc++)  // repeat the loop until the columns are fully labeled
        {
            Console.Write(cc + " ");                        // creates spacing between columns 
        }
        Console.WriteLine();

        for (int cr = 0; cr < compBoard.GetLength(0); cr++)
        {
            char rowLabel = (char)('A' + cr); // labeling rows 
            Console.Write(rowLabel + "  "); // row numbering + spacing between each label

            for (int cc = 0; cc < compBoard.GetLength(1); cc++)      // hide ships: only show water (~), hit (H), or miss (M)
            {
                if (compBoard[cr, cc] == 'S')
                {
                    Console.Write("~ ");                            // writes over a ship on cpu board's display so user cannot see a ship is there
                }
                else
                {
                    Console.Write(compBoard[cr, cc] + " ");        // creates spacing between array pieces
                }
            }
            Console.WriteLine();
        }
    }


    static void PlaceShip(char[,] board, int shipSize, char shipSymbol)
    {
        Random rand = new Random();             // random selection on array of where to place ship
        bool placed = false;

        while (!placed)                         // conditional to ensure ships are placed so long as they have not been already
        {
            int row = rand.Next(0, 10);         // places ship randomly in the row
            int col = rand.Next(0, 10);         // places ship randomly in the column
            bool horizontal = rand.Next(2) == 0;

            bool canPlace = true;               // checks that the ships fit in the board and do not overlap with each other

            if (horizontal)                         // validation check assuming the ship is horizontal
            {
                if (col + shipSize > 10)            // if the ship overlaps the last column, then we try placing it again
                    continue;

                for (int i = 0; i < shipSize; i++)  // loop to try placing until the ship fits inside of the board
                {
                    if (board[row, col + i] != '~') // checks if there are currently any ships in place where we plan to sit one; if so we cannot place it and we try again
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace)                       // place the ship symbol, denoted 'S', where we have decided to randomly place our ships when above conditions are met
                {
                    for (int i = 0; i < shipSize; i++)
                    board[row, col + i] = shipSymbol;   // if the ship does not overlap columns NOR rows, then we can place 'S' where we have ships
                    placed = true;
                }
            }
            else                                    // validation check assuming the ship is vertical
            {
                if (row + shipSize > 10)            // if the ship overlaps the last row, then we try placing it again
                continue;

                for (int i = 0; i < shipSize; i++)  // loop for placing the ship until it fits inside the board
                {
                    if (board[row + i, col] != '~') // checks if there are any ships where we are currently trying to sit one; if so, we do not place it and then the program tries again
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace)                       // place the ship symbol, 'S', where we have decided to randomly place the ships when conditions are met
                {
                    for (int i = 0; i < shipSize; i++)
                        board[row + i, col] = shipSymbol;   // if the ship does not overlap columns NOR rows, then we place an 'S' where we have ships
                    placed = true;
                }
            }
        }
    }
    
    
    
    static void userShot(char[,] compBoard)
    {
        while (true)
        {
            Console.WriteLine("\nIt's your turn to fire!");
            Console.Write("Enter your shot (e.g., A0 or J9), or type 'Q' to quit: ");  // gets input from user
            string input = Console.ReadLine().ToUpper().Trim();     // converts any letters to uppercase

            if (input == "Q")                                       // checks if the user has entered 'q' or 'Q' to indicate to stop the program
            {
                Console.WriteLine("\nGame terminated.\n");              // stops the game
                Environment.Exit(0);                                // exits program 
            }
            if (input.Length < 2 || input.Length > 3)               // confirms the input is no less than 2 and no more than 3 characters
            {
                Console.WriteLine("Invalid input. Try again.");
                continue;
            }

            char rowChar = input[0];                                // ensures the first character in the input is a letter
            if (rowChar < 'A' || rowChar > 'J')                     // checks that the letter input is A-J (rows)
            {
                Console.WriteLine("Invalid row. Use letters A-J.");
                continue;
            }

            if (!int.TryParse(input.Substring(1), out int col) || col < 0 || col > 9)       // checks that the second character can become an integer and is between 0-9 (columns)
            {
                Console.WriteLine("Invalid column. Use numbers 0-9.");
                continue;
            }

            int row = rowChar - 'A';                                                        // subtracted input row from A (the first row)

            if (compBoard[row, col] == 'H' || compBoard[row, col] == 'M')                   // checks if the user has already shot at this location before
            {
                Console.WriteLine("You've already shot here. Try again.");
                continue;
            }

            if (compBoard[row, col] == 'S')                                                 // if ship is present (and not previously hit), alert user of a hit
            {
                Console.WriteLine("\nHIT!\n");
                compBoard[row, col] = 'H';                                                   // updates the board to show the hit tile
            }
            else
            {
                Console.WriteLine("\nMiss!\n");                                              // else (ship is not present) tell user they missed
                compBoard[row, col] = 'M';                                                   // updates the board to show the miss tile
            }
            break;
        }
    }



    static void cpuShot(char[,] userBoard)
    {
        Random rand = new Random();                                             // rolls a random array tile to attack
        int row, col;
        bool validShot = false;

        while (!validShot)                                                      
        {
            row = rand.Next(0, 10);
            col = rand.Next(0, 10);

            char current = userBoard[row, col];

            if (current == 'H' || current == 'M') continue;                     // CPU has shot here previously; will not shoot again

            Console.WriteLine($"\nCPU fires at {(char)('A' + row)}{col}!");

            if (current == 'S')                                                 // if a ship is present, inform user their ship was hit
            {
                Console.WriteLine("\nCPU HIT your ship!\n");
                userBoard[row, col] = 'H';                                      // change the board to show the hit tile
            }
            else
            {
                Console.WriteLine("\nCPU missed!\n");                               // else (no ship present) inform user the CPU missed
                userBoard[row, col] = 'M';                                          // change the board to show the miss tile
            }
            validShot = true;
        }
    }



    static bool HasRemainingShips(char[,] board)                                // checks if the user/cpu has any ships remaining on their board
    {
        foreach (char cell in board)
        {
            if (cell == 'S')                                                    // assuming there are ships remaining, we return true and escape
            return true;
        }
        return false;                                                           // return false if no ships remain
    }



    static void PauseQuitOption()
    {
        Console.WriteLine("\nPress any key to continue, or enter 'Q' to quit..."); // prompts user to press a key to continue or to quit
        var key = Console.ReadKey(true); // true = don't show keypress (just do action)

        if (key.Key == ConsoleKey.Q)     // do the following ONLY if the user's input is 'Q'
        {
            Console.WriteLine("\nGame terminated.\n");
            Environment.Exit(0);        // stops the program
        }
    }
    


    static void displayIntro()
    {
        Console.WriteLine("Welcome to Battleship!");        // greeting and instructions for the user!!!
        Console.WriteLine("Your goal is sink all of your opponent's ships before they can sink yours.");
        Console.WriteLine("Your ships, as well as your opponent's ships, are randomly assigned to the board.");
        Console.WriteLine("You have 5 ships, each a different size.");
        Console.WriteLine("Your Carrier ship takes up 5 spaces.");
        Console.WriteLine("Your Battleship takes up 4 spaces.");
        Console.WriteLine("Your Cruiser takes up 3 spaces.");
        Console.WriteLine("Your Submarine takes up 3 spaces.");
        Console.WriteLine("Your Destroyer takes up 2 spaces.");
        Console.WriteLine("You and your opponent will take turns firing at each other's ships. Neither of you know where the other's ships are located.");           
        Console.WriteLine("The game continues until one of you sink all of the other's ships.");
        Console.WriteLine("Now let the game begin!");
        Console.WriteLine("\nPress any key to take a glance at the game boards...");  // prompts user to press any key to begin the game
        Console.ReadKey();
    }
}