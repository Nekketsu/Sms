namespace Sms.Vdp
{
    public class Mode2Renderer : IVdpRenderer
    {
        private readonly TMS9918A vdp;

        private Color[] oldStyleColors =
        {
            new Color(0, 0, 0), // Transparent
            new Color(0, 0, 0), // Black
            new Color(33, 200, 66), // Medium green
            new Color(94, 220, 120), // Light green
            new Color(84, 85, 237), // Dark blue
            new Color(125, 118, 252), // Light blue
            new Color(212, 82, 77), // Dark red
            new Color(66, 235, 245), // Cyan
            new Color(252, 85, 84), // Medium red
            new Color(255, 121, 120), // Light red
            new Color(212, 193, 84), // Dark yellow
            new Color(230, 206, 84), // Light yellow
            new Color(33, 176, 59), // Dark green
            new Color(201, 91, 186), // Magenta
            new Color(204, 204, 204), // Gray
            new Color(255, 255, 255) // White
        };

        public Mode2Renderer(TMS9918A vdp)
        {
            this.vdp = vdp;
        }

        public void Render()
        {
            RenderSpritesMode2();
            RenderBackgroundMode2();
        }

        public void RenderSpritesMode2()
        {
            var size = vdp.Is8x16 ? 16 : 8;

            var spriteCount = 0;

            for (var sprite = (byte)0; sprite < 32; sprite++)
            {
                var location = (ushort)(vdp.SpriteAttributeTableBase + sprite * 4);
                var y = (int)vdp.VRam[location];

                // D0 means don't draw this sprite and all remaining sprites
                if (y == 0xD0)
                {
                    // If theree is no illegal sprites then store this sprite in the status
                    if (!vdp.SpriteOverFlow)
                    {
                        vdp.Registers.Status &= 0xE0; // Turn off last 5 bits
                        vdp.Registers.Status |= sprite; // Puts spritee into last 5 bits
                    }

                    return; // Need to return not break otherwise outisde of for loop will overwrite lastt 5 bits of status
                }

                if (y > 0xD0)
                {
                    y -= 0x100;
                }

                y++;

                if (vdp.VCounter >= y && vdp.VCounter < y + size)
                {
                    var x = vdp.VRam[(ushort)(location + 1)];
                    var pattern = vdp.VRam[(ushort)(location + 2)];
                    var color = vdp.VRam[(ushort)(location + 3)];
                    var ec = color.HasBit(7);

                    if (ec)
                    {
                        x -= 32;
                    }

                    color &= 0xF;

                    if (color == 0)
                    {
                        continue;
                    }

                    spriteCount++;

                    if (spriteCount > 4)
                    {
                        SetMode2IllegalSprites(sprite);
                        return; // Need to return otherwise outside of the for loop will reset last 5 bits of sprite
                    }
                    else
                    {
                        vdp.SpriteOverFlow = true;
                    }

                    var line = (byte)(vdp.VCounter - y);

                    if (size == 8)
                    {
                        DrawMode2Sprite((ushort)(vdp.SgTable + pattern * 8), x, line, color);
                    }
                    else
                    {
                        var address = (ushort)(vdp.SgTable + (pattern & 252) * 8);
                        DrawMode2Sprite(address, x, line, color);
                        DrawMode2Sprite(address, (byte)(x + 8), (byte)(line + 16), color);
                    }
                }
            }

            vdp.Registers.Status &= 0xE0; // Turn off last 5 bits
            vdp.Registers.Status |= 31; // Puts last sprite into last 5 bits
        }

        public void RenderBackgroundMode2()
        {
            var row = vdp.VCounter;
            var line = (byte)(vdp.VCounter % 8);

            for (var column = 0; column < 32; column++)
            {
                var nameBaseCopy = (ushort)(vdp.NameBase + row * 32 + column);

                var pattern = vdp.VRam[nameBaseCopy];
                var pgAddress = (ushort)(vdp.PgOffset + pattern * 8);

                pgAddress = vdp.PgOffset;
                pgAddress += line;

                var pixelLine = vdp.VRam[pgAddress];
                var colIndex = pattern & vdp.ColAnd;

                var color = vdp.VRam[(ushort)(vdp.ColorTableBase + colIndex * 8 + vdp.PgOffset + line)];
                var fore = (byte)(color >> 4);
                var back = (byte)(color & 0xF);

                var invert = 7;
                for (var x = 0; x < 8; x++, invert--)
                {
                    var colNum = pixelLine.HasBit(invert) ? fore : back;

                    if (colNum == 0)
                    {
                        continue;
                    }

                    var screenColor = oldStyleColors[colNum];
                    var xPos = column * 8 + x;

                    if (vdp.Screen[xPos, vdp.VCounter] != null)
                    {
                        continue;
                    }

                    if (xPos >= TMS9918A.NumResHorizontal)
                    {
                        continue;
                    }

                    vdp.Screen[xPos, vdp.VCounter] = screenColor;
                }
            }
        }

        private void SetMode2IllegalSprites(byte sprite)
        {
            vdp.SpriteOverFlow = true;
            vdp.Registers.Status &= 0xE0; // Turn off last 5 bits
            vdp.Registers.Status |= 31; // Puts last sprite into last 5 bits
        }

        private void DrawMode2Sprite(ushort address, byte x, byte line, byte color)
        {
            var screenColor = oldStyleColors[color];
            var invert = (byte)7;

            for (var i = 0; i < 8; i++, invert--)
            {
                var drawLine = vdp.VRam[(ushort)(address + line)];
                var xPos = x + i;

                // Is this a sprite collision?
                if (vdp.Screen[xPos, vdp.VCounter] != null)
                {
                    vdp.SpriteCollision = true;
                }

                if (!drawLine.HasBit(invert))
                {
                    continue;
                }

                vdp.Screen[xPos, vdp.VCounter] = screenColor;
            }

        }
    }
}
