/*
Current features:
    - Basic interaction
    - Basic statistical inference for computer

To Do:
    - Make statistical inference better (right now, it's very simple)
    - Generalize the determine_winner function
    - Make everything prettier
    - Output final prediction numbers (Optional, might be a cool feature for the user)
*/

#include <iostream>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

// define determine_winner function
// NOT GENERALIZED
int determine_winner(string c_move, string u_move){
    int winner;

    // TIE
    if (c_move == u_move){
        winner = 2;
        return winner;
    }

    // Computer chose rock outcomes
    if (c_move == "rock"){
        if (u_move == "paper"){
            // user chose paper, beats rock and wins
            winner = 1;
            return winner;
        } else {
            // user chose scissors, rock beats and user loses
            winner = 0;
            return winner;
        }
    }

    // Computer chose paper outcomes
    if (c_move == "paper"){
        if (u_move == "scissors"){
            winner = 1;
            return winner;
        } else {
            winner = 0;
            return winner;
        }
    }

    // Computer chose scissors outcomes
    if (c_move == "scissors"){
        if (u_move == "rock"){
            winner = 1;
            return winner;
        } else {
            winner = 0;
            return winner;
        }
    }

    return 3; //Should never reach this.
}


int main(){
    /*
        Plan: best of 5. r, p, or s
        scenarios:
        r + r = tie
        r + p = p wins
        r + s = r wins
        p + p = tie
        p + s = s wins
        s + s = tie

        Model remembers what user picks and guesses what they will pick next

        Start with equal chances for each option, model picks randomly.
        Then, increase frequency score for the option the user picked. 
            Model picks the option that counters the one the user is likely to pick.
    */
    
    int total_options = 3;
    vector<int> frequency;
    for (int i = 0; i < total_options; i++){
        frequency.push_back(1);
    }

    vector<string> move_names = {"rock", "paper", "scissors"};

    string user_move;
    int num_user_move;
    string computer_move;

    int counter = 0;
    int user_score = 0;
    int computer_score = 0;

    // Set up user interactions with game:
    cout << "Welcome to Rock, Paper, Scissors!" << endl;

    while (user_score < 3 && computer_score < 3){
        cout << "Game " << counter << ":" << endl;

        // Get user action
        cout << "What is your move? (1 for rock, 2 for paper, or 3 for scissors)" << endl;
        cin >> num_user_move;
        num_user_move--;
        user_move = move_names[num_user_move];

        // Get computer action
        // if all options have equal chances, computer picks randomly
        if (adjacent_find(frequency.begin(), frequency.end(), not_equal_to<>() ) == frequency.end()){
            int rand_choice = rand() % (total_options + 1);
            computer_move = move_names[rand_choice];
        // else, pick move with highest win chance
        }
        else {
            //get element w/highest frequency
            auto max_it = max_element(frequency.begin(), frequency.end());
            int max_index = distance(frequency.begin(), max_it);
            
            // determine optimal computer move
            string likely_move = move_names[max_index];
            if (likely_move == "rock"){
                computer_move = "paper";
            } else if (likely_move == "paper"){
                computer_move = "scissors";
            } else if (likely_move == "scissors"){
                computer_move = "rock";
            }
        }

        cout << "computer chose: " << computer_move << endl;

        int result = determine_winner(computer_move, user_move);

        if (result == 1){
            cout << "You win!" << endl;
            user_score++;
        } else if (result == 2){
            cout << "It's a tie!" << endl;
        } else if (result == 0){
            cout << "You lose!" << endl;
            computer_score++;
        }

        cout << "The score is now User with " << user_score << " points, and Computer with " << computer_score << " points" << endl;
    
        // update frequency information
        if (result == 1){
            // user won, so it's more likely they will stick with their choice
            frequency[num_user_move] = frequency[num_user_move]++;
        } else {
            // user lost or tied, so it's more likely they will change their choice
            frequency[num_user_move] = frequency[num_user_move]--;
        }

        counter++;
    }

    return 1;
}

