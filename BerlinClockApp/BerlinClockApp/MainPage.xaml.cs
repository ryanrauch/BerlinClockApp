﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace BerlinClockApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
	    {
	        SKSurface surface = e.Surface;
	        SKCanvas canvas = surface.Canvas;

	        canvas.Clear(SKColors.AntiqueWhite);


	    }
	}
}
