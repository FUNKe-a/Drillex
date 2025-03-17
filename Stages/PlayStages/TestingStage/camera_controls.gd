extends Camera2D
var mouse_held: bool
var mouse_start:= Vector2.ZERO
var camera_pos:= Vector2.ZERO
var ZoomTarget: Vector2
var MaxPanX:= 300
var MaxPanY:= 200
var minZoom:= Vector2(0.5, 0.5)
var maxZoom:= Vector2(2, 2)
# Called when the node enters the scene tree for the first time.
func _ready():
	mouse_held = false
	ZoomTarget = zoom
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	Zoom(delta)
	Pan()
	pass

func Zoom(delta):
	if Input.is_action_just_pressed("Camera_zoom_in"):
		ZoomTarget = zoom * 1.1
	elif  Input.is_action_just_pressed("Camera_zoom_out"):
		ZoomTarget = zoom / 1.1
	
	zoom = zoom.slerp(ZoomTarget, 20 * delta)
	print(zoom)
	zoom = clamp(zoom, minZoom, maxZoom)
pass

func Pan():
	if !mouse_held and Input.is_action_just_pressed("Camera_pan"):
		mouse_start = get_viewport().get_mouse_position()
		camera_pos = position
		mouse_held = true
	if mouse_held and Input.is_action_just_released("Camera_pan"):
		mouse_held = false
		print(position)
	if mouse_held:
		var moveVector = get_viewport().get_mouse_position() - mouse_start
		position = camera_pos - moveVector * 1/zoom.x
		
	position.x = clamp(position.x, -MaxPanX, MaxPanX)
	position.y = clamp(position.y, -MaxPanY, MaxPanY)
pass
