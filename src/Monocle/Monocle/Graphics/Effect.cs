using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Monocle.Utils.Logging;
using OpenTK;

namespace Monocle.Graphics
{
    public sealed class Effect
    {
        int vertexID, fragmentID;
        public int programID;

        private Effect(int vertexID, int fragmentID, int programID)
        {
            this.vertexID = vertexID;
            this.fragmentID = fragmentID;
            this.programID = programID;
        }

        public static Effect CreateEffect(string vertexSource, string fragmentSource)
        {
            int vertID = GL.CreateShader(ShaderType.VertexShader);
            int fragID = GL.CreateShader(ShaderType.FragmentShader);


            GL.ShaderSource(vertID, vertexSource);
            GL.ShaderSource(fragID, fragmentSource);

            GL.CompileShader(vertID);
            GL.CompileShader(fragID);

            Debug.LogInfo(GL.GetShaderInfoLog(vertID));
            Debug.LogInfo(GL.GetShaderInfoLog(fragID));

            int programID = GL.CreateProgram();

            GL.AttachShader(programID, vertID);
            GL.AttachShader(programID, fragID);

            GL.LinkProgram(programID);

            Debug.LogInfo(GL.GetProgramInfoLog(programID));

            ErrorCode code = GL.GetError();
            if (code != ErrorCode.NoError)
            {
                GL.DeleteShader(vertID);
                GL.DeleteShader(fragID);
                GL.DeleteProgram(programID);

                throw new OpenTK.GraphicsException("Effect could not be compleated correctly!");
            }

            return new Effect(vertID, fragID, programID);   
        }

        public void Use()
        {
            GL.UseProgram(this.programID);
        }

        public void SetUniform(string name, int value)
        {
            int location = GL.GetUniformLocation(this.programID, name);
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, float value)
        {
            int location = GL.GetUniformLocation(this.programID, name);
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name , ref Matrix4 value)
        {
            int location = GL.GetUniformLocation(this.programID, name);
            ErrorCode error = GL.GetError();
            GL.UniformMatrix4(location, false, ref value);
            error = GL.GetError();
        }

        public void Delete()
        {
            GL.DeleteShader(this.vertexID);
            GL.DeleteShader(this.fragmentID);
            GL.DeleteProgram(this.programID);
        }

    }
}