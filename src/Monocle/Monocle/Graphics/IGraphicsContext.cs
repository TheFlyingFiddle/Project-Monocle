using System;
using OpenTK.Graphics.OpenGL;
namespace Monocle.Graphics
{
    public interface IGraphicsContext
    {
        Texture2D this[int index] { set; }

        void AttachShader(int programID, int shaderID);
        void BindVertexBuffer(VertexBuffer buffer);
        void BindIndexBuffer(IntIndexBuffer buffer);
        void BindIndexBuffer(ShortIndexBuffer buffer);
        void UseShaderProgram(ShaderProgram program);

        void BindTexture(TextureTarget target, int id);
        void BufferData<T>(BufferTarget target, IntPtr size, T[] data, BufferUsageHint hint) where T : struct;
        void BufferSubData<T>(BufferTarget target, IntPtr offset, IntPtr length, T[] data) where T : struct;
        void CompileShader(int id);
        int CreateProgram();
        int CreateShader(ShaderType type);
        void DeleteBuffers(int id);
        void DeleteBuffers(int[] ids);
        void DeleteProgram(int id);
        void DeleteShader(int id);
        void DeleteTexture(int texture);
        void EnableVertexAttribArray(int index);
        void GenBuffers(int count, out int buffers);
        void GenBuffers(int count, int[] buffers);
        void GenerateMipmap(GenerateMipmapTarget target);
        int GenTexture();
        int GetAttribLocation(int programID, string name);
      
        void GetTexImage<T>(TextureTarget target, 
                            int level, 
                            PixelFormat format,
                            PixelType pixelType,
                            T[] storage) where T : struct;
        
        void LinkProgram(int program);  
        void ShaderSource(int id, string src);     
        void TexImage2D(TextureTarget target, 
                        int level, 
                        PixelInternalFormat intenalFormat, 
                        int width, int height, bool border, PixelFormat format, PixelType pixelType, IntPtr data);
        
        void Uniform1(int location, int value);
        void Uniform1(int location, float value);
        void Uniform1(int location, uint value);
        void Uniform2(int location, ref OpenTK.Vector2 value);
        void Uniform2(int location, int value0, int value1);
        void Uniform2(int location, float value0, float value1);
        void Uniform2(int location, uint value0, uint value1);
        void Uniform3(int location, ref OpenTK.Vector3 value);
        void Uniform3(int location, int value0, int value1, int value2);
        void Uniform3(int location, float value0, float value1, float value2);
        void Uniform3(int location, uint value0, uint value1, uint value2);
        void Uniform4(int location, ref OpenTK.Vector4 value);
        void Uniform4(int location, int value0, int value1, int value2, int value3);
        void Uniform4(int location, float value0, float value1, float value2, float value3);
        void Uniform4(int location, uint value0, uint value1, uint value2, uint value3);
        void UniformMatrix2(int location, float[] matrix, bool transpose);
        void UniformMatrix3(int location, float[] matrix, bool transpose);
        void UniformMatrix4(int location, ref OpenTK.Matrix4 matrix, bool transpose);

        void VertexAttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int strideInBytes, int offsetInBytes);

        void Enable(EnableCap enableCap);

        void BlendFunc(BlendingFactorSrc blendingFactorSrc, BlendingFactorDest blendingFactorDest);

        void ClearColor(OpenTK.Graphics.Color4 color4);
        void DrawElements(BeginMode beginMode, int p, DrawElementsType drawElementsType, int p_2);

        void TexParameter(TextureTarget textureTarget, TextureParameterName textureParameterName, int p);

        string GetShaderInfoLog(int vertID);

        string GetProgramInfoLog(int programID);

        int GetUniformLocation(int programID, string name);

        void Viewport(int x, int y, int Width, int Height);
        void Clear(ClearBufferMask mask);
    }


}
