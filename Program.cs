﻿using System;
using System.Runtime.ExceptionServices;
using SFML;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Vector;

namespace UI
{
    static class Program
    {

        static void OnClose(object ?sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow ?window = sender as RenderWindow;
            window?.Close();
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
            // defining globals
            Clock clock = new();
            Time dt = new();

            vec2 mousepos = new();
            Color windowcolour = new Color(0, 192, 255);

            float brightness = 255;

            vec2 lockaxis = new(1, 1);
            vec2 offset = new();

            List<UIelement> uielements = new();

            int page = 0;

            SoundBuffer clickbuffer = new("assets/Click.wav");
            Sound click = new Sound(clickbuffer);

            SoundBufferRecorder recorder = new();

            //defining button layouts for different pages
            //
            UIelement[] menubuttons = {new TextureButton("play", lockaxis, new(0), new(30, 30), "assets/playbutton.png"),
                                    new TextureButton("left", new(1, 1), new(-75, 0), new(30, 30), "assets/leftbutton.png", "assets/altleftbutton.png"),
                                    new TextureButton("right", new(1, 1), new(75, 0), new(30, 30), "assets/rightbutton.png", "assets/altrightbutton.png"),
                                    new LabelButton("up", new(1, 1), new(0, -75), new(30, 30), "Up", "assets/upbutton.png", "assets/altupbutton.png"),
                                    new LabelButton("down", new(1, 1), new(0, 75), new(30, 30), "1234567890", "assets/downbutton.png", "assets/altdownbutton.png"),
                                    new TextureButton("reset", new(1, 1), new(-250, 0), new(20, 20), "assets/reloadbutton.png", "assets/altreloadbutton.png"),
                                    new LabelButton("rec", new(1, 1), new(250, 0), new(20, 20), "Rec", "assets/playbutton.png"),
                                    new UIimage("arrows", new(1, 1), new(0, 0), new(120, 120), "assets/arrowbackground.png")
                                    };

            UIelement[] submenubuttons = { new TextureButton("play", lockaxis, new(0), new(20, 20), "assets/playbutton.png"),
                                        new LabelButton("colour", new(1, 2), new(0, -50), new(20, 20), "Dark mode\n  On / Off"),
                                        new TextureSlider("yoffset", new(1, 1), new(0, -50), new(100, 10), 0.5f, "assets/slide.png", "assets/handle.png", "assets/althandle.png", new(12, 2)),
                                        new TextureSlider("xoffset", new(1, 1), new(0, 50), new(100, 10), 0.5f, "assets/slide.png", "assets/handle.png", "assets/althandle.png", new(12, 2)),
                                        new Label("offsety", new(1, 1), new(150, -50), new(20, 10), "0"),
                                        new Label("offsetx", new(1, 1), new(150, 50), new(20, 10), "0"),
                                        new Slider("no", new(1, 1), new(0, -75), new(100, 10), 1),
                                        new Slider("no", new(1, 1), new(0, 75), new(100, 10), 1),
                                        new Slider("no", new(1, 1), new(0, -100), new(100, 10), 1),
                                        new Slider("no", new(1, 1), new(0, 100), new(100, 10), 1),
                                        new Slider("no", new(1, 1), new(0, -125), new(100, 10), 1),
                                        new TextureSlider("brightness", new(1, 1), new(0, 125), new(100, 10), 1, "assets/sunslide.png", "assets/sunhandle.png", "assets/altsunhandle.png", new(12, 2)),
                                        };


            // CREATING MAIN WINDOW
            Styles style = Styles.Default;
            RenderWindow app = new RenderWindow(new VideoMode(640, 360), "Game");

            app.SetVerticalSyncEnabled(true);
            //app.SetFramerateLimit(10);


            // CAMERA RESIZING EVENT STUFF
            vec2 camsize = new(1000, 562.5);
            vec2 minsize = new(640, 360);
            View camera = new View(new Vector2f(400, 400), new Vector2f());

            camera = windowresize(camera, camsize, minsize, app.Size.X, app.Size.Y);
            app.SetView(camera);

            void OnKeyPress(object ?sender, KeyEventArgs e)
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
                    app.MouseButtonPressed += OnMousedown;
                    app.MouseButtonReleased += OnMouseup;

                    camera = windowresize(camera, camsize, minsize, app.Size.X, app.Size.Y);
                    app.SetView(camera);
                }
                if (e.Scancode == Keyboard.Scancode.Escape)
                {
                    app.Close();
                }
            }

            void OnResize(object ?sender, SizeEventArgs e)
            {
                RenderWindow? window = sender as RenderWindow;

                camera = windowresize(camera, camsize, minsize, e.Width, e.Height);
                window?.SetView(camera);
            }

            app.Closed += OnClose;
            app.Resized += OnResize;
            app.KeyPressed += OnKeyPress;

