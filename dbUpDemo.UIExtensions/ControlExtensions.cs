using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbUpDemo.UIExtensions
{
    public static class ControlExtensions
    {
        public static void Aguarde(this Control control, bool wait = true)
        {
            control.UseWaitCursor = wait;
            bool enable = !wait;

            ChangeControlEnable(control, enable);
        }

        private static void ChangeControlEnable(Control pai, bool enable)
        {
            foreach(Control control in pai.Controls)
            {
                ChangeEnable(control, enable);

                if (control.HasChildren)
                {
                    ChangeControlEnable(control, enable);
                }
            }
        }


        private static void ChangeEnable(Control control, bool enable)
        {
            if(control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(delegate ()
                {
                    control.Enabled = enable;
                }));
            }
            else
            {
                control.Enabled = enable;
            }
        }
    }
}
