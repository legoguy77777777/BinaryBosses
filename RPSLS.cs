/*Author: Emily Espe
 * Date: 1/25/21
 * Description:  This game is a popular five-weapon expansion, "rock-paper-scissors-Spock-lizard", invented by Sam Kass and Karen Bryla, 
 * which adds "Spock" and "lizard" to the standard three choices of rock-paper-scissors.  
 * In this app, the player plays against the computer's randomly generated selection.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPSLS
{
    class Program
    {
        static bool isPlaying = false;
        static string[] possibleThrows = new string[5] { "rock", "paper", "scissors", "lizard", "spock" };
        static string actionWord;
        static void Main(string[] args)
        { string userThrow;
            string computerThrow;
            bool userWon;
            DisplayRules();
            do
            {
                userThrow = GetUserThrow();
                computerThrow =GetComputerThrow();
                userWon = DetermineOutcome(userThrow, computerThrow, out userThrow, out computerThrow);
               
                DisplayResults(userThrow,computerThrow,userWon);
                AskToPlay();

            } while (isPlaying);
            Console.ReadKey();

        }
        static void DisplayRules()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("****Rock, Paper, Scissors, Lizard, Spock****");
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("Scissors cuts paper, paper covers rock, rock crushes lizard,\nlizard poisons Spock, Spock smashes scissors, scissors decapitates lizard,\nlizard eats paper, paper disproves Spock, Spock vaporizes rock,\nand as it always has, rock crushes scissors.");
            
           
            Console.WriteLine("");
        }
        static string GetUserThrow()
        {
            bool isValid = false;
            string userThrow;
            do
            {
                Console.Write("Enter your throw:  ");
                userThrow = Console.ReadLine();
                userThrow = userThrow.Trim().ToLower();
                foreach (string validThrow in possibleThrows)
                {
                    if (userThrow == validThrow)
                    {
                        isValid = true;
                        break;
                    }
                }

            } while (!isValid);//loop
            return userThrow;
        }
        static string GetComputerThrow()
        {
            int index;
            string computerThrow;
            Random randomNumGen = new Random();
            index = randomNumGen.Next(0, possibleThrows.Length);
            computerThrow = possibleThrows[index];
            return computerThrow;
            
        }
        static bool DetermineOutcome(string userAnswer, string computerAnswer, out string uAnswer, out string cAnswer)
        { /// fix tie
            bool userWon = false;
      
            while (computerAnswer == userAnswer)
            {
                Console.WriteLine("Oh no! You picked {0} and the computer picked {1}! Try again!", userAnswer, computerAnswer);
                userAnswer = GetUserThrow();
                computerAnswer = GetComputerThrow();

            }

            switch (userAnswer)
            {
                case "rock":

                    switch (computerAnswer)
                    {
                        case "paper":
                            actionWord = "covers";
                            userWon = false;
                            break;

                        case "scissors":
                            actionWord = "smashes";
                            userWon = true;
                            break;
                        case "lizard":
                            actionWord = "crushes";
                            userWon = true;
                            break;
                        case "spock":
                            actionWord = "vaporizes";
                            userWon = false;
                            break;
                        default:
                            actionWord = "tie";
                               break;
                            
                    }
               

                    break;

                case "paper":

                    switch (computerAnswer)
                    {
                        case "rock":
                            actionWord = "covers";
                            userWon = true;
                            break;

                        case "scissors":
                            actionWord = "cuts";
                            userWon = false;
                            break;
                        case "lizard":
                            actionWord = "eats";
                            userWon = false;
                            break;
                        case "spock":
                            actionWord = "disproves";
                            userWon = true;
                            break;

                    }
                    

                    break;

                case "scissors":

                    switch (computerAnswer)
                    {
                        case "rock":
                            actionWord = "crushes";
                            userWon = false;
                            break;

                        case "paper":
                            actionWord = "cuts";
                            userWon = true;
                            break;
                        case "lizard":
                            actionWord = "decapitates";
                            userWon = true;
                            break;
                        case "spock":
                            actionWord = "crushes";
                            userWon = false;
                            break;
                    }
                    break;
                    
                case "lizard":
                    switch (computerAnswer)
                    {
                        case "rock":
                            actionWord = "crushes";
                            userWon = false;
                            break;

                        case "paper":
                            actionWord = "eats";
                            userWon = true;
                            break;
                        case "scissors":
                            actionWord = "decapitates";
                            userWon = false;
                            break;
                        case "spock":
                            actionWord = "poisons";
                            userWon = true;
                            break;
                    }


                    break;

                case "spock":
                    switch (computerAnswer)
                    {
                        case "rock":
                            actionWord = "vaporizes";
                            userWon = true;
                            break;

                        case "paper":
                            actionWord = "disproves";
                            userWon = false;
                            break;
                        case "scissors":
                            actionWord = "crushes";
                            userWon = true;
                            break;
                        case "lizard":
                            actionWord = "poisons";
                            userWon = false;
                            break;
                    }

                    break;

                default:
                    Console.WriteLine("Error. Refactor code.");
                    break;
            }
            uAnswer = userAnswer;
            cAnswer = computerAnswer;
            return userWon;
        }
        static bool BreakTie()
        {
            bool tieBroken = true;

            return tieBroken;
        }
        static void DisplayResults(string userAnswer, string computerAnswer, bool userWon)
        {
            if (userWon)
            {
                Console.WriteLine("You played {0} and the computer played {1}.",userAnswer, computerAnswer);
                Console.WriteLine("{0} {1} {2}!",userAnswer.Substring(0,1).ToUpper() + userAnswer.Substring(1), actionWord, computerAnswer);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You won!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("You played {0} and the computer played {1}.", userAnswer, computerAnswer);
                Console.WriteLine("{0} {1} {2}!", computerAnswer.Substring(0,1).ToUpper() + computerAnswer.Substring(1), actionWord, userAnswer);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You lost...");
                Console.ResetColor();
            }

        }
        static void AskToPlay()
        {
            char playAnswer;
            bool isValid;
            do
            {
                Console.Write("Would you like to play again? (y/n)");
                isValid = char.TryParse(Console.ReadLine(), out playAnswer);               
            } while (!isValid || playAnswer != 'y' && playAnswer != 'n');

            if (playAnswer == 'y')
            {
                isPlaying = true;
            }
            else
            {
                isPlaying = false;
            }
        }

    }
}
