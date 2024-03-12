class_name GameController extends Node
@export var health_points : int = 3
@export_node_path("CardField") var card_field_path : NodePath
var card_buffer : int = -1
var card_field : CardField
func _ready():
	card_field = get_node(card_field_path) as CardField
	card_field.card_toogled.connect(on_card_selected)
	
func on_card_selected(id:int, is_card_front:bool) :
	if card_buffer < 0 :
		card_buffer = id
		return
	elif card_buffer == id :
		card_field.remove_cards_by_id(id)
	else :
		health_points -= 1;
		
		if health_points < 1 :
			game_over()
		
	card_buffer = -1
			
func game_over():
	pass
			
