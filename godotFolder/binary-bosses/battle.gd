extends Control

var current_player_health = 0
var current_enemy_health = 0


func _ready():
	set_health($VBoxContainer/EnemyHealthBar, State.enemy_current_health, State.enemy_max_health)
	current_enemy_health = State.enemy_current_health


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
	var enemy_move = moves[randi_range(0, 2)]
	move_checker(player_move, enemy_move)
	

func move_checker(player_move, enemy_move):
	
	if player_move == enemy_move:
		pass
	if (player_move == "attack" and enemy_move == "magic") or (player_move == "parry" and enemy_move == "attack") or (player_move == "magic" and enemy_move == "parry"):
		current_enemy_health = max(0, current_enemy_health - State.player_damage)
		set_health($VBoxContainer/EnemyHealthBar, current_enemy_health, State.enemy_max_health)
	else:
		pass
	
	pass
