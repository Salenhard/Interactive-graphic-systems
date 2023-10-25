extends ParallaxBackground

var scrolling_speed = 25

func _process(delta):
	var diff = scrolling_speed * delta
	scroll_offset.x -= diff
	scroll_offset.y -= diff
