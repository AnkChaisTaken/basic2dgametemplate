using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime;

namespace AnkGraphics
{
    class Program
    {
        static Form myform = new Form();
        public static Dictionary<int, Object> objects = new Dictionary<int, Object>();

        static int UPDATE_MS = 3;

        public static double gravity = 2;

        public static void Main()
        {
            myform.Text = "Graphics";
            myform.Size = new Size(640, 400);
            myform.StartPosition = FormStartPosition.CenterScreen;

            myform.Show();

            DrawSquare(0, 0, 640, 480, Color.White);

            CreateObject(256, 0, 32, 16, 1, Color.Red, true, true);
            CreateObject(128, 256, 256, 16, 1, Color.Black, true, false);

            //thread update
            Thread t = new Thread(Update);
            t.Start();

            //thread keybinds
            Thread k = new Thread(UpdateKeybinds);
            k.Start();

            Application.Run();
            Console.ReadLine();
        }

        static void CreateObject(int _x, int _y, int _width, int _height, int _mass, Color _color, bool _solidness, bool _gravityAffection)
        {
            Object obj = new Object();
            obj.Create(_x, _y, _width, _height, _mass, _color, _solidness, _gravityAffection);
            objects.Add(objects.Count, obj);
        }

        public static void DrawSquare(int x, int y, int width, int height, Color color)
        {
            using (Graphics g = myform.CreateGraphics())
            {
                SolidBrush Bbrush = new SolidBrush(color);
                Rectangle rect = new Rectangle(x, y, width, height);
                g.FillRectangle(Bbrush, rect);
            }
        }

        public static void ClearField()
        {
            using (Graphics g = myform.CreateGraphics())
            {
                g.Clear(Color.White);
            }
        }

        private static void Update()
        {
            foreach(Object obj in objects.Values)
            {
                obj.Update();
            }

            //
            Thread.Sleep(UPDATE_MS);
            Update();
        }

        private static void UpdateKeybinds()
        {
            while(true)
            {

            }
        }
    }

    class Object
    {
        private int id;
        public int x, y;
        public int width, height;

        public bool solidness;
        public bool gravityAffection;
        private bool isCollidingDown = false;
        public int mass;
        Color color;

        public void Update()
        {
            Program.DrawSquare(x, y, width, height, color);

            if (gravityAffection)
            {
                foreach(Object obj in Program.objects.Values)
                {
                    if (obj.id != id)
                    {
                        if (obj.solidness)
                        {
                            if (solidness)
                            {
                                if ((y + height) >= obj.y && (y + height) <= (obj.y + obj.height))
                                {
                                    if ((x + width) > obj.x && x < (obj.x + obj.width))
                                    {
                                        isCollidingDown = true;
                                        //Console.WriteLine("Collision detected! x:" + x + " y:" + y);
                                    }else
                                    {
                                        isCollidingDown = false;
                                    }
                                }
                                else
                                {
                                    isCollidingDown = false;
                                }
                            }
                        }
                    }
                }
                if(!isCollidingDown)
                {
                    UpdatePosition(x, y + (Convert.ToInt32(Math.Floor(Program.gravity))));
                }
            }
        }

        public void Create(int _x, int _y, int _width, int _height,int _mass, Color _color, bool _solidness, bool _gravityAffection)
        {
            id = Program.objects.Count;
            width = _width;
            height = _height;
            mass = _mass;
            color = _color;
            solidness = _solidness;
            gravityAffection = _gravityAffection;
            UpdatePosition(_x, _y);
        }

        public void UpdatePosition(int _x, int _y)
        {
            Program.DrawSquare(x, y, width, height, Color.White);
            x = _x;
            y = _y;
            Program.DrawSquare(x, y, width, height, color);
        }
    }
}
