﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace DebugMod
{
    public class CanvasPanel
    {
        private CanvasImage background;
        private GameObject canvas;
        private Vector2 position;
        private Vector2 size;
        private Dictionary<string, CanvasButton> buttons = new Dictionary<string, CanvasButton>();
        private Dictionary<string, CanvasPanel> panels = new Dictionary<string, CanvasPanel>();
        private Dictionary<string, CanvasImage> images = new Dictionary<string, CanvasImage>();
        private Dictionary<string, CanvasText> texts = new Dictionary<string, CanvasText>();

        public bool active;

        public CanvasPanel(GameObject parent, Texture2D tex, Vector2 pos, Vector2 sz, Rect bgSubSection)
        {
            position = pos;
            size = sz;
            canvas = parent;
            background = new CanvasImage(parent, tex, pos, sz, bgSubSection);

            active = true;
        }

        public void AddButton(string name, Texture2D tex, Vector2 pos, Vector2 sz, UnityAction func, Rect bgSubSection, Font font = null, string text = null, int fontSize = 13)
        {
            CanvasButton button = new CanvasButton(canvas, tex, position + pos, size + sz, bgSubSection, font, text, fontSize);
            button.AddClickEvent(func);

            buttons.Add(name, button);
        }

        public void AddPanel(string name, Texture2D tex, Vector2 pos, Vector2 sz, Rect bgSubSection)
        {
            CanvasPanel panel = new CanvasPanel(canvas, tex, position + pos, sz, bgSubSection);

            panels.Add(name, panel);
        }

        public void AddImage(string name, Texture2D tex, Vector2 pos, Vector2 size, Rect subSprite)
        {
            CanvasImage image = new CanvasImage(canvas, tex, position + pos, size, subSprite);

            images.Add(name, image);
        }

        public void AddText(string name, string text, Vector2 pos, Font font, int fontSize = 13)
        {
            CanvasText t = new CanvasText(canvas, position + pos, font, text, fontSize);

            texts.Add(name, t);
        }

        public CanvasButton GetButton(string buttonName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetButton(buttonName);
            }

            if (buttons.ContainsKey(buttonName))
            {
                return buttons[buttonName];
            }

            return null;
        }

        public CanvasImage GetImage(string imageName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetImage(imageName);
            }

            if (images.ContainsKey(imageName))
            {
                return images[imageName];
            }

            return null;
        }

        public CanvasPanel GetPanel(string panelName)
        {
            if (panels.ContainsKey(panelName))
            {
                return panels[panelName];
            }

            return null;
        }

        public CanvasText GetText(string textName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetText(textName);
            }

            if (texts.ContainsKey(textName))
            {
                return texts[textName];
            }

            return null;
        }

        public void SetPosition(Vector2 pos)
        {
            background.SetPosition(pos);

            Vector2 deltaPos = position - pos;
            position = pos;

            foreach (CanvasButton button in buttons.Values)
            {
                button.SetPosition(button.GetPosition() + deltaPos);
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.SetPosition(panel.GetPosition() + deltaPos);
            }
        }

        public void TogglePanel(string name)
        {
            if (active && panels.ContainsKey(name))
            {
                panels[name].ToggleActive();
            }
        }

        public void AddButtonToPanel(string panelName, string buttonName, Texture2D tex, Vector2 pos, Vector2 sz, UnityAction func, Rect bgSubSection, Font font = null, string text = null, int fontSize = 13)
        {
            if (panels.ContainsKey(panelName))
            {
                panels[panelName].AddButton(buttonName, tex, pos, sz, func, bgSubSection, font, text, fontSize);
            }
        }

        public void ToggleActive()
        {
            active = !active;
            SetActive(active, false);
        }

        public void SetActive(bool b, bool panel)
        {
            background.SetActive(b);

            foreach (CanvasButton button in buttons.Values)
            {
                button.SetActive(b);
            }

            foreach (CanvasImage image in images.Values)
            {
                image.SetActive(b);
            }

            foreach (CanvasText t in texts.Values)
            {
                t.SetActive(b);
            }

            if (panel)
            {
                foreach (CanvasPanel p in panels.Values)
                {
                    p.SetActive(b, false);
                }
            }

            active = b;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void FixRenderOrder()
        {
            foreach (CanvasText t in texts.Values)
            {
                t.MoveToTop();
            }

            foreach (CanvasButton button in buttons.Values)
            {
                button.MoveToTop();
            }

            foreach (CanvasImage image in images.Values)
            {
                image.SetRenderIndex(0);
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.FixRenderOrder();
            }

            background.SetRenderIndex(0);
        }
    }
}
