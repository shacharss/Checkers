using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers.GUI
{
    internal class BoardButton : Button
    {
        internal readonly int r_Row;
        internal readonly int r_Col;

        internal BoardButton(int i_Row, int i_Col)
            : base()
        {
            r_Row = i_Row;
            r_Col = i_Col;
        }
    }
}
