﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Field_Location_SampleProject
{
    public partial class View : Form
    {
        BG_Field field;
        Graphics g;
        Pen pen;
        SolidBrush b;

        Point oldPos;
        Point pos;

        public View()
        {
            InitializeComponent();
        }

        private void View_Load(object sender, EventArgs e)
        {
            g = panelDraw.CreateGraphics();
            b = new SolidBrush(Color.Red);
            pen = new Pen(Color.Green);
            pen.Width = 2;

            oldPos = new Point();
            pos = new Point();

            DrawField();
        }

        public void DrawField()
        {
            field = new BG_Field(panelDraw.Width, panelDraw.Height);

        }

        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {

            foreach (var l in field.Locations)
            {
                g.FillRectangle(b, l.PosX - 2, l.PosY - 2, 4, 4);
            }

            for (int i = field.Locations.Count() - 1; i > 0; i--)
            {
                oldPos.X = field.Locations[i].PosX;
                oldPos.Y = field.Locations[i].PosY;

                pos.X = field.Locations[i - 1].PosX;
                pos.Y = field.Locations[i - 1].PosY;

                g.DrawLine(pen, oldPos, pos);
            }
        }
    }
}
