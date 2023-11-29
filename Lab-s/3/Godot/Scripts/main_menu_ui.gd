extends Control


func _on_exit_pressed():
	get_tree().quit()


func _on_game_rules_pressed():
	get_tree().change_scene_to_file("res://Scenes/game_rules.tscn")


func _on_models_pressed():
	get_tree().change_scene_to_file("res://Scenes/models.tscn")
