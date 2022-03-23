﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for ToggleButton.xaml
    /// </summary>
    public partial class ToggleButton : UserControl
    {
        Thickness LeftSide = new Thickness(-39, 0, 0, 0);
        Thickness RightSide = new Thickness(0, 0, -39, 0);
        SolidColorBrush Off = new SolidColorBrush(Color.FromRgb(53, 69, 64));
        SolidColorBrush On = new SolidColorBrush(Color.FromRgb(34, 32, 47));
        private bool Toggled ;

        public ToggleButton()
        {
            
      

            InitializeComponent();
            if (Back.Tag.ToString() == "en-US")
            {

                Back.Fill = Off;
                Toggled = false;
                Dot.Margin = LeftSide;

            } if(Back.Tag.ToString() == "fr-FR")
            {
                    Back.Fill = On;
                    Toggled = true;
                    Dot.Margin = RightSide;
            }

        
        }

        

        public bool Toggled1 { get => Toggled; set => Toggled = value; }

        private void Dot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Toggled)
            {
                Back.Fill = On;
                Toggled = true;
                Dot.Margin = RightSide;

            }
            else 
            {

                Back.Fill = Off;
                Toggled = false;
                Dot.Margin = LeftSide;

            }




        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Toggled)
            {
                Back.Fill = On;
                Toggled = true;
                Dot.Margin = RightSide;

            }
            else
            {

                Back.Fill = Off;
                Toggled = false;
                Dot.Margin = LeftSide;

            }

        }
    }
}
