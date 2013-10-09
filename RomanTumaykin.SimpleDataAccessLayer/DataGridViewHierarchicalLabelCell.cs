using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RomanTumaykin.SimpleDataAccessLayer
{
    internal class DataGridViewHierarchicalLabelCell : DataGridViewTextBoxCell
    {

        private static void PaintErrorIcon(Graphics graphics, Rectangle iconBounds)
        {
            graphics.DrawLine(new Pen(new SolidBrush(Color.Black)), 5, 0, 5, iconBounds.Height/2);
            graphics.DrawLine(new Pen(new SolidBrush(Color.Black)), 5, iconBounds.Height/2, iconBounds.Width - 2,
                              iconBounds.Height/2);

        }

        protected virtual void PaintErrorIcon(Graphics graphics, Rectangle clipBounds, Rectangle cellValueBounds,
                                              string errorText)
        {
            DataGridViewHierarchicalLabelCell.PaintErrorIcon(graphics, cellValueBounds);
        }
    }
}
