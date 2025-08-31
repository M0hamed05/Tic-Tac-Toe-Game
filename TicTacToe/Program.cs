using System;
using System.Collections.Generic;

namespace TicTacToe
{

    internal class program
    {
        static void Main(string[] args)
        {
            Game.start.start_game();
        }

    }

    internal class Game
    {
        private class shared
        {
            protected static char[] positions = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            protected static List<int> free_positions = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            protected static readonly Random rnd = new Random();//static so won't repeat numbers

            protected static readonly int[][] wins =
             {
             new[] {1,2,3}, new[] {4,5,6}, new[] {7,8,9},
             new[] {1,4,7}, new[] {2,5,8}, new[] {3,6,9},
             new[] {1,5,9}, new[] {3,5,7}
         };

            protected static int player1_win_times = 0;
            protected static int player2_win_times = 0;
            protected static int pc_win_times = 0;
            protected static int draw_times = 0;

            public static int read_positive_int()
            {
                int num;
                string s;
                while (true)
                {
                    try
                    {
                        s = Console.ReadLine();
                        num = Convert.ToInt32(s);

                        if (num < 0)
                        {
                            Console.Write("Enter a positive number: ");
                            continue;
                        }
                        return num;
                    }
                    catch (OverflowException)
                    {
                        Console.Write("Number is too Big, Try again: ");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("This is not a right number, Try again: ");
                    }
                }

            }

            public static char read_right_char(string error_message = "Invalid, Enter y/n: ")
            {
                while (true)
                {
                    string s = Console.ReadLine().ToLower();
                    if (s == "y" || s == "n") return s[0];
                    Console.WriteLine(error_message);
                }
            }

            protected static int read_position(string message = "Choose Position X: ")
            {
                Console.Write(message);
                int pos = read_positive_int();

                while (pos < 1 || pos > 9)
                {
                    Console.Write("No Position With This number, Try Again: ");
                    pos = read_positive_int();
                }

                while (positions[pos] == 'X' || positions[pos] == 'O')
                {
                    Console.Write("Position Taken, Try Again: ");
                    pos = read_positive_int();
                }
                free_positions.Remove(pos);

                return pos;
            }

            protected static int get_pc_positions()
            {
                int pos = free_positions[rnd.Next(free_positions.Count)];
                free_positions.Remove(pos);
                return pos;
            }

            protected static void clear_positions()
            {
                for (int i = 0; i < 10; i++)
                {
                    positions[i] = (char)('0' + i);
                }

                free_positions.Clear();
                for (int i = 1; i <= 9; i++)
                {
                    free_positions.Add(i);
                }

            }

            protected static char check_winner()
            {
                foreach (var line in wins)
                {
                    if (positions[line[0]] == positions[line[1]] && positions[line[1]] == positions[line[2]])
                    {
                        return positions[line[0]];
                    }
                }

                return '0';
            }

        }

        private class one_player : shared
        {
            internal static void print_result_1player()
            {
                Console.WriteLine($"Player Wins: {player1_win_times}");
                Console.WriteLine($"PC Wins: {pc_win_times}");
                Console.WriteLine($"Draws: {draw_times}\n");

                if (player1_win_times > pc_win_times) Console.WriteLine("Player  Win!");
                else if (player1_win_times < pc_win_times) Console.WriteLine("PC  Win!");
                else Console.WriteLine("Draw!");
            }

            static void print_board_1player(int round)
            {
                Console.WriteLine($"P1: {player1_win_times} Pc: {pc_win_times}");
                Console.WriteLine("===========");
                Console.WriteLine($"  Round {round + 1}");
                Console.WriteLine("===========");
                Console.WriteLine($" {positions[1]} | {positions[2]} | {positions[3]}");
                Console.WriteLine("-----------");
                Console.WriteLine($" {positions[4]} | {positions[5]} | {positions[6]}");
                Console.WriteLine("-----------");
                Console.WriteLine($" {positions[7]} | {positions[8]} | {positions[9]}");
                Console.WriteLine("===========");
            }

            static bool choose_positions_1player(int round, ref int times_of_moves)
            {
                if (round % 2 == 0 || round == 0)
                {
                    print_board_1player(round);
                    positions[read_position()] = 'X';
                    if (check_winner() == 'X' || check_winner() == 'O') return false;
                    times_of_moves++;
                    if (times_of_moves != 5)
                    {
                        positions[get_pc_positions()] = 'O';

                    }
                    else { return false; }

                }
                else
                {
                    positions[get_pc_positions()] = 'O';
                    print_board_1player(round);
                    times_of_moves++;
                    if (times_of_moves != 5)
                    {
                        positions[read_position()] = 'X';
                    }
                    else { return false; }

                }

                return true;
            }

