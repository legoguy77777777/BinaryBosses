extends Control

var current_player_health = 0
var current_enemy_health = 0

# Vars for the opponent model
var past_player_moves = []
var past_comp_moves = []
var past_round_results = []
var action_scores = {
	"attack": 0,
	"parry": 0,
	"magic": 0
	}
var transition_scores = { #for the markov chain
	"attack": {"attack": 0, "parry": 0, "magic": 0},
	"parry": {"attack": 0, "parry": 0, "magic": 0},
	"magic": {"attack": 0, "parry": 0, "magic": 0}
}
	
# control number of games
var game_counter = 0

func _ready():
	set_health($VBoxContainer/EnemyHealthBar, State.enemy_current_health, State.enemy_max_health)
	set_health($Panel/HBoxContainer/PlayerHealthBar, State.player_current_health, State.player_max_health)
	current_enemy_health = State.enemy_current_health
	current_player_health = State.player_current_health


func set_health(progress_bar, health, max_health):
	progress_bar.value = health
	progress_bar.max_value = max_health
	progress_bar.get_node("Label").text = "HP: %d/%d" % [health, max_health]


func _on_attack_pressed() -> void:
	enemy_turn("attack")

func _on_parry_pressed() -> void:
	enemy_turn("parry")

func _on_magic_pressed() -> void:
	enemy_turn("magic")

func enemy_turn(player_move):
	var moves = ["attack", "parry", "magic"]
	var optimal_counters = ["parry", "magic", "attack"]
	var enemy_move = ""
	var predicted_move = ""
	
	if len(past_player_moves) == 0: #if first round, go w/random
		predicted_move = moves[randi_range(0, 2)]
	else:
		var previous_move = past_player_moves[-1]
		
		# select most optimal option
		var max_val = 0
		var candidates = []
		
		for move in moves:
			var score = transition_scores[previous_move][move]
			if score > max_val:
				max_val = score
				candidates = [move]
			elif score == max_val:
				candidates.append(move)
		
		# if we have observed the tranisition, randomly choose between the best candidates
		if len(candidates) > 0:
			predicted_move = candidates[randi_range(0, candidates.size() - 1)]
		else: # if no observations for the transition, use frequency
			max_val = -INF
			for key in action_scores:
				if action_scores[key] > max_val:
					max_val = action_scores[key]
					predicted_move = key
		
	var index = moves.find(predicted_move)
	enemy_move = optimal_counters[index]
	
	# update transition scores and arrays AFTER deciding current move, so no cheating
	if past_player_moves.size() > 0:
		var previous_move = past_player_moves[-1]
		
		# update markov
		transition_scores[previous_move][player_move] += 1
		# update statistical model
		action_scores[previous_move] += 1
	
	past_comp_moves.append(enemy_move)
	past_player_moves.append(player_move) 
	
	# check who won
	move_checker(player_move, enemy_move)
	

func move_checker(player_move, enemy_move):
	
	if player_move == enemy_move:
		past_round_results.append("tie") #record that the round was tied
		pass
	if (player_move == "attack" and enemy_move == "magic") or (player_move == "parry" and enemy_move == "attack") or (player_move == "magic" and enemy_move == "parry"):
		current_enemy_health = max(0, current_enemy_health - State.player_damage)
		set_health($VBoxContainer/EnemyHealthBar, current_enemy_health, State.enemy_max_health)
		past_round_results.append("player") #record that player won
	else:
		current_player_health = max(0, current_player_health - State.enemy_damage)
		set_health($Panel/HBoxContainer/PlayerHealthBar, current_player_health, State.player_max_health)
		past_round_results.append("computer") #record that computer won
	
	if current_player_health == 0 or current_enemy_health == 0:
		game_counter += 1
		
		if game_counter == 3: #end after three games
			get_tree().quit()
		else:
			_ready()
