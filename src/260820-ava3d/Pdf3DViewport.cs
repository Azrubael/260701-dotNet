using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;

namespace _260820_ava3d;

public class Pdf3DViewport : OpenGlControlBase
{
    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        // 1. Clear the buffer
        gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        gl.Clear(GlConsts.GL_COLOR_BUFFER_BIT | GlConsts.GL_DEPTH_BUFFER_BIT);

        // 2. Logic to render the extracted U3D/PRC mesh goes here
    }
}