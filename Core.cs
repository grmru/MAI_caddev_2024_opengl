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

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }
}