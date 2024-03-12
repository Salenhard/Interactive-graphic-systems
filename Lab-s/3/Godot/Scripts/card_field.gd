class_name CardField extends MarginContainer

@export_category("Cards field")
@export var card_pairs_amount: int = 2
var box: HFlowContainer = HFlowContainer.new();
signal card_toogled(id: int, is_face_up: bool)

func _init():
	box.add_theme_constant_override("h_separation", 8)
	box.add_theme_constant_override("v_separation", 8)
	add_theme_constant_override("margin_bottom", 8)
	add_theme_constant_override("margin_left", 8)
	add_theme_constant_override("margin_right", 8)
	add_theme_constant_override("margin_top", 8)
	add_child(box)

func _ready():
	var cards: Array[Card] = []
	var card_buffer: Card
	for i in range(card_pairs_amount):
		for j in range(2):
			card_buffer = Card.new()
			card_buffer.id = i + 1
			card_buffer.toogled.connect(some_card_toogled)
			cards.append(card_buffer)
	cards.shuffle()
	for card in cards:
		box.add_child(card)

func some_card_toogled(id: int, is_face_up: bool):
	card_toogled.emit(id, is_face_up)

func remove_cards_by_id(id: int):
	var cards_amount: int = box.get_child_count() - 1
	while cards_amount > 0:
		var child: Card = box.get_child(cards_amount)
		if child.id == id:
			box.remove_child(child)
		else:
			cards_amount -= 1

func cards_shufle_and_fill():
	var children: Array[Node] = box.get_children()
	for c in children:
		box.remove_child(c)
		
func flip_down_cards_by_id(id: int):
	var cards_amount: int = box.get_child_count() - 1
	while cards_amount > 0:
		var child: Card = box.get_child(cards_amount)
		if child.id == id:
			child.toogle()
		else:
			cards_amount -= 1
	
