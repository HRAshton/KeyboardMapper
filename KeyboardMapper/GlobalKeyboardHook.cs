/*
 * Thanks Duncan Palmer for this class.
 * https://stackoverflow.com/questions/10567127/global-keyboard-hook-that-doesnt-disable-user-input-outside-of-form
 */
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardMapper
{
    internal class GlobalKeyboardHook
    {
        private delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        private struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        private readonly Keys[] hookedKeys;
        private readonly bool catchAllKeyDowns;

        private IntPtr hhook = IntPtr.Zero;

        private KeyboardHookProc SAFE_delegate_callback;

        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;

        public GlobalKeyboardHook(Keys[] hookedKeys, bool catchAllKeyDowns = false)
        {
            this.hookedKeys = hookedKeys;
            this.catchAllKeyDowns = catchAllKeyDowns;
            Hook();
        }

        ~GlobalKeyboardHook()
        {
            Unhook();
        }

        public void Hook()
        {
            var hInstance = LoadLibrary("User32");
            SAFE_delegate_callback = HookProc;
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, SAFE_delegate_callback, hInstance, 0);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(hhook);
        }

        private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            try
            {
                if (code < 0)
                {
                    return CallNextHookEx(hhook, code, wParam, ref lParam);
                }

                var key = (Keys)lParam.vkCode;
                if (!catchAllKeyDowns && !hookedKeys.Contains(key))
                {
                    return CallNextHookEx(hhook, code, wParam, ref lParam);
                }

                var kea = new KeyEventArgs(key);
                if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && KeyDown != null)
                {
                    KeyDown(this, kea);
                }
                else if (wParam == WM_KEYUP || wParam == WM_SYSKEYUP)
                {
                    KeyUp?.Invoke(this, kea);
                }

                if (kea.Handled)
                {
                    return 1;
                }

                return CallNextHookEx(hhook, code, wParam, ref lParam);
            }
            catch
            {
                return 0;
            }
        }


        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);
    }
}