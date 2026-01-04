// Readme for RLImageDrawingDemo Project
// ref2: https://github.com/raylib-cs/raylib-cs - image drawing
// ref3: https://github.com/raylib-cs/raylib-cs/blob/master/Examples/Textures/ImageDrawing.cs 
// ref4: https://www.bing.com/search?pglt=2083&q=c%23+destructor&cvid=d16c3cc4c9fc4c309a70b36ca7788d07&gs_lcrp=EgRlZGdlKgYIABBFGDkyBggAEEUYOTIGCAEQABhAMgYIAhAAGEAyBggDEAAYQDIGCAQQABhAMgYIBRAAGEAyBggGEAAYQDIGCAcQABhAMgYICBBFGDoyCAgJEOkHGPxV0gEINTg1NWowajGoAgiwAgE&FORM=ANNAB1&PC=U531
// ref 5: https://www.bing.com/search?qs=LT&pq=c%23+check+if+file&sk=CSYN1&sc=14-16&q=c%23+check+if+file+exists&cvid=d59c9408ed4949d2a059c5597aa455bb&gs_lcrp=EgRlZGdlKgcIABAAGPkHMgcIABAAGPkHMgYIARBFGDkyBggCEAAYQDIGCAMQABhAMgYIBBAAGEAyBggFEAAYQDIGCAYQABhAMgYIBxAAGEAyBggIEEUYOjIICAkQ6QcY_FXSAQg1NDA3ajBqNKgCCLACAQ&FORM=ANAB01&PC=U531
// ref5: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/using-exceptions 

//ref 6: https://copilot.microsoft.com/chats/ven19XW3uJKaDu6zfDxbj


[1] Setting up:
A) Create folder
B) Extensions 
b1) Install c# Devkit
b2) Visual Nuget

Article about .net 8 vs 9 vs 10
https://www.linkedin.com/pulse/net-10-vs-9-8-key-differences-features-why-you-should-midrar-khan-all4f/

Open command terminal Ctr+Shift+P
>install new .net sdk
installing .NET 10.0.101-global~x64 .. latest LTS version
Watch out for Authization Popup!

Restart VS Code for safety :)

1C) Start a new .NET console project
Ctr+Shitf+P > .NET NEW Project
Console App
Named : RLTutorial
Default Directory
Choose .sln
Create Project

Run Program.cs
Build will compile and run .. success :)

1D) Use NUGET to install RAYLIB and BINDINGS..
Ctr+Shift+P >Nuget ... choose visual nuget

Search for raylib
Select raylib-cs
install V7+

Most interesting is the addition of line 11-13 of Program.cs
//force base to use the source code folder and not the bin folder
        // ref : https://copilot.microsoft.com/chats/ven19XW3uJKaDu6zfDxbj
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

Herewith the working project code.


```cs
//file: MyImageLib.cs

using Raylib_cs;
using System.Diagnostics;
using System.Numerics;
using System.Xml;

namespace MyImageLib
{

    public class MyImageLibEx : Exception
    {
        public MyImageLibEx(string message) : base(message){}
    }
    public class MyImage
    {
        
        public bool HasTexture;
        private string FileName {get;}

        private Raylib_cs.Texture2D RLTexture;

        private float Scale {get; set;}
        private Vector2 Position {get; set;}

        private Vector2 Dimensions {get; set;}

        private Raylib_cs.Rectangle Rectangle{get; set;}

        private Raylib_cs.Image RLImage;

        public MyImage(string filename = "./Assets/Background.jpg",
                     Vector2 position = default,
                     float scale = 1.0f
                     )
        {
            
            if (!File.Exists(filename)){
                throw new MyImageLibEx($"Image file {filename} does not exist!");    
            }
            FileName = filename;
            if (scale != 0)
            {
                Scale = Math.Abs(scale);
            }
            else
            {
                scale = 1.0f;
            }
            if (position == default)
            {
                position = new Vector2(0.0f,0.0f);
            }
            Position = position;

            //load the image into RAM
            RLImage = Raylib.LoadImage(FileName);
            Dimensions = new Vector2(RLImage.Width,RLImage.Height);
            Dimensions = Scale * Dimensions;
            Rectangle = new Raylib_cs.Rectangle(Position.X,Position.Y,
                                             Dimensions.X,Dimensions.Y);

            //Draw Image and Grab as Texture a Texture, releasing the Image from RAM
            Raylib_cs.Rectangle src = new Raylib_cs.Rectangle(0,0,Dimensions.X,Dimensions.Y);
            RLTexture = Raylib.LoadTextureFromImage(RLImage);
            Raylib.UnloadImage(RLImage);
            HasTexture = true;

        }//constructor

        public void UpdateRLRectangle()
        {
            Rectangle = new Raylib_cs.Rectangle(Position.X,Position.Y,
                                             Dimensions.X,Dimensions.Y);
        }

        public void Draw()
        {   
            if (!HasTexture) throw new MyImageLibEx($"Image with file {FileName} has no texture!");
            
            //int px = (int)Position.X;
            //int py = (int)Position.Y;
            //Raylib.DrawTexture(RLTexture,px,py,Color.White);
            float rotation = 0.0f;
            Raylib.DrawTextureEx(RLTexture,Position,rotation,Scale,Color.White);
        }

        public void MoveTo(int x, int y)
        {
            Position = new Vector2 ((float) x, (float) y);
            UpdateRLRectangle();
        }

        public void MoveBy(int dx, int dy)
        {
            Position += new Vector2 ((float) dx, (float) dy);
            UpdateRLRectangle();
        }

        public bool IsMouseIndide()
        {
            Vector2 mouse = Raylib.GetMousePosition();
            return Raylib.CheckCollisionPointRec(mouse,Rectangle);
        }

        public void Unload()
        {
            Raylib.UnloadTexture(RLTexture);
            HasTexture = false;
        }//Unload


    }//class:Image

    class MyCircle{

        Vector2 Position;
        float Radius;

        Color Colour;

        public MyCircle(Vector2 position, float radius, Color colour)
    {
        Position = position;
        Radius = radius;
        Colour = colour;
        
    }

    public void Draw()
    {
        int x = (int) Position.X;
        int y = (int) Position.Y;
        Raylib.DrawCircle(x,y,Radius,Colour);
    }
    }//class MyCircle

    class CircleQ
    {
        List<MyCircle> Queue;

        int Head;
        int Tail;

        public CircleQ()
        {
            Queue = new List<MyCircle>();
            Head = -1;
            Tail = -1;
        }//constructor

        public void Enque(MyCircle circle)
        {
            
            Queue.Add(circle);
            Tail = Queue.Count-1;
            
        }

        public MyCircle Dequeue()
        {
            if (Tail >= 0)
            {
                var tail = Queue[Tail];
                Queue.RemoveAt(0);
                Tail = Queue.Count-1;
                return tail;
            }
            else
            {
                System.Console.WriteLine("Cand deque and empty queue");
                return null;
            }
        }

        public void DrawQueue()
        {   if (Queue.Count == 0) return;
            foreach (MyCircle circle in Queue)
            {
                circle.Draw();
            }
        }

        public void EmptyTheQueue()
        {
            while (Queue.Count > 0){
                Dequeue();
            }
        }

    }//CircleQ

}//namespace : MyImageLib

// ------------------------------end of file

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

// ------------------------------end of file

'''


