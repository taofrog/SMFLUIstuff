using System;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Vector;

namespace UI
{
    static class Program
    {
        /*static void drawcollider(Collider body, RenderWindow screen, Color colour)
        {
            RectangleShape r = new RectangleShape(new Vector2f(body.size.x, body.size.y));
            r.Origin = new Vector2f(body.half_size.x, body.half_size.y);
            r.Position = new Vector2f(body.position.x, body.position.y);

            r.FillColor = colour;
            screen.Draw(r);
        }
        */
        static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        static View windowresize(View camera, vec2 maxsize, vec2 minsize, float resizedx, float resizedy)
        {
            float aspectratio = maxsize.x / maxsize.y;
            vec2 ratios = new(Math.Min((float)resizedx / (aspectratio * (float)resizedy), 1), Math.Min((aspectratio * (float)resizedy) / (float)resizedx, 1));
            camera.Size = new Vector2f(maxsize.x * ratios.x, maxsize.y * ratios.y);

            return camera;
        }

        static void Main()
        {
            Clock clock = new();
            Time dt = new();
            // Create the main window
            Styles style = Styles.Default;
            RenderWindow app = new RenderWindow(new VideoMode(640, 360), "Game");

            app.SetVerticalSyncEnabled(true);
            //app.SetFramerateLimit(10);

            vec2 camsize = new(1000, 562.5);
            vec2 minsize = new(640, 360);
            View camera = new View(new Vector2f(400, 400), new Vector2f());

            camera = windowresize(camera, camsize, minsize, app.Size.X, app.Size.Y);
            app.SetView(camera);

            void OnKeyPress(object sender, KeyEventArgs e)
            {
                if (e.Scancode == Keyboard.Scancode.F11)
                {
                    string title;
                    if (style == Styles.Default)
                    {
                        style = Styles.Fullscreen;
                        title = "fullscreen game";
                    }
                    else
                    {
                        style = Styles.Default;
                        title = "windowed game";
                    }

                    app.Close();
                    app = new RenderWindow(new VideoMode(1920, 1080), title, style);

                    app.SetVerticalSyncEnabled(true);
                    //app.SetFramerateLimit(60);

                    app.Closed += OnClose;
                    app.Resized += OnResize;
                    app.KeyPressed += OnKeyPress;

                    camera = windowresize(camera, camsize, minsize, app.Size.X, app.Size.Y);
                }
                if (e.Scancode == Keyboard.Scancode.Escape)
                {
                    app.Close();
                }
            }

            void OnResize(object sender, SizeEventArgs e)
            {
                RenderWindow? window = sender as RenderWindow;

                camera = windowresize(camera, camsize, minsize, e.Width, e.Height);
                window.SetView(camera);
            }

            app.Closed += OnClose;
            app.Resized += OnResize;
            app.KeyPressed += OnKeyPress;

            Color windowColor = new Color(0, 192, 255);

            vec2 lockaxis = new();

            // Start the game loop
            while (app.IsOpen)
            {
                // Process events
                app.DispatchEvents();

                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                }
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    vec2 mousepos = new(Mouse.GetPosition(app).X, Mouse.GetPosition(app).Y);
                    lockaxis.x = Math.Clamp(MathF.Floor(3 * (float)mousepos.x / app.Size.X), 0, 2);
                    lockaxis.y = Math.Clamp(MathF.Floor(3 * (float)mousepos.y / app.Size.Y), 0, 2);
                }

                UIelement button = new UIelement(lockaxis, new(0), new(50, 30));

                // Clear screen
                app.Clear(windowColor);

                //draw things here
                RectangleShape r = new RectangleShape(new Vector2f(button.size.x*2, button.size.y*2));
                vec2 buttonpos = button.topleft(app.GetView());
                r.Position = new Vector2f(buttonpos.x, buttonpos.y);
                Console.WriteLine(app.GetView().Center);

                r.FillColor = new(255, 0, 0, 255);
                app.Draw(r);

                // Update the window
                app.Display();
            }
        }
    }
}
