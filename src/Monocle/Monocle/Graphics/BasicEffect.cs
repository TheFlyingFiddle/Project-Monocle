using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Graphics
{
    class BasicEffect
    {
        public static readonly string VertexShaderSource = @"
#version 130

precision highp float;

uniform mat4 projection_matrix;

in vec2 in_position;
in vec2 in_coords;
in vec4 in_tint;

out vec2 coords;
out vec4 tint;

void main(void)
{
  coords = in_coords;
  tint = in_tint;
  gl_Position = projection_matrix * vec4(in_position, 1, 1);
}";

        public static readonly string FragmentShaderSource = @"
#version 130

precision highp float;

in vec2 coords;
in vec4 tint;
out vec4 out_frag_color;

uniform sampler2D tex;

void main(void)
{
  out_frag_color = texture(tex, coords) * tint.bgra;
}";
    }
}
