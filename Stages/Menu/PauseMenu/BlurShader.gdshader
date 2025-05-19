shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, filter_linear_mipmap;
uniform float amount: hint_range(0.0,5.0);
uniform float darken_amount : hint_range(0.0, 1.0) = 0.5;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 color = textureLod(screen_texture, SCREEN_UV, amount);
	
	vec4 dark_overlay = vec4(0.0, 0.0, 0.0, darken_amount);
    color = mix(color, dark_overlay, darken_amount);
	
	COLOR = color;
	// Called for every pixel the material is visible on.
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