            // UI EVENT STUFF

            void OnMousedown(object ?sender, MouseButtonEventArgs e)
            {
                foreach (UIelement element in uielements.ToList())
                {
                    Button? button = element as Button;
                    button?.checkpress(mousepos, app);

                    Slider? slider = element as Slider;
                    slider?.checkpress(mousepos, app);
                }
            }

            void OnMouseup(object ?sender, MouseButtonEventArgs e)
            {
                foreach (UIelement element in uielements.ToList())
                {
                    Button? button = element as Button;
                    button?.checkrelease();

                    Slider? slider = element as Slider;
                    slider?.checkrelease();
                }
            }

            app.MouseButtonPressed += OnMousedown;
            app.MouseButtonReleased += OnMouseup;

            void OnButtonPress(object sender, EventArgs e)
            {
                string ?name = sender.ToString();
                if (name != "rec")
                {
                    click.Play();
                }

                if (name == "left")
                {
                    lockaxis.x = 0;
                }
                else if (name == "right")
                {
                    lockaxis.x = 2;
                }
                else if (name == "down")
                {
                    lockaxis.y = 2;
                }
                else if (name == "up")
                {
                    lockaxis.y = 0;
                }
                else if (name == "reset")
                {
                    lockaxis = new(1, 1);
                }
                else if(name == "colour")
                {
                    if (windowcolour.B == 255)
                    {
                        windowcolour = new Color(0, 0, 0);
                    }
                    else
                    {
                        windowcolour = new Color(0, 192, 255);
                    }
                }
                else if(name == "rec")
                {
                    recorder.Start();
                }
            }
            void OnButtonRelease(object sender, EventArgs e)
            {
                string? name = sender.ToString();

                if (name == "play")
                {
                    if (page == 0)
                    {
                        page = 1;

                        uielements.Clear();
                        uielements = new(submenubuttons);
                    }
                    else
                    {
                        page = 0;

                        uielements.Clear();
                        uielements = new(menubuttons);
                    }
                }
                else if (name == "rec")
                {
                    recorder.Stop();
                    click = new(recorder.SoundBuffer);
                }
            }

            void SliderCheck(string name, float var)
            {
                if (name == "brightness")
                {
                    brightness = 255 * var;
                }
                else if (name == "xoffset")
                {
                    offset.x = 500 * var - 250;
                }
                else if (name == "yoffset")
                {
                    offset.y = -500 * var + 250;
                }
            }

            void LabelCheck(Label label)
            {
                string newtext = label.text.DisplayedString;
                string name = label.name;
                if (name == "offsetx")
                {
                    newtext = "X position: " + ((int)offset.x).ToString();
                }
                else if (name == "offsety")
                {
                    newtext = "Y position: " + ((int)offset.y).ToString();
                }
                else
                {
                    return;
                }

                if (newtext != label.text.DisplayedString)
                {
                    label.retext(newtext);
                    Console.WriteLine("changed");
                }
            }

            // CREATING A UI

            // initialising the eventhandlers
            foreach (UIelement element in menubuttons)
            {
                Button? button = element as Button;
                if (button != null)
                {
                    button.buttondown += OnButtonPress;
                    button.buttonup += OnButtonRelease;
                }
            }

            foreach (UIelement element in submenubuttons)
            {
                Button? button = element as Button;

                if (button != null)
                {
                    button.buttondown += OnButtonPress;
                    button.buttonup += OnButtonRelease;
                }
            }

            uielements = new List<UIelement>(menubuttons);

            // Start the game loop
            while (app.IsOpen)
            {
                // Process events
                app.DispatchEvents();

                mousepos = new(Mouse.GetPosition(app).X, Mouse.GetPosition(app).Y);

                foreach (UIelement element in uielements.ToList())
                {
                    Slider? slider = element as Slider;
                    if (slider != null)
                    {
                        float var = slider.update(mousepos, app);
                        SliderCheck(slider.name, var);
                    }
                    Label? label = element as Label;
                    if (label != null)
                    {
                        LabelCheck(label);
                    }
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                }
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    //lockaxis.x = Math.Clamp(MathF.Floor(3 * (float)mousepos.x / app.Size.X), 0, 2);
                    //lockaxis.y = Math.Clamp(MathF.Floor(3 * (float)mousepos.y / app.Size.Y), 0, 2);
                }

                uielements[0].xlock = (uint)lockaxis.x;
                uielements[0].ylock = (uint)lockaxis.y;
                uielements[0].xoffset = offset.x;
                uielements[0].yoffset = offset.y;

                // Clear screen
                app.Clear(windowcolour * new Color((byte)brightness, (byte)brightness, (byte)brightness));

                //draw things here
                for (int i = uielements.Count - 1; i >= 0; i--)
                {
                    uielements[i].draw(app);
                }

                // Update the window
                app.Display();
            }
        }
    }
}
