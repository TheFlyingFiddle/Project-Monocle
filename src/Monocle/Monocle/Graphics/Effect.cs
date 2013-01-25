using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Monocle.Utils.Logging;
using OpenTK;

namespace Monocle.Graphics
{
    public sealed class ShaderProgram
    {
        public readonly IGraphicsContext GraphicsContext;

        internal readonly int Handle;
        int vertexID, fragmentID;

        private ShaderProgram(IGraphicsContext context, int vertexID, int fragmentID, int programID)
        {
            this.vertexID = vertexID;
            this.fragmentID = fragmentID;
            this.Handle = programID;
            this.GraphicsContext = context;
        }

        public static ShaderProgram CreateProgram(IGraphicsContext context, string vertexSource, string fragmentSource)
        {
            int vertID = context.CreateShader(ShaderType.VertexShader);
            int fragID = context.CreateShader(ShaderType.FragmentShader);


            context.ShaderSource(vertID, vertexSource);
            context.ShaderSource(fragID, fragmentSource);

            context.CompileShader(vertID);
            context.CompileShader(fragID);

            Debug.LogInfo(context.GetShaderInfoLog(vertID));
            Debug.LogInfo(context.GetShaderInfoLog(fragID));

            int programID = context.CreateProgram();

            context.AttachShader(programID, vertID);
            context.AttachShader(programID, fragID);

            context.LinkProgram(programID);

            Debug.LogInfo(context.GetProgramInfoLog(programID));

            return new ShaderProgram(context, vertID, fragID, programID);   
        }

        public void SetUniform(string name, int value)
        {
            int location = this.GraphicsContext.GetUniformLocation(this.Handle, name);
            GraphicsContext.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = GraphicsContext.GetUniformLocation(this.Handle, name);
            GraphicsContext.Uniform1(location, value);
        }

        public void SetUniform(string name , ref Matrix4 value)
        {
            int location = GraphicsContext.GetUniformLocation(this.Handle, name);
            GraphicsContext.UniformMatrix4(location, ref value, false);
        }

        public void Delete()
        {
            GraphicsContext.DeleteShader(this.vertexID);
            GraphicsContext.DeleteShader(this.fragmentID);
            GraphicsContext.DeleteProgram(this.Handle);
        }

    }
}