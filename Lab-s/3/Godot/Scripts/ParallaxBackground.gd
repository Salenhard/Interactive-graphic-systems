extends ParallaxBackground

var speed = -25

func _process(delta):
	var diff = delta * speed
	scroll_offset.x += diff
	scroll_offset.y += diff
