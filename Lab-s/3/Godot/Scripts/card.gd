class_name Card extends BaseButton

static var sprite_front: Texture2D = load("res://Assets/Images/card_front_texture.png")
static var sprite_back: Texture2D = load("res://Assets/Images/card_back_texture.png")
const DEFAULT_SIZE: Vector2 = Vector2(50, 75)

@export var is_card_up:bool = false
@export var id:int = -1
var sprite:Sprite2D = Sprite2D.new()
var label:Label = Label.new()

signal toogled(id:int, is_card_up:bool)

func _init():
	custom_minimum_size = DEFAULT_SIZE
	size = custom_minimum_size
	sprite.centered = false
	label.add_theme_color_override("font_color", Color.BLACK)
	label.text = str(id)
	# TODO: change magic number 56 to number that calculated by card size
	label.add_theme_font_size_override("font_size", 28)

func _ready():
	add_child(sprite)
	update_cart_texture()
	var cont = CenterContainer.new()
	cont.custom_minimum_size = custom_minimum_size
	cont.size = custom_minimum_size
	cont.add_child(label)
	add_child(cont)

func update_cart_texture():
	sprite.texture = (sprite_front) if is_card_up else (sprite_back)
	var sprite_size = sprite.get_rect().size
	sprite.scale = custom_minimum_size / sprite_size
	label.set_visible(is_card_up)
	size = custom_minimum_size

func toogle():
	is_card_up = !is_card_up
	update_cart_texture()
	
func _pressed():
	toogle()
