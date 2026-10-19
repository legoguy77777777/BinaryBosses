
import random

# Possible moves
possible_throws = ["rock", "paper", "scissors", "lizard", "spock"]

# Maps each winning move to the moves it defeats and the action word.
winning_moves = {
    "rock": {
        "scissors": "crushes",
        "lizard": "crushes"
    },
    "paper": {
        "rock": "covers",
        "spock": "disproves"
    },
    "scissors": {
        "paper": "cuts",
        "lizard": "decapitates"
    },
    "lizard": {
        "paper": "eats",
        "spock": "poisons"
    },
    "spock": {
        "rock": "vaporizes",
        "scissors": "smashes"
    }
}

user_pick_scores = {
    "rock": 0,
    "paper": 0,
    "scissors": 0,
    "lizard": 0,
    "spock": 0
}


def display_rules():
    print("****Rock, Paper, Scissors, Lizard, Spock****")
    print()
    print(
        "Scissors cuts paper, paper covers rock, rock crushes lizard,\n"
        "lizard poisons Spock, Spock smashes scissors, "
        "scissors decapitates lizard,\n"
        "lizard eats paper, paper disproves Spock, "
        "Spock vaporizes rock,\n"
        "and as it always has, rock crushes scissors."
    )
    print()


def get_user_throw():
    while True:
        user_throw = input("Enter your throw: ").strip().lower()

        if user_throw in possible_throws:
            return user_throw

        print("Invalid throw. Please enter rock, paper, scissors, "
              "lizard, or spock.")


def get_computer_throw(round_results, user_picks):
    # if no info on player, choose randomly
    if (len(round_results) == 0):
        return random.choice(possible_throws)
    else: # Else, choose counter to option user will most likely pick 
        if round_results[-1]: # last result was user win, user will likely stick w/pick
            user_pick_scores[user_picks[-1]] += 1
        else: # last result was not user win, user will likely switch pick
            user_pick_scores[user_picks[-1]] -= 1

        most_likely_user_pick = max(user_pick_scores, key = user_pick_scores.get)

        for key in winning_moves:
            if most_likely_user_pick in winning_moves[key]:
                return key


def determine_outcome(user_answer, computer_answer, user_score, computer_score, round_results, user_picks) :
    # Keep asking for new moves until there is no tie.
    while user_answer == computer_answer:
        print(
            f"Oh no! You picked {user_answer} and the computer "
            f"picked {computer_answer}! Try again!"
        )
        user_answer = get_user_throw()
        user_picks.append(user_answer)
        round_results.append(False)
        computer_answer = get_computer_throw(round_results, user_picks)

    # Check whether the user's move defeats the computer's move.
    if computer_answer in winning_moves[user_answer]:
        user_score += 1
        return True, user_answer, computer_answer, user_score
    else:
        computer_score += 1
        return False, user_answer, computer_answer, computer_score


def display_results(user_answer, computer_answer, user_won):
    if user_won:
        winner = user_answer
        loser = computer_answer
        print(
            f"You played {user_answer} and the computer "
            f"played {computer_answer}."
        )
        print(
            f"{winner.capitalize()} "
            f"{winning_moves[winner][loser]} {loser}!"
        )
        print("You won!")
    else:
        winner = computer_answer
        loser = user_answer
        print(
            f"You played {user_answer} and the computer "
            f"played {computer_answer}."
        )
        print(
            f"{winner.capitalize()} "
            f"{winning_moves[winner][loser]} {loser}!"
        )
        print("You lost...")

def dsiplay_final_result(user_score, computer_score):
    if user_score > computer_score:
        print(f"You beat the computer {user_score} to {computer_score}!")
    elif computer_score > user_score:
        print(f"The computer beat you {computer_score} to {user_score}...")
    else:
        print(f"You tied with the computer!")


def ask_to_play():
    while True:
        play_answer = input(
            "Would you like to play again? (y/n) "
        ).strip().lower()

        if play_answer in ("y", "n"):
            return play_answer == "y"

        print("Please enter 'y' or 'n'.")


def main():
    display_rules()

    is_playing = True

    while is_playing:
        user_score = 0
        computer_score = 0

        round_results = []
        user_picks = []

        while user_score < 3 and computer_score < 3:
            user_throw = get_user_throw()
            computer_throw = get_computer_throw(round_results, user_picks)

            user_won, user_throw, computer_throw, updated_score = determine_outcome(
                user_throw, computer_throw, user_score, computer_score, round_results, user_picks
            )
            round_results.append(user_won)
            user_picks.append(user_throw)

            if user_won:
                user_score = updated_score
            else:
                computer_score = updated_score

            display_results(user_throw, computer_throw, user_won)

        dsiplay_final_result(user_score, computer_score)
        is_playing = ask_to_play()

    print("Thanks for playing!")


if __name__ == "__main__":
    main()
