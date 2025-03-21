extends Camera2D
@export var MaxPanX:= Vector2(0, 300);
@export var MaxPanY:= Vector2(0, 300);
@export var minZoom:= Vector2(0.5, 0.5)
@export var maxZoom:= Vector2(2, 2)
@export var ZoomSpeed: float

var mouse_held: bool

var mouse_start:= Vector2.ZERO
var camera_pos:= Vector2.ZERO
var ZoomTarget: Vector2

func _ready():
	ZoomTarget = zoom
	pass # Replace with function body.

func _process(delta):
	Zoom(delta)
	Pan()
	pass
	
func _unhandled_input(event: InputEvent):
	if event.is_action_pressed("Camera_pan"):
		mouse_held = true
	if event.is_action_released("Camera_pan"):
		mouse_held = false
	if event.is_action_pressed("Camera_zoom_in"):
		ZoomTarget = zoom * 1.1
	if event.is_action_pressed("Camera_zoom_out"):
		ZoomTarget = zoom / 1.1

func Zoom(delta):
	zoom = zoom.slerp(ZoomTarget, ZoomSpeed * delta)
	zoom = clamp(zoom, minZoom, maxZoom)
pass

func Pan():
	if !mouse_held: 
		mouse_start = get_viewport().get_mouse_position()
		camera_pos = position
	if mouse_held:
		var moveVector = get_viewport().get_mouse_position() - mouse_start
		position = camera_pos - moveVector * 1/zoom.x
		
	position.x = clamp(position.x, MaxPanX.x, MaxPanX.y)
	position.y = clamp(position.y, MaxPanY.x, MaxPanY.y)
pass
