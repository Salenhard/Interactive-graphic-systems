class_name Model3000 extends RigidBody3D

var dispose_timer: Timer = Timer.new()
@export_category("Model3000")
## model path is path from project root path
@export_file("*.gltf", "*.glb") var model_path: String = "res://"
@export var dispose_seconds: int = 1
@export var models_scale: Vector3 = Vector3.ONE


func _init():
	dispose_timer.timeout.connect(on_timeout)


func load_model():
	var scene: PackedScene = Model3000.load_glft_document(model_path)
	if scene != null:
		var model_node: Node = scene.instantiate()
		for c in model_node.get_children():
			if c is MeshInstance3D:
				var m: MeshInstance3D = MeshInstance3D.new()
				m.mesh = c.mesh
				m.skin = c.skin
				m.skeleton = c.skeleton
				m.scale = models_scale
				m.create_convex_collision()
				for mc in m.get_children():
					if mc is StaticBody3D:
						for mcc in mc.get_children():
							if mcc is CollisionShape3D:
								var shape_buffer: CollisionShape3D = mcc.duplicate(true)
								shape_buffer.scale = models_scale
								add_child(shape_buffer)
						m.remove_child(mc)
				add_child(m)


func _ready():
	load_model()
	dispose_timer.wait_time = dispose_seconds
	add_child(dispose_timer)
	dispose_timer.start()


static func load_glft_document(glft_document_path: String) -> PackedScene:
	glft_document_path = ProjectSettings.globalize_path(glft_document_path)
	var is_null: bool = glft_document_path == null
	var exists: bool = FileAccess.file_exists(glft_document_path)
	if is_null or not exists:
		printerr("Couldn't find file (full file path: %s)." % glft_document_path)
		return null

	var gltf_document_load = GLTFDocument.new()
	var gltf_state_load = GLTFState.new()
	var error = gltf_document_load.append_from_file(glft_document_path, gltf_state_load)
	if error == OK:
		return load(glft_document_path)

	printerr("Couldn't load glTF scene (error code: %s)." % error_string(error))
	return null


func on_timeout():
	queue_free()
