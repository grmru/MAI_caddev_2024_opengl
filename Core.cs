using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MAI.CadDev;

public class Core : GameWindow
{
    public Core(int width, int height, string title) 
    : base(new GameWindowSettings(), 
           new NativeWindowSettings() 
           { 
              ClientSize = new OpenTK.Mathematics.Vector2i(width, height), 
              Title = title 
           }) 
    {

    }

    float[] vertices = {
        -0.5f, -0.5f, 0.0f, //Bottom-left vertex
        0.5f, -0.5f, 0.0f,  //Bottom-right vertex
        0.0f,  0.5f, 0.0f   //Top vertex
    };

    int _VertexBufferObject = 0;
    int _VertexArrayObject = 0;

    int _VertexShader = 0;
    int _FragmentShader = 0;
    int _ProgramHandle = 0;

    string sVertexShader = @"
#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}
";

    string sFragmentShader = @"
#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}
";

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _VertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _VertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        _VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_VertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _VertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(_VertexShader, sVertexShader);

        _FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(_FragmentShader, sFragmentShader);


        GL.CompileShader(_VertexShader);
        GL.GetShader(_VertexShader, ShaderParameter.CompileStatus, out int successV);
        if (successV == 0)
        {
            string infoLog = GL.GetShaderInfoLog(_VertexShader);
            Console.WriteLine(infoLog);
        }

        GL.CompileShader(_FragmentShader);
        GL.GetShader(_FragmentShader, ShaderParameter.CompileStatus, out int successF);
        if (successF == 0)
        {
            string infoLog = GL.GetShaderInfoLog(_FragmentShader);
            Console.WriteLine(infoLog);
        }


        _ProgramHandle = GL.CreateProgram();
        GL.AttachShader(_ProgramHandle, _VertexShader);
        GL.AttachShader(_ProgramHandle, _FragmentShader);
        GL.LinkProgram(_ProgramHandle);

        GL.GetProgram(_ProgramHandle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0)
        {
            string infoLog = GL.GetProgramInfoLog(_ProgramHandle);
            Console.WriteLine(infoLog);
        }

        GL.UseProgram(_ProgramHandle);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        //Code goes here.

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }
}