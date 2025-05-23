extends Camera2D
@export var MaxPanX:= Vector2(0, 300);
@export var MaxPanY:= Vector2(0, 300);

var mouse_held: bool

var mouse_start:= Vector2.ZERO
var camera_pos:= Vector2.ZERO

func _process(_delta):
	Pan()
	pass
	
func _unhandled_input(event: InputEvent):
	if event.is_action_pressed("CameraPan"):
		mouse_held = true
	if event.is_action_released("CameraPan"):
		mouse_held = false

func Pan():
	if !mouse_held: 
		mouse_start = get_viewport().get_mouse_position()
		camera_pos = position
	if mouse_held:
		var moveVector: Vector2 = get_viewport().get_mouse_position() - mouse_start
		position = camera_pos - moveVector * 1/zoom.x
		
	position.x = clamp(position.x, MaxPanX.x, MaxPanX.y)
	position.y = clamp(position.y, MaxPanY.x, MaxPanY.y)
pass
