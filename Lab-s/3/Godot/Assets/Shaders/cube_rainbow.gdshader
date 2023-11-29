shader_type spatial;

void fragment() {
	vec2 uv = FRAGCOORD.xy / VIEWPORT_SIZE.xy;
	ALBEDO = 0.5 + 0.5 * cos(TIME + uv.xyx + vec3(0,2,4));
}
