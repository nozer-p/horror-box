using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotForgeStudio.HorrorBox
{
    public class UIManager : IService, IUIManager
    {
        private List<IUIElement> _uiPages;
        private List<IUIPopup> _uiPopups;

        public IUIElement CurrentPage { get; private set; }

        public GameObject Canvas { get; private set; }

        public void Init()
        {
            Canvas = GameObject.Find("Canvas");

            _uiPages = new List<IUIElement>();
            _uiPages.Add(new MainPage());
            _uiPages.Add(new GamePage());
            
            foreach (var page in _uiPages)
                page.Init();

            _uiPopups = new List<IUIPopup>();
            _uiPopups.Add(new SettingsPopup());
            _uiPopups.Add(new PausePopup());
            _uiPopups.Add(new ResultsPopup());

            foreach (var popup in _uiPopups)
                popup.Init();
        }

        public void Dispose()
        {
            foreach (var page in _uiPages)
                page.Dispose();

            foreach (var popup in _uiPopups)
                popup.Dispose();
        }

        public void Update()
        {
            foreach (var page in _uiPages)
                page.Update();

            foreach (var popup in _uiPopups)
                popup.Update();
        }

        public void HideAllPages()
        {
            foreach (var _page in _uiPages)
                _page.Hide();
        }

        public void HideAllPopups()
        {
            foreach (var _popup in _uiPopups)
                _popup.Hide();
        }

        public void SetPage<T>(bool hideAll = false) where T : IUIElement
        {
            IUIElement previousPage = null;

            if (hideAll)
            {
                HideAllPages();
            }
            else
            {
                if (CurrentPage != null)
                {
                    CurrentPage.Hide();
                    previousPage = CurrentPage;
                }
            }

            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    CurrentPage = _page;
                    break;
                }
            }
            CurrentPage.Show();
        }

        public void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }

            if (setMainPriority)
                popup.SetMainPriority();

            if (message == null)
                popup.Show();
            else
                popup.Show(message);
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    _popup.Hide();
                    break;
                }
            }
        }

        public T GetPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = null;
            foreach (var _popup in _uiPopups)
            {
                if (_popup is T)
                {
                    popup = _popup;
                    break;
                }
            }
            return (T)popup;
        }

        public T GetPage<T>() where T : IUIElement
        {
            IUIElement page = null;
            foreach (var _page in _uiPages)
            {
                if (_page is T)
                {
                    page = _page;
                    break;
                }
            }
            return (T)page;
        }
    }
}