            internal static void start_rounds_1player(int rounds)
            {
                char winner;
                bool pass;
                int x;
                for (int i = 0; i < rounds; i++)
                {
                    x = 0;
                    pass = true;
                    do
                    {
                        pass = choose_positions_1player(i, ref x);

                        winner = check_winner();

                        if (winner == 'X' || winner == 'O') pass = false;

                        Console.Clear();

                    } while (pass);

                    if (winner == 'X') player1_win_times++;
                    else if (winner == 'O') pc_win_times++;
                    else draw_times++;

                    clear_positions();
                }

            }

        }

        private class two_player : shared
        {
            internal static void print_result_2player()
            {
                Console.WriteLine($"Player 1 Wins: {player1_win_times}");
                Console.WriteLine($"Player 2 Wins: {player2_win_times}");
                Console.WriteLine($"Draws: {draw_times}\n");

                if (player1_win_times > player2_win_times) Console.WriteLine("Player 1 Win!");
                else if (player1_win_times < player2_win_times) Console.WriteLine("Player 2  Win!");
                else Console.WriteLine("Draw!");
            }

            static void print_board_2player(int round)
            {
                Console.WriteLine($"P1: {player1_win_times} P2: {player2_win_times}");
                Console.WriteLine("===========");
                Console.WriteLine($"  Round {round + 1}");
                Console.WriteLine("===========");
                Console.WriteLine($" {positions[1]} | {positions[2]} | {positions[3]}");
                Console.WriteLine("-----------");
                Console.WriteLine($" {positions[4]} | {positions[5]} | {positions[6]}");
                Console.WriteLine("-----------");
                Console.WriteLine($" {positions[7]} | {positions[8]} | {positions[9]}");
                Console.WriteLine("===========");
            }

            static bool choose_positions_2player(int round, ref int times_of_moves)
            {

                if (round % 2 == 0 || round == 0)
                {
                    print_board_2player(round);
                    positions[read_position("Player 1 (X): ")] = 'X';

                    if (check_winner() == 'X' || check_winner() == 'O') return false;
                    times_of_moves++;

                    Console.Clear();
                    print_board_2player(round);

                    if (times_of_moves != 5)
                    {
                        positions[read_position("Player 2 (O): ")] = 'O';
                    }
                    else { return false; }

                }
                else
                {
                    print_board_2player(round);
                    positions[read_position("Player 2 (O): ")] = 'O';

                    if (check_winner() == 'X' || check_winner() == 'O') return false;

                    Console.Clear();
                    print_board_2player(round);

                    times_of_moves++;
                    if (times_of_moves != 5)
                    {
                        positions[read_position("Player 1 (X): ")] = 'X';
                    }
                    else { return false; }

                }

                return true;
            }

            internal static void start_rounds_2player(int rounds)
            {
                char winner;
                bool pass;
                int x;
                for (int i = 0; i < rounds; i++)
                {
                    x = 0;
                    pass = true;
                    do
                    {

                        pass = choose_positions_2player(i, ref x);

                        winner = check_winner();

                        if (winner == 'X' || winner == 'O') pass = false;

                        Console.Clear();

                    } while (pass);

                    if (winner == 'X') player1_win_times++;
                    else if (winner == 'O') player2_win_times++;
                    else draw_times++;

                    clear_positions();
                }
            }
        }

        internal class start
        {
            public static void start_game()
            {
                char again = 'y';
                do
                {
                    Console.WriteLine("Welcome to Tic Tac Toe game");
                    Console.Write("How many rounds you want to play? ");
                    int rounds = shared.read_positive_int();

                    Console.Write("1 or 2 Players? ");
                    int players;
                    do
                    {
                        players = shared.read_positive_int();
                        Console.Write("Enter 1 or 2: ");

                    } while (players != 1 && players != 2);

                    Console.Clear();

                    start_rounds(rounds, players);

                    if (players == 1) one_player.print_result_1player();
                    else two_player.print_result_2player();

                    Console.Write("Do you want to play again? (y/n)? ");
                    again = shared.read_right_char();
                    Console.Clear();
                } while (again == 'y');
                Console.WriteLine("GoodBye");
                System.Threading.Thread.Sleep(1000);
            }

            protected static void start_rounds(int rounds, int player)
            {
                if (player == 1) one_player.start_rounds_1player(rounds);
                else two_player.start_rounds_2player(rounds);
            }

        }
    }

}
