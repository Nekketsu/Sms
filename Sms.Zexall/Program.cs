using Sms;
using Sms.Zexall;

var data = await File.ReadAllBytesAsync("Roms/zexall.sms");
var mapper = new CpmMapper(data);

var z80 = new Z80(mapper);
var keyboardJoypad = new KeyboardJoyPad();
z80.Ports.MapPorts(keyboardJoypad);

var vpd = new TMS9918A();
z80.Ports.MapPorts(vpd);

var nullPorts = new NullPorts();
z80.Ports.MapPorts(nullPorts);

//z80.Registers.PC = mapper.StartingAddress;

while (true)
{
    if (z80.Registers.PC == 5)
    {
        if (z80.Registers.C == 2)
        {
            Console.Write((char)z80.Registers.E);
        }
        else if (z80.Registers.C == 9)
        {
            var i = z80.Registers.DE;
            var c = (char)z80.Memory[i];
            while (c != '$')
            {
                Console.Write(c);
                i++;
                c = (char)z80.Memory[i];
            }
        }
    }
    else if (z80.Registers.PC == 7 && z80.Memory[z80.Registers.PC] == 0xc9)
    {
        Console.WriteLine("Patch: Return from syscall");
    }

     z80.ExecuteNextInstruction();

    keyboardJoypad.Update();
}