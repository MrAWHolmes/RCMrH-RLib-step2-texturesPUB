//file Program.cs
using Raylib_cs;
using System.Numerics;

// our class namespace :)
namespace MyImageLib;

public class MyApp
{
    public static void Main()
    {   //force base to use the source code folder and not the bin folder
        // ref : https://copilot.microsoft.com/chats/ven19XW3uJKaDu6zfDxbj
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        // Inititialization - InitWindow to happen first!
        const int screenWidth = 768;
        const int screenHeight = 432;
        Raylib.InitWindow(screenWidth, screenHeight, "raylib [textures] example - image drawing");
        Raylib.SetTargetFPS(60);

        // Load Images
        var background = new MyImage("Assets/Background.jpg",Vector2.Zero,0.5f);
        var cQ = new CircleQ();


        //main game loop
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.RayWhite);
            background.Draw();

            Vector2 MousePos = Raylib.GetMousePosition();

            if (background.IsMouseIndide()){

                System.Console.WriteLine($"Mouse inside image at {MousePos}.");
                MousePos = Raylib.GetMousePosition();
                // draw a +
                int x = (int) MousePos.X;
                int y = (int) MousePos.Y;
                Raylib.DrawLine(x-5,y,x+5,y,Color.Red);  // --
                Raylib.DrawLine(x,y-5,x,y+5,Color.Red);  //  |

                if  (Raylib.IsMouseButtonReleased(MouseButton.Left)){
                    cQ.Enque(new MyCircle(MousePos,10,Color.RayWhite));
                }
                
            }
            else
            {
                if (Raylib.IsMouseButtonReleased(MouseButton.Left))
                {
                    MousePos = Raylib.GetMousePosition();
                    int x = (int) MousePos.X;
                    int y = (int) MousePos.Y;

                    background.MoveTo(x,y);
                    cQ.EmptyTheQueue();
                }
            }

            cQ.DrawQueue();

            Raylib.EndDrawing();
        }

        cQ.EmptyTheQueue();
        background.Unload();

        Raylib.CloseWindow();

   
        

    }//Main
    
}//class:MyApp
