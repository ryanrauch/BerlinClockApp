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
	    private static SKColor backgroundColor = SKColors.Black;
        private SKPaint maskFillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Black.WithAlpha(160),
            IsAntialias = true
            
        };
        private SKPaint backgroundStrokePaint = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            Color = backgroundColor,
            StrokeWidth = 1
        };
        private SKPaint redFillPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Red
        };
        private SKPaint yellowFillPaint = new SKPaint()
	    {
            Style = SKPaintStyle.Fill,
            Color =  SKColors.Yellow
	    };

	    public MainPage()
		{
			InitializeComponent();
		}

	    private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
	    {
	        SKSurface surface = e.Surface;
	        SKCanvas canvas = surface.Canvas;

	        canvas.Clear(backgroundColor);

	        int width = e.Info.Width;
	        int height = e.Info.Height;
	        bool landscape = width > height;

            // Construct elements within a defined size of 88x100
            // Boundaries chosen due to divisibility with 11 and 4, and slightly smaller than 100
            const int scalarWidth = 88;
	        const int scalarHeight = 100;
	        const int sectionQuadWidth = scalarWidth / 4;
	        const int sectionElevenWidth = scalarWidth / 11;
	        const int sectionHeight = scalarHeight / 5;

	        DateTime currentTime = DateTime.Now;

            //Scale the canvas to fit properly
	        if (landscape)
	            canvas.Scale(height / scalarHeight);
	        else
	            canvas.Scale(width / scalarWidth);

            // Keep track of y-axis value used for each row
            int currentY = sectionHeight / 2;

            // Circle is only lit on even-numbered seconds.
            #region 1-Second Circle
            canvas.DrawCircle(scalarWidth / 2, currentY, currentY, yellowFillPaint);
	        if (currentTime.Second % 2 == 1) 
	            canvas.DrawCircle(scalarWidth / 2, currentY, currentY, maskFillPaint);
	        canvas.DrawCircle(scalarWidth / 2, currentY, currentY, backgroundStrokePaint);
            #endregion

            // Each lit block signifies 5 hrs since midnight 
	        // ie: (0:00, 5:00, 10:00, 15:00, 20:00)
            #region 5-Hour Blocks
            currentY = sectionHeight;
	        for (int i = 0; i < 4; ++i)
	        {
	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, redFillPaint);
                if(currentTime.Hour < (i+1)*5)
    	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, maskFillPaint);
	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, backgroundStrokePaint);
	        }
            #endregion

            // Each lit block signifies 1 hr since last 5 hr block
            // ie: 02:00 would have 2 blocks lit, and 05:00 would have none lit
	        #region 1-Hour Blocks
	        currentY += sectionHeight;
	        for (int i = 0; i < 4; ++i)
	        {
	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, redFillPaint);
	            if (currentTime.Hour % 5 < i + 1)
	                canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, maskFillPaint);
	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, backgroundStrokePaint);
	        }
            #endregion

            // Each lit block signifies 5 minutes into hour
            // ie: all-off would be :00
            // and 3 lit would be :15
            // Red blocks are at :15, :30 and :45 minutes
	        #region 5-Minute Blocks
	        currentY += sectionHeight;
	        for (int i = 0; i < 11; ++i)
	        {
                if((i+1) % 3 == 0)
    	            canvas.DrawRect(i * sectionElevenWidth, currentY, sectionElevenWidth, sectionHeight, redFillPaint);
                else
	                canvas.DrawRect(i * sectionElevenWidth, currentY, sectionElevenWidth, sectionHeight, yellowFillPaint);
	            if (currentTime.Minute < (i + 1) * 5)
	                canvas.DrawRect(i * sectionElevenWidth, currentY, sectionElevenWidth, sectionHeight, maskFillPaint);
	            canvas.DrawRect(i * sectionElevenWidth, currentY, sectionElevenWidth, sectionHeight, backgroundStrokePaint);
	        }
            #endregion

            // Each block is 1 minute since above 5 minute block was lit
            // ie: 00:15 would not light any blocks
            // 00:16 would light first block, and 00:17 would light first two blocks
	        #region 1-Minute Blocks
	        currentY += sectionHeight;
	        for (int i = 0; i < 4; ++i)
	        {
	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, yellowFillPaint);
	            if (currentTime.Minute % 5 < i + 1)
	                canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, maskFillPaint);
	            canvas.DrawRect(i * sectionQuadWidth, currentY, sectionQuadWidth, sectionHeight, backgroundStrokePaint);
	        }
	        #endregion
        }
    }
}
