using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Monocle.Graphics
{
    class ShortIndexBuffer
    {
        private readonly BufferUsageHint hint;
        private readonly int handle;
        
        public ShortIndexBuffer(BufferUsageHint hint)
        {
            GL.GenBuffers(1, out handle);
            this.hint = hint;
        }

        public void SetData(ushort[] data)
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(data.Length * 2), data, hint);
        }

        public void SetSubData(ushort[] data, int offset)
        {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 2), (IntPtr)(data.Length * 2), data);
        }

        public void SetSubData(ushort[] data, int offset, int length)
        {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 2), (IntPtr)(length * 2), data);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.handle);
        }
    }

    class IntIndexBuffer
    {
        private readonly BufferUsageHint hint;
        private readonly int handle;

        public IntIndexBuffer(BufferUsageHint hint)
        {
            GL.GenBuffers(1, out handle);
            this.hint = hint;
        }

        public void SetData(int[] data)
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(data.Length * 4), data, hint);
        }

        public void SetSubData(int[] data, int offset)
        {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 4), (IntPtr)(data.Length * 4), data);
        }

        public void SetSubData(int[] data, int offset, int length)
        {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)(offset * 4), (IntPtr)(length * 4), data);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, this.handle);
        }
    }
}