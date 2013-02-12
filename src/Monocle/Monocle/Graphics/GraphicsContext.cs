using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;

namespace Monocle.Graphics
{
    class OpenGLGraphicsContext : IGraphicsContext
    {
        private Rect viewport;
        private Rect scissor;

        public Rect Viewport
        {
            get { return this.viewport; }
            set
            {
                this.viewport = value;
                GL.Viewport((int)value.X, (int)value.Y, (int)value.W, (int)value.H);            
            }
        }

        public Rect Scissor
        {
            get { return this.scissor; }
            set
            {
                this.scissor = value;
                GL.Scissor((int)value.X, (int)(this.viewport.H - value.Y - value.H), (int)value.W, (int)value.H);
            }
        }


        #region Texture

        public int GenTexture()
        {
            return GL.GenTexture();
        }

        public void BindTexture(TextureTarget target, int id)
        {
            GL.BindTexture(target, id);
        }

        public void DeleteTexture(int texture)
        {
            GL.DeleteTexture(texture);
        }

        public void TexImage2D(TextureTarget target,
                               int level,
                               PixelInternalFormat intenalFormat,
                               int width,
                               int height,
                               bool border,
                               PixelFormat format,
                               PixelType pixelType,
                               IntPtr data)
        {
            GL.TexImage2D(target, level, intenalFormat, width, height, border ? 1 : 0, format, pixelType, data);
        }

        public void GenerateMipmap(GenerateMipmapTarget target)
        {
            GL.GenerateMipmap(target);
        }

        public void GetTexImage<T>(TextureTarget target, int level, PixelFormat format, PixelType pixelType, T[] storage) where T : struct
        {
            GL.GetTexImage<T>(target, level, format, pixelType, storage);
        }

        public Texture2D this[int index]
        {
            set 
            {
                GL.ActiveTexture(TextureUnit.Texture0 + index);
                GL.BindTexture(TextureTarget.Texture2D, value.Handle);
            }
        }

        #endregion
         
        #region Buffer Objects
        
        public void GenBuffers(int count, out int buffers)
        {
            GL.GenBuffers(count, out buffers);
        }

        public void GenBuffers(int count, int[] buffers)
        {
            GL.GenBuffers(count, buffers);
        }

