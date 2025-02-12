using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using System.Collections.Generic;
using System.Text;

namespace NonoSharp;

public static class TextRenderer
{
    /// <summary>
    /// Dictionary of loaded fonts
    /// </summary>
    private static Dictionary<string, SpriteFont> _fonts;

    /// <summary>
    /// Load a font
    /// </summary>
    /// <param name="fontName">Font name (key in dictionary)</param>
    /// <param name="fontAsset">The sprite font asset to use</param>
    /// <param name="content">Content manager</param>
    public static void LoadFont(string fontName, string fontAsset, ContentManager content)
    {
        _fonts ??= new();
        Log.Logger.Information($"Loading font \"{fontAsset}\" as \"{fontName}\"");
        _fonts[fontName] = content.Load<SpriteFont>(fontAsset);
    }

    /// <summary>
    /// Draw some text
    /// </summary>
    /// <param name="batch">Sprite batch to use</param>
    /// <param name="font">Name of the font to use</param>
    /// <param name="x">X position of the text</param>
    /// <param name="y">Y position of the text</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">The color of the text</param>
    public static void DrawText(SpriteBatch batch, string font, int x, int y, string text, Color color)
    {
        batch.DrawString(_fonts[font], text, new Vector2((float)x, (float)y), color);
    }

    /// <summary>
    /// Draw some text (with scale)
    /// </summary>
    /// <param name="batch">Sprite batch to use</param>
    /// <param name="font">Name of the font to use</param>
    /// <param name="x">X position of the text</param>
    /// <param name="y">Y position of the text</param>
    /// <param name="scale">Text scale</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">The color of the text</param>
    public static void DrawText(SpriteBatch batch, string font, int x, int y, float scale, string text, Color color)
    {
        batch.DrawString(_fonts[font], text, new Vector2((float)x, (float)y), color, 0, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
    }

    /// <summary>
    /// Draw some centered text
    /// </summary>
    /// <param name="batch">Sprite batch to use</param>
    /// <param name="font">Name of the font to use</param>
    /// <param name="scale">Text scale</param>
    /// <param name="text">Text to draw</param>
    /// <param name="color">The color of the text</param>
    /// <param name="rect">Rectangle to center the text in</param>
    public static void DrawTextCenter(SpriteBatch batch, string font, float scale, string text, Color color, Rectangle rect)
    {
        Vector2 textSize = _fonts[font].MeasureString(text);
        float centerX = rect.X + ((rect.Width - (textSize.X * scale)) / 2);
        float centerY = rect.Y + ((rect.Height - (textSize.Y * scale)) / 2);
        batch.DrawString(_fonts[font], text, new Vector2(centerX, centerY), color, 0, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
    }

    public static void DrawTextWrapped(SpriteBatch batch, string font, int x, int y, float scale, string text, float maxWidth, Color color)
    {
        batch.DrawString(_fonts[font], wrapText(_fonts[font], text, maxWidth, scale), new(x, y), color, 0, Vector2.Zero, scale, SpriteEffects.None, 1.0f);
    }

    /// <summary>
    /// Measure the size of text
    /// </summary>
    /// <param name="font">Font name</param>
    /// <param name="text">Text to measure</param>
    /// <returns>The size of the text</returns>
    public static Vector2 MeasureString(string font, string text)
    {
        return _fonts[font].MeasureString(text);
    }

    private static string wrapText(SpriteFont spriteFont, string text, float maxLineWidth, float scale)
    {
        // i stole this code from stackoverflow
        string[] words = text.Split(' ');
        StringBuilder sb = new();
        float lineWidth = 0f;
        float spaceWidth = spriteFont.MeasureString(" ").X;

        foreach (string word in words)
        {
            Vector2 size = spriteFont.MeasureString(word) * scale;

            if (lineWidth + size.X < maxLineWidth)
            {
                sb.Append(word + " ");
                lineWidth += size.X + spaceWidth;
            }
            else
            {
                sb.Append("\n" + word + " ");
                lineWidth = size.X + spaceWidth;
            }
        }

        return sb.ToString();
    }
}
