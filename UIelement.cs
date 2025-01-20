using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector;
using SFML;
using SFML.Window;
using SFML.Graphics;
using System.Runtime.InteropServices.Marshalling;
using static SFML.Window.Mouse;

namespace UI
{
    struct UIpos
    {
        public uint xlock;
        public float xoffset;

        public uint ylock;
        public float yoffset;

        public UIpos(vec2 locks, vec2 offset)
        {
            xlock = (uint)locks.x;
            ylock = (uint)locks.y;

            xoffset = offset.x;
            yoffset = offset.y;
        }

        public UIpos(uint lockx, uint locky, float offsetx, float offsety)
        {
            xlock = (uint)lockx;
            ylock = (uint)locky;

            xoffset = offsetx;
            yoffset = offsety;
        }
    }

    internal class UIelement
    {
        protected UIpos position;
        protected vec2 halfsize;

        protected Texture texture;
        protected Sprite sprite;

        public uint xlock
        {
            get { return position.xlock; }
            set { position.xlock = value; }
        }
        public uint ylock
        {
            get { return position.ylock; }
            set { position.ylock = value; }
        }
        public float xoffset
        {
            get { return position.xoffset; }
            set { position.xoffset = value; }
        }
        public float yoffset
        {
            get { return position.yoffset; }
            set { position.yoffset = value; }
        }

        // actually halfsize idk why its named that
        public vec2 size
        {
            get { return halfsize; }
            set { halfsize = value; }
        }

        public UIelement(vec2 locked, vec2 offset, vec2 hsize, string texturepath = "notexture.png")
        {
            position = new UIpos(locked, offset);
            halfsize = hsize;

            texture = new Texture(texturepath);
            texture.Smooth = true;
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(2*halfsize.x / texture.Size.X, 2 * halfsize.y / texture.Size.Y);
        }

        public vec2 topleft(View camera)
        {
            vec2 topleft = new();
            vec2 campos = new(camera.Center.X, camera.Center.Y);
            vec2 camsize = new(camera.Size.X/2, camera.Size.Y/2);

            if (position.xlock == 0)
            {
                topleft.x = campos.x - camsize.x + position.xoffset;
            }
            else if (position.xlock == 1)
            {
                topleft.x = campos.x - halfsize.x + position.xoffset;
            }
            else if (position.xlock == 2)
            {
                topleft.x = campos.x + camsize.x - 2*halfsize.x + position.xoffset;
            }

            if (position.ylock == 0)
            {
                topleft.y = campos.y - camsize.y + position.yoffset;
            }
            else if (position.ylock == 1)
            {
                topleft.y = campos.y - halfsize.y + position.yoffset;
            }
            else if (position.ylock == 2)
            {
                topleft.y = campos.y + camsize.y - 2 * halfsize.y + position.yoffset;
            }

            return topleft;
        }

        public virtual void draw(RenderWindow app)
        {
            vec2 pos = topleft(app.GetView());
            sprite.Position = new Vector2f(pos.x, pos.y);

            app.Draw(sprite);
        }

        /*
        ui elements have a snapping point and an offset, to deal with resizing the window.
        each axis can be stuck to an edge of the screen, or the centre, with an offset.
        toggle to scale the element with the screen or not.
        different elements have different hitboxes but all are defined by a rectangle
        make animated and textured UI enements somehow
        should have hovered and clicked options idk how that would work
        maybe seperate for diferent elements

        have a type of element that contains lots of other elements, that would be a dif class

        for buttons we would have a mousepressed and mousereleased function

        */
    }
}
