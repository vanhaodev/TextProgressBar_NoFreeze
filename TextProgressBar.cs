using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace WindowsFormsApp1.module
{
    //Author https://github.com/ukushu/TextProgressBar
    //Edited & Refactor by Nguyen Van Hao
    //
    public class TextProgressBar : ProgressBar
    {
        [Description("Font of the text on ProgressBar"), Category("Additional Options")]
        public Font TextFont { get; set; } = new Font(FontFamily.GenericSansSerif, 11);

        private SolidBrush _textColourBrush = (SolidBrush)Brushes.Black;
        [Category("Additional Options")]
        public Color TextColor
        {
            get
            {
                return _textColourBrush.Color;
            }
            set
            {
                _textColourBrush.Dispose();
                _textColourBrush = new SolidBrush(value);
            }
        }

        private string _text = "";

        [Description("Nếu rỗng thì khỏi hiển thị, khi nào chạy mới hiện phần value bên trong source"), Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string CustomText
        {
            get
            {
                return _text;
            }
            set
            {              
                _text = value;               
                Invalidate();//redraw component after change value from VS Properties section
            }
        }

        private string _textToDraw
        {
            get
            {
                string text = CustomText;

                text = $"{CustomText} {_percentageStr}";
               

                return text;
            }
            set { }
        }

        private string _percentageStr 
        { 
            get 
            { 
                if ((int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100 < 1)
                {
                    return ""; //trả về text rỗng thay vì 0%
                }    
                return $"{(int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100 } %"; 
            } 
        }

        //string kq = diem >= 8 ? "Giỏi" : diem >= 6.5 ? "Khá" : diem >= 5 ? "Trung bình" : "Kém";

        private string _currProgressStr
        {
            get
            {               
                return $"{Value}/{Maximum}";
            }
        }

        public TextProgressBar()
        {
            Value = Minimum;
            FixComponentBlinking();
        }

        private void FixComponentBlinking()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawStringIfNeeded(g);
        }

        private void DrawStringIfNeeded(Graphics g)
        {
            string text = _textToDraw;

            SizeF len = g.MeasureString(text, TextFont);

            Point location = new Point(((Width / 2) - (int)len.Width / 2), ((Height / 2) - (int)len.Height / 2));

            g.DrawString(text, TextFont, (Brush)_textColourBrush, location);
        }

        public new void Dispose()
        {
            _textColourBrush.Dispose();
           
            base.Dispose();
        }
    }
}
