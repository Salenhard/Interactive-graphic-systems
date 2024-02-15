extends Control
@export var girafe : PackedScene
@export var card : PackedScene
@export var compas : PackedScene
@export var pyramid : PackedScene
var list = [girafe, card, compas, pyramid]
func _on_return_pressed():
	get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")
	
func _on_spawn_object():	
	var object = list[randi_range(0,3)]
	var obect_spawn_location = get_node("SpawnPath/SpawnLocation") #spawn path
	obect_spawn_location.progress_ratio = randf()
	object.initialize(obect_spawn_location)
	add_child(object)
