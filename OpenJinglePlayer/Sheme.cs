using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace OpenJinglePlayer
{
    class Sheme
    {
        const int NUMW = 3;
        const int NUMH = 10;

        private int x = 0, y = 0, w = 0, h = 0, space=0;

        private List<Tile> _tiles;
        public String Name;
        
        public Sheme()
        {
            _tiles = new List<Tile>();

            for (int i = 0; i < NUMW * NUMH; i++)
            {
                _tiles.Add(new Tile(i + 1));
            }

            Name = "Unbenannt";
        }

        public bool Load(string path, XPathNavigator navigator)
        {
            _tiles = new List<Tile>();
            CHelper.GetValueFromXML(path + "/Name", navigator, ref Name, "Unbenannt");

            for (int i = 0; i < NUMH * NUMW; i++)
            {
                Tile t = new Tile(i);
                CHelper.GetValueFromXML(path + "/Tile" + (i + 1).ToString() + "/Path", navigator, ref t.FilePath, String.Empty);
                t.SetFile(t.FilePath);
                CHelper.GetValueFromXML(path + "/Tile" + (i + 1).ToString() + "/Name", navigator, ref t.Name, t.Name);
                _tiles.Add(t);
            }
            return true;
        }

        public bool Save(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
            for (int i = 0; i < _tiles.Count; i++)
            {
                writer.WriteStartElement("Tile" + (i + 1).ToString());
                writer.WriteElementString("Path", _tiles[i].FilePath);
                writer.WriteElementString("Name", _tiles[i].Name);
                writer.WriteEndElement();
            }
            return true;
        }

        public Tile GetTile(int mx, int my)
        {
            int tw = (w - NUMW * space) / NUMW;
            int th = (h - NUMH * space) / NUMH;

            for (int i = 0; i < NUMW; i++)
            {
                for (int j = 0; j < NUMH; j++)
                {
                    if (_tiles[i * NUMH + j].isMouseOver(mx, my))
                        return _tiles[i * NUMH + j];
                }
            }
            return null;
        }

        public Tile GetActiveTile()
        {
            for (int i = 0; i < NUMW; i++)
            {
                for (int j = 0; j < NUMH; j++)
                {
                    if (_tiles[i * NUMH + j].Status == State.Paused || _tiles[i * NUMH + j].Status == State.Playing)
                        return _tiles[i * NUMH + j];
                }
            }
            return null;
        }

        public Tile GetNextNotOpenedTile()
        {
            for (int i = 0; i < NUMW; i++)
            {
                for (int j = 0; j < NUMH; j++)
                {
                    if (_tiles[i * NUMH + j].WaitForOpen)
                        return _tiles[i * NUMH + j];
                }
            }
            return null;
        }

        public void StopAll(Tile NotThisTile = null)
        {
            foreach (Tile tile in _tiles)
            {
                if (tile != NotThisTile)
                    tile.Stop();
            }
        }

        public void PauseAll()
        {
            foreach (Tile tile in _tiles)
            {
                if (tile.Status == State.Playing)
                    tile.Pause();
            }
        }

        public void Draw(Graphics g, int x, int y, int w, int h, int space, int mx, int my)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;

            int tw = (w - NUMW * space) / NUMW;
            int th = (h - NUMH * space) / NUMH;

            for (int i = 0; i < NUMW; i++)
            {
                for (int j = 0; j < NUMH; j++)
                {
                    _tiles[i * NUMH + j].Draw(g, x + tw * i + space * i, y + th * j + j * space, tw, th, mx, my);
                }
            }
        }
    }
}
