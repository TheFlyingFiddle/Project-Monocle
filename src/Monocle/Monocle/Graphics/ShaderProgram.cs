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

        //TEMPORARY FIX IN A BETTER FASION!!!!!
        private readonly Dictionary<string, int> attributeMap;
        private readonly Dictionary<string, int> uniformMap;

        private ShaderProgram(IGraphicsContext context, int vertexID, int fragmentID, int programID)
        {
            this.vertexID = vertexID;
            this.fragmentID = fragmentID;
            this.Handle = programID;
            this.GraphicsContext = context;

            this.attributeMap = new Dictionary<string, int>();
            this.uniformMap = new Dictionary<string, int>();
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
            int location;
            if (!uniformMap.TryGetValue(name, out location))
            {
                location = GraphicsContext.GetUniformLocation(this.Handle, name);
                if (location == -1)
                    throw new ArgumentException();
                else
                    uniformMap.Add(name, location);
            }

            GraphicsContext.Uniform1(location, value);    
        }

        public void SetUniform(string name, float value)
        {
            int location;
            if (!uniformMap.TryGetValue(name, out location))
            {
                location = GraphicsContext.GetUniformLocation(this.Handle, name);
                if (location == -1)
                    throw new ArgumentException();
                else
                    uniformMap.Add(name, location);
            }

            GraphicsContext.Uniform1(location, value);    
        }

        public void SetUniform(string name , ref Matrix4 value)
        {
            int location;
            if(!uniformMap.TryGetValue(name, out location)) 
            {
                location = GraphicsContext.GetUniformLocation(this.Handle, name);
                if (location == -1)
                    throw new ArgumentException();
                else
                    uniformMap.Add(name, location);
            }

            GraphicsContext.UniformMatrix4(location, ref value, false);        
        }

        //TEMPORARY METHOD REMOVE THIS AS SOON AS POSSIBLE!!!
        public int GetAttributeLocation(string name)
        {
            int location;
            if (attributeMap.TryGetValue(name, out location))
            {
                return location;
            }

            location = this.GraphicsContext.GetAttribLocation(this.Handle, name);
            if (location == -1)
                throw new ArgumentException();
            else
                this.attributeMap.Add(name, location);

            return location;
        }

        public void Delete()
        {
            GraphicsContext.DeleteShader(this.vertexID);
            GraphicsContext.DeleteShader(this.fragmentID);
            GraphicsContext.DeleteProgram(this.Handle);
        }

    }
}