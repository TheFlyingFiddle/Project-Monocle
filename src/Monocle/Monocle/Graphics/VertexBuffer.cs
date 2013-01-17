using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Monocle.Graphics
{
    public class VertexBuffer<T>  where T : struct, IVertex
    {
        private int bufferHandle;
        private BufferUsageHint hint;

        public VertexBuffer(BufferUsageHint hint)
        {
            this.hint = hint;
            InitBuffer();
        }

        private unsafe void InitBuffer()
        {
            GL.GenBuffers(1, out bufferHandle);
        }

        public void SetData(T[] data)
        {
            if(data == null)
                throw new ArgumentNullException();
            else if(data.Length < 1)
                throw new ArgumentException("Data must contain atleas one element!");

            GL.BufferData<T>(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * data[0].SizeInBytes), data, hint);
        }

        public void SetSubData(T[] data, int offset)
        {
            if (data == null)
                throw new ArgumentNullException();
            else if(data.Length < 1)
                throw new ArgumentException("Data must contain atleas one element!");


            GL.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)(offset * data[0].SizeInBytes), (IntPtr)(data.Length * data[0].SizeInBytes), data);
        }



        public void SetSubData(T[] data, int offset, int count)
        {
            GL.BufferSubData<T>(BufferTarget.ArrayBuffer, (IntPtr)(offset * data[0].SizeInBytes), (IntPtr)(count * data[0].SizeInBytes), data);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.bufferHandle);
        }
    }
}