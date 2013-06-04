using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace OpenJinglePlayer
{
    class Shemes
    {
        const int VERSION = 1;

        const int MAXNUM = 10;

        private List<Sheme> sheme;
        private int currentSheme;

        private int x = 0, y = 0, w = 0, h = 0, space = 0;

        public string FileName;

        public string[] ShemeList
        {
            get
            {
                if (sheme.Count == 0)
                    return null;

                List<string> result = new List<string>();

                foreach (Sheme s in sheme)
                {
                    result.Add(s.Name);
                }
                return result.ToArray();
            }
        }

        public Shemes()
        {
            sheme = new List<Sheme>();
            sheme.Add(new Sheme());
            sheme[0].Name = "Datensatz 1";
            currentSheme = 0;
            FileName = String.Empty;
        }

        public bool Load(string FilePath)
        {
            bool loaded = false;
            try
            {
                XPathDocument xPathDoc = null;
                XPathNavigator navigator = null;

                bool opened = false;
                try
                {
                    xPathDoc = new XPathDocument(FilePath);
                    navigator = xPathDoc.CreateNavigator();
                    opened = true;
                }
                catch (Exception)
                {
                    loaded = false;
                    if (navigator != null)
                        navigator = null;

                    if (xPathDoc != null)
                        xPathDoc = null;

                    //CLog.LogError("Error loading theme file " + file + ": " + e.Message);
                }

                if (opened)
                {
                    sheme.Clear();

                    List<string> shemes = CHelper.GetValuesFromXML("Shemes", navigator);

                    foreach (string s in shemes)
                    {
                        Sheme sh = new Sheme();
                        loaded &= sh.Load("//root/Shemes/" + s, navigator);
                        sheme.Add(sh);
                    }

                    FileName = FilePath;
                    currentSheme = 0;
                    loaded = true;
                }
   
            }
            catch (Exception)
            {
                ;
            }

            return loaded;
        }

        public bool Save()
        {
            bool saved = false;
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.ConformanceLevel = ConformanceLevel.Document;

                string file = Path.Combine(FileName);
                XmlWriter writer = XmlWriter.Create(file, settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("root");

                //setting
                writer.WriteStartElement("Settings");
                writer.WriteElementString("FileVersion", VERSION.ToString());
                writer.WriteEndElement();

                //shemes
                writer.WriteStartElement("Shemes");
                for (int i = 0; i < sheme.Count; i++)
                {
                    writer.WriteStartElement("Sheme" + (i + 1).ToString());
                    sheme[i].Save(writer);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                // End of File
                writer.WriteEndElement(); //end of root
                writer.WriteEndDocument();

                writer.Flush();
                writer.Close();

                saved = true;
            }
            catch (Exception)
            {
                ;
            }


            return saved;
        }

        public bool SaveAs(string FilePath)
        {
            FileName = FilePath;
            return Save();
        }

        public void Select(int Index)
        {
            if (Index < 0 || Index >= sheme.Count)
                return;

            currentSheme = Index;
        }

        public int GetSelection()
        {
            return currentSheme;
        }

        public void DeleteSheme()
        {
            if (sheme.Count == 0)
                return;

            sheme.RemoveAt(currentSheme);
            currentSheme = sheme.Count - 1;
        }

        public void AddSheme()
        {
            if (sheme.Count >= MAXNUM)
                return;

            sheme.Add(new Sheme());
            sheme[sheme.Count-1].Name = "Datensatz " + sheme.Count.ToString();
            currentSheme = sheme.Count - 1;
        }

        public string GetCurrentShemeName()
        {
            if (sheme.Count == 0)
                return null;

            return sheme[currentSheme].Name;
        }

        public void SetCurrentShemeName(string NewName)
        {
            if (sheme.Count == 0)
                return;

            sheme[currentSheme].Name = NewName;
        }

        public Tile GetTile(int MouseX, int MouseY)
        {
            if (sheme.Count == 0)
                return null;

            return sheme[currentSheme].GetTile(MouseX, MouseY);
        }

        public Tile GetActiveTile()
        {
            if (sheme.Count == 0)
                return null;

            return sheme[currentSheme].GetActiveTile();
        }

        public int MouseClick(int MouseX, int MouseY)
        {
            if (sheme.Count == 0)
                return -1;

            int tw = w;
            int th = (h - MAXNUM * space) / MAXNUM;

            for (int i = 0; i < sheme.Count; i++)
            {
                int iy = y + th * i + i * space;

                if (MouseX >= x && MouseX <= x + tw && MouseY >= iy && MouseY <= iy + th)
                {
                    return i;
                }
            }

            return -1;
        }

        public void StopAll(Tile NotThisTile = null)
        {
            if (sheme.Count == 0)
                return;

            sheme[currentSheme].StopAll(NotThisTile);
        }

        public void PauseAll()
        {
            if (sheme.Count == 0)
                return;

            sheme[currentSheme].PauseAll();
        }

        public void Draw(Graphics g, int x, int y, int w, int h, int space, int mx, int my)
        {
            if (sheme.Count == 0)
                return;

            sheme[currentSheme].Draw(g, x, y, w, h, space,  mx, my);
        }

        public void DrawList(Graphics g, int x, int y, int w, int h, int space, int mx, int my)
        {
            if (sheme.Count == 0)
                return;

            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.space = space;

            int tw = w;
            int th = (h - MAXNUM * space) / MAXNUM;

            for (int i = 0; i < sheme.Count; i++)
            {
                Brush brush = Brushes.Gray;

                if (currentSheme == i)
                    brush = Brushes.LightGray;

                int iy = y + th * i + i * space;
                g.FillRectangle(brush, x, iy, tw, th);

                if (mx >= x && mx <= x + tw && my >= iy && my <= iy + th)
                {
                    g.DrawRectangle(new Pen(Brushes.Blue, 3), new Rectangle(x, iy, tw, th));
                }

                g.DrawString(sheme[i].Name,
                    new Font(FontFamily.GenericSansSerif, th * 0.5f, FontStyle.Regular, GraphicsUnit.Pixel, 0, false),
                    Brushes.Black,
                    x + 5,
                    iy + 0.2f * th);
            }
        }


    }
}
