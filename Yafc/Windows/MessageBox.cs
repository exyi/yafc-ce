﻿using System;
using System.Threading.Tasks;
using Yafc.UI;

namespace Yafc {
    public class MessageBox : PseudoScreen<bool> {
        public MessageBox() : base(30f) { }

        private string title, message, yes, no;

        public static void Show(Action<bool, bool> result, string title, string message, string yes, string no) {
            MessageBox instance = new MessageBox { title = title, complete = result, message = message, yes = yes, no = no };
            _ = MainScreen.Instance.ShowPseudoScreen(instance);
        }

        public static void Show(string title, string message, string yes) {
            Show(null, title, message, yes, null);
        }

        public static Task<(bool haveChoice, bool choice)> Show(string title, string message, string yes, string no) {
            TaskCompletionSource<(bool, bool)> tcs = new TaskCompletionSource<(bool, bool)>();
            Show((a, b) => tcs.TrySetResult((a, b)), title, message, yes, no);
            return tcs.Task;
        }

        public override void Build(ImGui gui) {
            BuildHeader(gui, title);
            if (message != null) {
                gui.BuildText(message, wrap: true);
            }

            gui.AllocateSpacing(2f);
            using (gui.EnterRow(allocator: RectAllocator.RightRow)) {
                if (gui.BuildButton(yes)) {
                    CloseWithResult(true);
                }

                if (no != null && gui.BuildButton(no, SchemeColor.Grey)) {
                    CloseWithResult(false);
                }
            }
        }
    }
}
