extends Control


func _on_return_button_pressed():
	get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")


func _on_to_fullscreen_button_toggled(toggled_on):
	if toggled_on:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	else:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
