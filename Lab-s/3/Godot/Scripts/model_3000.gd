class_name Model3000 extends RigidBody3D

var dispose_timer: Timer
@export_category("Model3000")
## model path is path from root path
@export var model_path: String = "res://"
@export var dispose_seconds: int = 1


func load_model():
	dispose_timer = Timer.new()
	dispose_timer.wait_time = dispose_seconds
	var scene: PackedScene = Model3000.load_glft_document(model_path)
	if scene != null:
		var model_node: Node = scene.instantiate()
		for c in model_node.get_children():
			if c is MeshInstance3D:
				var m: MeshInstance3D = MeshInstance3D.new()
				m.mesh = c.mesh
				m.skin = c.skin
				m.skeleton = c.skeleton
				m.create_convex_collision()
				add_child(m)


func _ready():
	load_model()


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
