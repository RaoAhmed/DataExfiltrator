using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;
using System.Linq;



namespace WorkingWithHID_ConsoleApp
{
    internal class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
        [STAThread]
        static void Main(string[] args)
        {
            //String file=@"C:\Users\test\Desktop\ams.png";
            String file="";
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                Title = "Select a File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog.FileName;
                Console.WriteLine("Selected File: " + file);
            }
            else
            {
                Console.WriteLine("No file selected.");
                return;
            }
            //String file = $@"{args[0]}";
            //string file = ;
            //string outputFile = "";
            //if (args.Length == 2)
            //{
            //    outputFile = $@"{args[1]}";
            //}
            //else
            //{
            //    outputFile = $@"{args[0]}_base64";
            //}
            //String data = File.ReadAllText(file); // read all text (file contain binary data) from target.txt
            var sim = new InputSimulator();

            DateTime start;
            DateTime end;
            start = DateTime.Now;
            byte[] byteArray = File.ReadAllBytes(file);
            string data = string.Join("", byteArray.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
            //Console.WriteLine(data);
            //Console.WriteLine("length of file: {0}", fileBytes.Length);
            //sim.Keyboard.KeyPress(VirtualKeyCode.SCROLL);

            if (data[0] == '1')
            { SetKey(VirtualKeyCode.NUMLOCK); Console.Write(1); }
            else
            { UnSetKey(VirtualKeyCode.NUMLOCK); Console.Write(0); }

            if (data[1] == '1')
            { SetKey(VirtualKeyCode.CAPITAL); Console.Write(1); }
            else
            { UnSetKey(VirtualKeyCode.CAPITAL); Console.Write(0); }

            if (!ScrollLockStatus()) // turn the scroll lock on if it is off
            {
                sim.Keyboard.KeyPress(VirtualKeyCode.SCROLL); // turn scroll lock on
                //Console.WriteLine("scroll lock is truned on");
            }

            for (int i = 2; i < data.Length; i+=2)
            {
                while (ScrollLockStatus()) ; // keep waiting while Scroll Lock is on
                //Thread.Sleep(20);
                //sim.Keyboard.KeyPress(VirtualKeyCode.SCROLL);
                //start = DateTime.Now;
                if (data[i] == '1')
                { SetKey(VirtualKeyCode.NUMLOCK); Console.Write(1); }
                else
                { UnSetKey(VirtualKeyCode.NUMLOCK); Console.Write(0); }

                try
                {
                    if (data[i + 1] == '1')
                    { SetKey(VirtualKeyCode.CAPITAL); Console.Write(1); }
                    else
                    { UnSetKey(VirtualKeyCode.CAPITAL); Console.Write(0); }
                }
                catch
                {
                    continue;
                }

                if (!ScrollLockStatus()) // turn the scroll lock on if it is off
                {
                    sim.Keyboard.KeyPress(VirtualKeyCode.SCROLL); // turn scroll lock on
                    //Console.WriteLine("turn scroll lock off");
                }

                //Thread.Sleep(20);
            }
            Console.WriteLine(start-DateTime.Now);

            //sim.Keyboard.KeyPress(VirtualKeyCode.CAPITAL); // Toggle CAPS LOCK
            //Console.WriteLine(Console.CapsLock); // Check CAPS LOCK status
            //Console.WriteLine(VirtualKeyCode.CAPITAL.GetType()); // type of VirtualKeyCode.Capital is WindowsInput.Native.VirtualKeyCode

            //sim.Keyboard.KeyPress(VirtualKeyCode.NUMLOCK); // Toggle NUM LOCK
            //Console.WriteLine(Console.NumberLock); // Check NUM LOCK status
            //Console.WriteLine(VirtualKeyCode.NUMLOCK.GetType());


        }

        private static void UnSetKey(VirtualKeyCode key)
        {
            var sim = new InputSimulator();
            if ((key == VirtualKeyCode.NUMLOCK) && (Console.NumberLock))
                sim.Keyboard.KeyPress(VirtualKeyCode.NUMLOCK);

            else if ((key == VirtualKeyCode.CAPITAL) && (Console.CapsLock))
                sim.Keyboard.KeyPress(VirtualKeyCode.CAPITAL);
        }

        private static void SetKey(VirtualKeyCode key)
        {
            var sim = new InputSimulator();
            if ((key == VirtualKeyCode.NUMLOCK) && !(Console.NumberLock))
                sim.Keyboard.KeyPress(VirtualKeyCode.NUMLOCK);

            else if ((key == VirtualKeyCode.CAPITAL) && !(Console.CapsLock))
                sim.Keyboard.KeyPress(VirtualKeyCode.CAPITAL);

        }

        public static bool ScrollLockStatus()
        {
            return (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
        }

        private static bool ScrollChanged(bool previous, bool current)
        {
            //Console.WriteLine((previous ^ current) ? true : false);
            return (previous ^ current) ? true : false;
        }
    }
}
