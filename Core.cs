using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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
        _aspectRatio = (float)width / (float)height;
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

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
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


    float _aspectRatio = new float();
    float _fov = MathHelper.PiOver2;

    private Vector3 _position_model { get; set; } = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 _position_eye { get; set; } = new Vector3(0.0f, 0.0f, 1.5f);
    private Vector3 _up { get; set; } = new Vector3(0.0f, 1.0f, 0.0f);

    private float _pitch;
    private float _yaw;

    private bool _firstMove = true;
    private Vector2 _lastPos;
    const float _sensitivity = 0.2f;
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        if (MouseState.IsButtonDown(MouseButton.Left))
        {
            if (_firstMove)
            {
                _lastPos = new Vector2(MouseState.X, MouseState.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = MouseState.X - _lastPos.X;
                var deltaY = MouseState.Y - _lastPos.Y;
                _lastPos = new Vector2(MouseState.X, MouseState.Y);

                _yaw += deltaX * _sensitivity;
                _pitch -= deltaY * _sensitivity;
            }

        }
        if (MouseState.IsButtonReleased(MouseButton.Left))
        {
            _firstMove = true;
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
        //_yaw += 128.0f * (float)e.Time;

        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        var model = Matrix4.Identity * 
            Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(_yaw)) * 
            Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(-_pitch));
        
        var view = Matrix4.LookAt(_position_eye, _position_model, _up);
        var projection = Matrix4.CreatePerspectiveFieldOfView(_fov, _aspectRatio, 0.01f, 100f);


        var key0 = GL.GetActiveUniform(_ProgramHandle, 0, out _, out _);
        var location0 = GL.GetUniformLocation(_ProgramHandle, key0);

        var key1 = GL.GetActiveUniform(_ProgramHandle, 1, out _, out _);
        var location1 = GL.GetUniformLocation(_ProgramHandle, key1);

        var key2 = GL.GetActiveUniform(_ProgramHandle, 2, out _, out _);
        var location2 = GL.GetUniformLocation(_ProgramHandle, key2);

        GL.UseProgram(_ProgramHandle);

        GL.UniformMatrix4(location0, true, ref model);
        GL.UniformMatrix4(location1, true, ref view);
        GL.UniformMatrix4(location2, true, ref projection);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        
        GL.Viewport(0, 0, e.Width, e.Height);

        _aspectRatio = Size.X / (float)Size.Y;
    }
}