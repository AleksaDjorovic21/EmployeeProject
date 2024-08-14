using EmployeeProject.Core.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EmployeeProject.Application.Interface;

namespace EmployeeProject.Application.Services;

public class ChartGenerator : IChartGenerator
{
    public void GeneratePieChart(List<AggregatedEmployeeInfo> data, string filePath)
    {
        try
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            int width = 1200;
            int height = 800;

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

            var totalHours = data.Sum(e => e.TotalTimeWorkedInHours);
            if (totalHours == 0) totalHours = 1;

            float startAngle = 0f;
            var colors = GenerateUniqueColors(data.Count);
            int colorIndex = 0;
            float radius = 200;
            float centerX = width / 2;
            float centerY = height / 2;

            // Draw pie chart slices
            foreach (var employee in data)
            {

                // Calculate the sweep angle for the pie slice
                float sweepAngle = (float) (employee.TotalTimeWorkedInHours / totalHours * 360);

                using (var paint = new SKPaint { Style = SKPaintStyle.Fill, Color = colors[colorIndex % colors.Length], IsAntialias = true })
                {
                    canvas.DrawArc(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius), startAngle, sweepAngle, true, paint);
                }

                // Calculate the midpoint angle for text placement
                float middleAngle = startAngle + (sweepAngle / 2);
                float radians = middleAngle * (float) Math.PI / 180;

                // Adjust position for the text to be within the slice
                float textX = centerX + (radius * 0.6f) * (float) Math.Cos(radians);
                float textY = centerY + (radius * 0.6f) * (float) Math.Sin(radians);

                // Draw percentage text
                using (var textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 14,
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                })
                {
                    var percentage = employee.TotalTimeWorkedInHours / totalHours * 100;
                    canvas.DrawText($"{percentage:F1}%", textX, textY, textPaint);
                }

                // Update for the next slice
                startAngle += sweepAngle;
                colorIndex++;
            }

            // Draw legend on the right side
            float legendStartX = width / 2 + radius + 50;
            float legendStartY = height / 2 - (data.Count * (20 + 10)) / 2;
            float legendBoxSize = 20;
            float spacing = 10;
            float itemHeight = legendBoxSize + spacing;

            float currentX = legendStartX;
            float currentY = legendStartY;

            colorIndex = 0;
            foreach (var employee in data)
            {
                // Color box
                using (var paint = new SKPaint { Style = SKPaintStyle.Fill, Color = colors[colorIndex % colors.Length], IsAntialias = true })
                {
                    canvas.DrawRect(currentX, currentY, legendBoxSize, legendBoxSize, paint);
                }

                // Employee name and percentage
                using (var textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 20,
                    IsAntialias = true
                })
                {
                    var percentage = employee.TotalTimeWorkedInHours / totalHours * 100;
                    var name = $"{employee.EmployeeName} ({percentage:F1}%)";
                    canvas.DrawText(name, currentX + legendBoxSize + spacing, currentY + legendBoxSize / 2 + 5, textPaint);
                }

                // Update position for the next item
                currentY += itemHeight;
                colorIndex++;
            }

            using var image = surface.Snapshot();
            using var dataImage = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(filePath);
            dataImage.SaveTo(stream);
            Console.WriteLine($"File saved to: {filePath}");
        }
    catch (Exception ex)
    {
        Console.WriteLine($"Error generating pie chart: {ex.Message}");
        throw;
    }
}

    private static SKColor[] GenerateUniqueColors(int count)
    {
        var colors = new SKColor[count];
        var random = new Random();
        var colorSet = new HashSet<SKColor>();

        while (colorSet.Count < count)
        {
            var color = new SKColor((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
            colorSet.Add(color);
        }

        return colorSet.ToArray();
    }
}

