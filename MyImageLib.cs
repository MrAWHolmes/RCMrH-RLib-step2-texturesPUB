//file: MyImageLib.cs

// public version

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
                System.Console.WriteLine("Cant deque and empty queue");
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

}//namespace : RLImageDrawingDemo