using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows;


namespace OpenJinglePlayer
{
    public partial class VideoForm : Form
    {
        public VideoForm()
        {
            InitializeComponent();

            MediaPlayer player = new MediaPlayer();

            player.Open(new Uri(@"test.mp4", UriKind.Relative));

            VideoDrawing aVideoDrawing = new VideoDrawing();

            aVideoDrawing.Rect = new Rect(0, 0, 100, 100);
            aVideoDrawing.Player = player;

            DrawingImage di = new DrawingImage(aVideoDrawing);

            // Play the video once.
            player.Play();        
        }


    }
}
