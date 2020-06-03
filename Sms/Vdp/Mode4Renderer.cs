namespace Sms.Vdp
{
    public class Mode4Renderer : IVdpRenderer
    {
        private readonly TMS9918A vdp;

        public Mode4Renderer(TMS9918A vdp)
        {
            this.vdp = vdp;
        }

        public void Render()
        {
            RenderSpritesMode4();
            RenderBackgroundMode4();
        }

        private void RenderSpritesMode4()
        {
            var spriteCount = 0;
            var size = 8;

            // Is it 8x16 sprite?
            if (vdp.Is8x16)
            {
                size = 16;
            }

            // Is the sprite zoomed?
            if (vdp.IsZommed)
            {
                size = 16;
            }

            // Loop throgh all 64 sprites and see which fall within range of the current scanline
            for (var sprite = 0; sprite < 64; sprite++)
            {
                var y = vdp.VRam[(ushort)(vdp.SpriteAttributeTableBase + sprite)];

                // In small resolution a sprite y value of 0xD0 means stop drawing sprites
                if ((vdp.Screen.Height == TMS9918A.NumResVertical) && (y == 0xD0))
                {
                    break;
                }

                // This is made sure the sprite will wrap if the top part
                // is off screen but the bottom isn't
                if (y > 0xD0)
                {
                    y = (byte)(y - 0x100);
                }

                // Y value is memory is actually y + 1
                y++;

                // Does the sprite fall within range of current scanline taking into account its size?
                if ((vdp.VCounter >= y) && (vdp.VCounter < (y + size)))
                {
                    // Record how many sprites we have drawn this scanline
                    spriteCount++;

                    // We can only draw 8 sprites per scanline. If drawing more set overflow and stop
                    if (spriteCount > 8)
                    {
                        vdp.SpriteOverFlow = true;
                        break;
                    }

                    var x = vdp.VRam[(ushort)(vdp.SpriteAttributeTableBase + 128 + (sprite * 2))];
                    var tileNumber = (ushort)vdp.VRam[(ushort)(vdp.SpriteAttributeTableBase + 129 + (sprite * 2))];

                    // If bit 3 of register 0 is set, x -= 8
                    if (vdp.ShiftX)
                    {
                        x -= 8;
                    }

                    // Are we using first sprite patterns or second
                    if (vdp.UseSecondPattern)
                    {
                        tileNumber += 256;
                    }

                    if (vdp.Is8x16)
                    {
                        // Ignore bit 0 of tile number if using 8x16 for lower tile
                        if (y < vdp.VCounter + 9)
                        {
                            tileNumber &= 0xFFFE;
                        }
                    }

                    // Each tile takes up 32 bytes in memory (4 bytes per line)
                    var startAddress = tileNumber * 32;

                    // Get the exact memory location for the current line being drawn of the tile
                    // Remember each line is 4 bytes
                    startAddress += 4 * (vdp.VCounter - y);

                    // Gather the 4 bytes of data needed to draw the current line of the tile
                    var data1 = vdp.VRam[(ushort)startAddress];
                    var data2 = vdp.VRam[(ushort)(startAddress + 1)];
                    var data3 = vdp.VRam[(ushort)(startAddress + 2)];
                    var data4 = vdp.VRam[(ushort)(startAddress + 3)];

                    // Draw all 8 pixels for the current tile line
                    int i;
                    int col;
                    for (i = 0, col = 7; i < 8; i++, col--)
                    {
                        // Make sure we don't go off the edge of the screen
                        if (x + i >= TMS9918A.NumResHorizontal)
                        {
                            continue;
                        }

                        // Is this a sprite collision?
                        if (vdp.Screen[x + 1, vdp.VCounter] != null)
                        {
                            vdp.SpriteCollision = true;
                            continue;
                        }

                        // Which palette to use?
                        var bit = (byte)(data4.HasBit(col) ? 1 : 0);
                        var palette = (byte)(bit << 3);
                        bit = (byte)(data3.HasBit(col) ? 1 : 0);
                        palette |= (byte)(bit << 2);
                        bit = (byte)(data2.HasBit(col) ? 1 : 0);
                        palette |= (byte)(bit << 1);
                        bit = (byte)(data1.HasBit(col) ? 1 : 0);
                        palette |= bit;

                        // Palette 0 is transparency
                        if (palette == 0)
                        {
                            continue;
                        }

                        // sprites always use the second palette region in memory (hence + 16)
                        var color = vdp.CRam[(ushort)(palette + 16)];

                        // Get its RGB components
                        var red = (byte)(color & 0x3);
                        color >>= 2;
                        var green = (byte)(color & 0x3);
                        color >>= 2;
                        var blue = (byte)(color & 0x3);

                        // We now have all the data needed to color this pixel. So write it to the screen
                        vdp.Screen[x + i, vdp.VCounter] = new Color(Color.GetColorShade(red), Color.GetColorShade(green), Color.GetColorShade(blue));
                    }
                }
            }
        }

        private void RenderBackgroundMode4()
        {
            var vStartingRow = vdp.VScroll >> 3;
            var vFineScroll = vdp.VScroll & 0x7;
            var hStartingCol = vdp.HScroll >> 3;
            var hFineScroll = vdp.HScroll & 0x7;

            // A Row is 8 pixels large
            var row = vdp.VCounter / 8;

            // Draw all 32 columns for this scanline (we only actually draw a pixel of the current column which is the scanline)
            for (var column = 0; column < 32; column++)
            {
                // Draw all 8 pixels in the column
                var invert = 7; // This is used for the color index
                for (var x = 0; x < 8; x++, invert--)
                {
                    var xPixel = x;

                    // Allow scrolling if we are not drawing the first column or limitHScroll is not set
                    var allowHScroll = (row > 1 || !vdp.LimitHScroll);

                    // i is set to the actual pixel on the screen. Each column is 8 pixels
                    var i = x;
                    i += column * 8;

                    // xPos will be the actual pixel to draw on the screen
                    var xPos = i;

                    // Change xPos to appropiate value if allowing horizontal scrolling
                    if (allowHScroll)
                    {
                        xPos = hStartingCol; // Set to horizontal starting column
                        xPos *= 8; // Each column is 8 pixels
                        xPos += xPixel + hFineScroll; // Add fine scroll value and current pixel
                        xPos %= vdp.Screen.Width; // If goes over width then wrap
                    }

                    // Only allow vertical scrolling if not drawing columns 24 to 32 if limitVScroll is set
                    var allowVScroll = !(xPos / 8 > 23 && vdp.LimitVScroll);

                    var vRow = row;

                    // Modify vRow to take into account vertical scrolling
                    if (allowVScroll)
                    {
                        // Add on the starting row value
                        vRow += vStartingRow;

                        // With addition of vFineScroll it may move us to next row.
                        var bumpRow = vdp.VCounter % 8;
                        if (bumpRow + vFineScroll > 7)
                        {
                            vRow++;
                        }

                        // Wrap if going over max rows
                        var mod = vdp.Screen.Height == TMS9918A.NumResVertical ? 28 : 32;
                        vRow = vRow % mod;
                    }

                    var col = column;

                    // Get correct position in memory for this tile
                    var nameBaseOffset = vdp.NameBase;
                    nameBaseOffset += (ushort)(vRow * 64); // each scanline has 32 tiles (1 tile per column) but 1 tile is 2 bytes in memory
                    nameBaseOffset += (ushort)(col * 2); // Each tile is two bytes in memory

                    // Get tile information data from memory
                    var tileData = (ushort)(vdp.VRam[(ushort)(nameBaseOffset + 1)] << 8);
                    tileData |= vdp.VRam[nameBaseOffset];

                    var hiPriority = tileData.HasBit(12);
                    var useSpritePalette = tileData.HasBit(11);
                    var vertFlip = tileData.HasBit(10);
                    var horzFlip = tileData.HasBit(9);
                    var tileDefinition = (ushort)(tileData & 0x1FF);

                    // Offset will point to which line of the pattern to draw
                    var offset = (int)vdp.VCounter;

                    // Move offset if allowVScroll
                    if (allowVScroll)
                    {
                        offset += vdp.VScroll;
                    }

                    // A tile is 8 pixels, so wrap if gone past
                    offset &= 8;

                    // If flipping vertically then invert offset
                    if (vertFlip)
                    {
                        offset *= -1;
                        offset += 7;
                    }

                    // Each pattern is 32 bytes in memory
                    tileDefinition *= 32;

                    // Each pattern line is 4 bytes. Offset contains correct pattern line
                    tileDefinition += (ushort)(4 * offset);

                    // Get pattern line data
                    var data1 = vdp.VRam[tileDefinition];
                    var data2 = vdp.VRam[(ushort)(tileDefinition + 1)];
                    var data3 = vdp.VRam[(ushort)(tileDefinition + 2)];
                    var data4 = vdp.VRam[(ushort)(tileDefinition + 3)];

                    var colorBit = invert;

                    // If horizontal flip then read from right to left
                    if (horzFlip)
                    {
                        colorBit = x;
                    }

                    // Get which palette for this pattern line
                    var bit = (byte)(data4.HasBit(colorBit) ? 1 : 0);
                    var palette = (byte)(bit << 3);
                    bit = (byte)(data3.HasBit(colorBit) ? 1 : 0);
                    palette |= (byte)(bit << 2);
                    bit = (byte)(data2.HasBit(colorBit) ? 1 : 0);
                    palette |= (byte)(bit << 1);
                    bit = (byte)(data1.HasBit(colorBit) ? 1 : 0);
                    palette |= bit;

                    var masking = false;

                    // Only mask if left column
                    if (xPos < 8 && vdp.MaskFirstColumn)
                    {
                        masking = true;
                        // Palette is set to backgrop color specified in register 7
                        palette = (byte)(vdp.Registers[7] & 15);
                        // Backdrop color uses the sprite palette
                        useSpritePalette = true;
                    }

                    // A tile can only have a high priority if it isn't palette 0
                    // If this doesn't work try changing the if statement to if (palette == (Register[7] & 15)
                    if (palette == 0)
                    {
                        hiPriority = false;
                    }
                    if (useSpritePalette)
                    {
                        palette += 16;
                    }

                    var color = vdp.CRam[palette];

                    // Split color into RGB components
                    var red = (byte)(color & 0x3);
                    color >>= 2;
                    var green = (byte)(color & 0x3);
                    color >>= 2;
                    var blue = (byte)(color & 0x3);

                    // A sprite is drawn here so let's not overwrite it :)
                    if (!masking && !hiPriority && vdp.Screen[xPos, vdp.VCounter] != null)
                    {
                        continue;
                    }

                    // Don't go over edge of screen
                    if (xPos >= TMS9918A.NumResHorizontal)
                    {
                        continue;
                    }

                    vdp.Screen[xPos, vdp.VCounter] = new Color(Color.GetColorShade(red), Color.GetColorShade(green), Color.GetColorShade(blue));
                }

                // Move starting col onto next column after drawing a column
                hStartingCol = (hStartingCol + 1) % 32;
            }
        }
    }
}