        public void BindVertexBuffer(VertexBuffer buffer)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer != null ? buffer.Handle : 0);
        }

        public void BindIndexBuffer(IntIndexBuffer buffer)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer != null ? buffer.Handle : 0);
        }

        public void BindIndexBuffer(ShortIndexBuffer buffer)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer != null ? buffer.Handle : 0);
        }

        public void DeleteBuffers(int id)
        {
            GL.DeleteBuffers(1, ref id);
        }

        public void DeleteBuffers(int[] ids)
        {
            GL.DeleteBuffers(ids.Length, ids);
        }

        public void BufferData<T>(BufferTarget target, IntPtr size, T[] data, BufferUsageHint hint) where T : struct
        {
            GL.BufferData(target, size, data, hint);
        }

        public void BufferSubData<T>(BufferTarget target, IntPtr offset, IntPtr length, T[] data) where T : struct
        {
            GL.BufferSubData(target, offset, length, data);
        }
        
        #endregion

        #region Programs and shaders

        public int CreateShader(ShaderType type)
        {
            return GL.CreateShader(type);
        }

        public int CreateProgram()
        {
            return GL.CreateProgram();
        }

        public void DeleteShader(int id)
        {
            GL.DeleteShader(id);
        }

        public void DeleteProgram(int id)
        {
            GL.DeleteProgram(id);
        }

        public void ShaderSource(int id, string src)
        {
            GL.ShaderSource(id, src);
        }

        public void CompileShader(int id)
        {
            GL.CompileShader(id);
        }

        public void AttachShader(int programID, int shaderID)
        {
            GL.AttachShader(programID, shaderID);
        }

        public void LinkProgram(int program)
        {
            GL.LinkProgram(program);
        }
        #region Uniforms


        public int GetUniformLocation(int programID, string name)
        {
            return GL.GetUniformLocation(programID, name);
        }

        public void Uniform1(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        public void Uniform1(int location, int value)
        {
            GL.Uniform1(location, value);
        }

        public void Uniform1(int location, uint value)
        {
            GL.Uniform1(location, value);
        }

        public void Uniform2(int location, float value0, float value1)
        {
            GL.Uniform2(location, value0, value1);
        }

        public void Uniform2(int location, uint value0, uint value1)
        {
            GL.Uniform2(location, value0, value1);
        }

        public void Uniform2(int location, int value0, int value1)
        {
            GL.Uniform2(location, value0, value1);
        }

        public void Uniform2(int location, ref Vector2 value)
        {
            GL.Uniform2(location, ref value);

        }

        public void Uniform3(int location, float value0, float value1, float value2)
        {
            GL.Uniform3(location, value0, value1, value2);
        }

        public void Uniform3(int location, uint value0, uint value1, uint value2)
        {
            GL.Uniform3(location, value0, value1, value2);
        }

        public void Uniform3(int location, int value0, int value1, int value2)
        {
            GL.Uniform3(location, value0, value1, value2);
        }

        public void Uniform3(int location, ref Vector3 value)
        {
            GL.Uniform3(location, ref value);
        }

        public void Uniform4(int location, float value0, float value1, float value2, float value3)
        {
            GL.Uniform4(location, value0, value1, value2, value3);
        }

        public void Uniform4(int location, uint value0, uint value1, uint value2, uint value3)
        {
            GL.Uniform4(location, value0, value1, value2, value3);
        }

        public void Uniform4(int location, int value0, int value1, int value2, int value3)
        {
            GL.Uniform4(location, value0, value1, value2, value3);
        }

        public void Uniform4(int location, ref Vector4 value)
        {
            GL.Uniform4(location, value);
        }

        public void UniformMatrix2(int location, float[] matrix, bool transpose)
        {
            GL.UniformMatrix2(location, matrix.Length, transpose, matrix);
        }

        public void UniformMatrix3(int location, float[] matrix, bool transpose)
        {
            GL.UniformMatrix3(location, matrix.Length, transpose, matrix);
        }

        public void UniformMatrix4(int location, ref Matrix4 matrix, bool transpose)
        {
            GL.UniformMatrix4(location, transpose, ref matrix);
        }

        #endregion

        #region Attributes

        public int GetAttribLocation(int programID, string name)
        {
            return GL.GetAttribLocation(programID, name);
        }

        public void EnableVertexAttribArray(int index)
        {
            GL.EnableVertexAttribArray(index);
        }

        public void VertexAttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int strideInBytes, int offsetInBytes)
        {
            GL.VertexAttribPointer(index, size, type, normalized, strideInBytes, offsetInBytes);
        }

        #endregion

        #endregion

        #region Misc

        public void Enable(EnableCap enableCap)
        {
            GL.Enable(enableCap);
        }

        public void BlendFunc(BlendingFactorSrc blendingFactorSrc, BlendingFactorDest blendingFactorDest)
        {
            GL.BlendFunc(blendingFactorSrc, blendingFactorDest);
        }
        
        #endregion

        #region Drawing

        public void DrawElements(BeginMode beginMode, int elementCount, DrawElementsType drawElementsType, int offsetsInBytes)
        {
            GL.DrawElements(beginMode, elementCount, drawElementsType, offsetsInBytes);
        }

        #endregion

        public void TexParameter(TextureTarget textureTarget, TextureParameterName textureParameterName, int format)
        {
            GL.TexParameter(textureTarget, textureParameterName, format);
        }

        public void UseShaderProgram(ShaderProgram program)
        {
            GL.UseProgram(program != null ? program.Handle : 0);
        }

        public string GetShaderInfoLog(int shaderID)
        {
            return GL.GetShaderInfoLog(shaderID);
        }

        public string GetProgramInfoLog(int programID)
        {
            return GL.GetProgramInfoLog(programID);
        }

        public void Clear(Color4 color, ClearBufferMask mask)
        {
            GL.ClearColor(color);
            GL.Clear(mask);
        }


        public void GenRenderBuffers(int n, out int handle)
        {
            GL.GenRenderbuffers(n, out handle);
        }

        public void GenFrameBuffers(int n, out int handle)
        {
            GL.GenFramebuffers(n, out handle);
        }

        public void BindFrameBuffer(FramebufferTarget framebufferTarget, int handle)
        {
            GL.BindFramebuffer(framebufferTarget, handle);
        }

        public void FrameBufferTexture2D(FramebufferTarget framebufferTarget, FramebufferAttachment framebufferAttachment, TextureTarget textureTarget, int textureHandle, int level)
        {
            GL.FramebufferTexture2D(framebufferTarget, framebufferAttachment, textureTarget, textureHandle, level);
        }

        public void DeleteFrameBuffers(int handle)
        {
            GL.DeleteFramebuffers(1, ref handle);
        }

        public void DeleterRenderBuffers(int handle)
        {
            GL.DeleteRenderbuffers(1, ref handle);
        }
    }

    class DebugGraphicsContext : IGraphicsContext
    {
        private readonly IGraphicsContext forwarding;
        public int GLCALLS = 0;


        public DebugGraphicsContext(IGraphicsContext forwarding)
        {
            this.forwarding = forwarding;
        }

        private void CheckGLError()
        {
            ErrorCode code = GL.GetError();
            if (code != ErrorCode.NoError)
                throw new GraphicsException("Error in graphics! ErrorCode : " + code);
        }

        public void AttachShader(int programID, int shaderID)
        {
            forwarding.AttachShader(programID, shaderID);
            CheckGLError();

            GLCALLS++;
        }

        public void BindTexture(TextureTarget target, int id)
        {
            forwarding.BindTexture(target, id);
            CheckGLError();


            GLCALLS++;
        }

        public void BufferData<T>(BufferTarget target, IntPtr size, T[] data, BufferUsageHint hint) where T : struct
        {
            forwarding.BufferData(target, size, data, hint);
            CheckGLError();
            GLCALLS++;
        }

        public void BufferSubData<T>(BufferTarget target, IntPtr offset, IntPtr length, T[] data) where T : struct
        {
            forwarding.BufferSubData(target, offset, length, data);
            CheckGLError();
            GLCALLS++;
        }

        public void CompileShader(int id)
        {
            forwarding.CompileShader(id);
            CheckGLError();
            GLCALLS++;
        }

        public int CreateProgram()
        {
            int tmp = forwarding.CreateProgram();
            CheckGLError();
            GLCALLS++;
            return tmp;
        }

        public int CreateShader(ShaderType type)
        {
           int tmp = forwarding.CreateShader(type);
           CheckGLError();
           GLCALLS++;
           return tmp;
        }

        public void DeleteBuffers(int id)
        {
            forwarding.DeleteBuffers(id);
            CheckGLError();
            GLCALLS++;
        }

        public void DeleteBuffers(int[] ids)
        {
            forwarding.DeleteBuffers(ids);
            CheckGLError();
            GLCALLS++;
        }

        public void DeleteProgram(int id)
        {
            forwarding.DeleteProgram(id);
            CheckGLError();
            GLCALLS++;
        }

        public void DeleteShader(int id)
        {
            forwarding.DeleteShader(id);
            CheckGLError();
            GLCALLS++;
        }

        public void DeleteTexture(int texture)
        {
            forwarding.DeleteTexture(texture);
            CheckGLError();
            GLCALLS++;
        }

        public void EnableVertexAttribArray(int index)
        {
            forwarding.EnableVertexAttribArray(index);
            CheckGLError();
            GLCALLS++;
        }

        public void GenBuffers(int count, out int buffers)
        {
            forwarding.GenBuffers(count, out buffers);
            CheckGLError();
            GLCALLS++;
        }

        public void GenBuffers(int count, int[] buffers)
        {
            forwarding.GenBuffers(count, buffers);
            CheckGLError();
            GLCALLS++;
        }

        public void GenerateMipmap(GenerateMipmapTarget target)
        {
            forwarding.GenerateMipmap(target);
            CheckGLError();
            GLCALLS++;
        }

        public int GenTexture()
        {
            int tmp = forwarding.GenTexture();
            CheckGLError();
            GLCALLS++;
            return tmp;
        }

        public int GetAttribLocation(int programID, string name)
        {
            int tmp = forwarding.GetAttribLocation(programID, name);
            CheckGLError();
            GLCALLS++;
            return tmp;
        }

        public void GetTexImage<T>(TextureTarget target, int level, PixelFormat format, PixelType pixelType, T[] storage) where T : struct
        {
            forwarding.GetTexImage(target, level, format, pixelType, storage);
            CheckGLError();
            GLCALLS++;
        }

        public void LinkProgram(int program)
        {
            forwarding.LinkProgram(program);
            CheckGLError();
            GLCALLS++;
        }

        public void ShaderSource(int id, string src)
        {
            forwarding.ShaderSource(id, src);
            CheckGLError();
            GLCALLS++;
        }

        public void TexImage2D(TextureTarget target, int level, PixelInternalFormat intenalFormat, int width, int height, bool border, PixelFormat format, PixelType pixelType, IntPtr data)
        {
            forwarding.TexImage2D(target, level, intenalFormat, width, height, border, format, pixelType, data);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform1(int location, int value)
        {
            forwarding.Uniform1(location, value);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform1(int location, float value)
        {
            forwarding.Uniform1(location, value);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform1(int location, uint value)
        {
            forwarding.Uniform1(location, value);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform2(int location, ref Vector2 value)
        {
            forwarding.Uniform2(location, ref value);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform2(int location, int value0, int value1)
        {
            forwarding.Uniform2(location, value0, value1);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform2(int location, float value0, float value1)
        {
            forwarding.Uniform2(location, value0, value1);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform2(int location, uint value0, uint value1)
        {
            forwarding.Uniform2(location, value0, value1);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform3(int location, ref Vector3 value)
        {
            forwarding.Uniform3(location, ref value);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform3(int location, int value0, int value1, int value2)
        {
            forwarding.Uniform3(location, value0, value1, value2);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform3(int location, float value0, float value1, float value2)
        {
            forwarding.Uniform3(location, value0, value1, value2);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform3(int location, uint value0, uint value1, uint value2)
        {
            forwarding.Uniform3(location, value0, value1, value2);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform4(int location, ref Vector4 value)
        {
            forwarding.Uniform4(location, ref value);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform4(int location, int value0, int value1, int value2, int value3)
        {
            forwarding.Uniform4(location, value0, value1, value2, value3);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform4(int location, float value0, float value1, float value2, float value3)
        {
            forwarding.Uniform4(location, value0, value1, value2, value3);
            CheckGLError();
            GLCALLS++;
        }

        public void Uniform4(int location, uint value0, uint value1, uint value2, uint value3)
        {
            forwarding.Uniform4(location, value0, value1, value2, value3);
            CheckGLError();
            GLCALLS++;
        }

        public void UniformMatrix2(int location, float[] matrix, bool transpose)
        {
            forwarding.UniformMatrix2(location, matrix, transpose);
            CheckGLError();
            GLCALLS++;
        }

        public void UniformMatrix3(int location, float[] matrix, bool transpose)
        {
            forwarding.UniformMatrix3(location, matrix, transpose);
            CheckGLError();
            GLCALLS++;
        }

        public void UniformMatrix4(int location, ref Matrix4 matrix, bool transpose)
        {
            forwarding.UniformMatrix4(location, ref matrix, transpose);
            CheckGLError();
            GLCALLS++;
        }

        public void VertexAttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int strideInBytes, int offsetInBytes)
        {
            forwarding.VertexAttribPointer(index, size, type, normalized, strideInBytes, offsetInBytes);
            CheckGLError();
            GLCALLS++;
        }

        public void Enable(EnableCap enableCap)
        {
            forwarding.Enable(enableCap);
            CheckGLError();
            GLCALLS++;
        }

        public void BlendFunc(BlendingFactorSrc blendingFactorSrc, BlendingFactorDest blendingFactorDest)
        {
            forwarding.BlendFunc(blendingFactorSrc, blendingFactorDest);
            CheckGLError();
            GLCALLS++;
        }

        public void DrawElements(BeginMode beginMode, int elementCount, DrawElementsType drawElementsType, int offsetInBytes)
        {
            forwarding.DrawElements(beginMode, elementCount, drawElementsType, offsetInBytes);
            CheckGLError();
            GLCALLS++;
        }

        public void BindIndexBuffer(IntIndexBuffer buffer)
        {
            forwarding.BindIndexBuffer(buffer);
            CheckGLError();
            GLCALLS++;
        }

        public void BindIndexBuffer(ShortIndexBuffer buffer)
        {
            forwarding.BindIndexBuffer(buffer);
            CheckGLError();
            GLCALLS++;
        }

        public void BindVertexBuffer(VertexBuffer buffer)
        {
            forwarding.BindVertexBuffer(buffer);
            CheckGLError();
            GLCALLS++;

        }

        public Texture2D this[int index]
        {
            set 
            {
                forwarding[index] = value;
                CheckGLError();
            }
        }

        public void TexParameter(TextureTarget textureTarget, TextureParameterName textureParameterName, int filter)
        {
            forwarding.TexParameter(textureTarget, textureParameterName, filter);
            CheckGLError();
            GLCALLS++;
        }

        public void UseShaderProgram(ShaderProgram program)
        {
            forwarding.UseShaderProgram(program);
            CheckGLError();
            GLCALLS++;
        }

        public string GetShaderInfoLog(int vertID)
        {
            var tmp = forwarding.GetShaderInfoLog(vertID);
            CheckGLError();
            GLCALLS++;
            return tmp;
        }

        public string GetProgramInfoLog(int programID)
        {
            var tmp = forwarding.GetProgramInfoLog(programID);
            CheckGLError();
            GLCALLS++;
            return tmp;
        }

        public int GetUniformLocation(int p, string name)
        {
            var tmp = forwarding.GetUniformLocation(p, name);
            CheckGLError();
            GLCALLS++;
            return tmp;
        }

        public void Clear(Color4 color, ClearBufferMask mask)
        {
            forwarding.Clear(color, mask);
            CheckGLError();
            GLCALLS++;
        }
        

        public Rect Viewport
        {
            get { return this.forwarding.Viewport; }
            set { this.forwarding.Viewport = value; }
        }

        public Rect Scissor
        {
            get { return this.forwarding.Scissor; }
            set { this.forwarding.Scissor = value; }
        }


        public void GenRenderBuffers(int n, out int handle)
        {
            forwarding.GenRenderBuffers(n, out handle);
            CheckGLError();
        }

        public void GenFrameBuffers(int n, out int handle)
        {
            forwarding.GenFrameBuffers(n, out handle);
            CheckGLError();
        }

        public void BindFrameBuffer(FramebufferTarget framebufferTarget, int handle)
        {
            forwarding.BindFrameBuffer(framebufferTarget, handle);
            CheckGLError();
        }

        public void FrameBufferTexture2D(FramebufferTarget framebufferTarget, FramebufferAttachment framebufferAttachment, TextureTarget textureTarget, int textureHandle, int level)
        {
            forwarding.FrameBufferTexture2D(framebufferTarget, framebufferAttachment, textureTarget, textureHandle, level);
            CheckGLError();
        }


        public void DeleteFrameBuffers(int handle)
        {
            forwarding.DeleteFrameBuffers(handle); 
            CheckGLError();
        }

        public void DeleterRenderBuffers(int handle)
        {
            forwarding.DeleterRenderBuffers(handle);
            CheckGLError();
        }
    }
}