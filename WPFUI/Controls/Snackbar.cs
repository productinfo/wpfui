﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls
{
    /// <summary>
    /// Small card with buttons displayed at the bottom for a short time.
    /// </summary>
    public class Snackbar : System.Windows.Controls.ContentControl, IIconElement
    {
        private readonly EventIdentifier _identifier = new();

        /// <summary>
        /// Property for <see cref="Show"/>.
        /// </summary>
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register("Show",
            typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Timeout"/>.
        /// </summary>
        public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register("Timeout",
            typeof(int), typeof(Snackbar), new PropertyMetadata(2000));

        /// <summary>
        /// Property for <see cref="Icon"/>.
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(Common.Icon), typeof(Snackbar),
            new PropertyMetadata(Common.Icon.Empty));

        /// <summary>
        /// Property for <see cref="IconFilled"/>.
        /// </summary>
        public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
            typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

        /// <summary>
        /// Property for <see cref="Title"/>.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string), typeof(Snackbar), new PropertyMetadata(""));

        /// <summary>
        /// Property for <see cref="Message"/>.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message",
            typeof(string), typeof(Snackbar), new PropertyMetadata(""));

        // TODO: Remove
        /// <summary>
        /// Property for <see cref="SlideTransform"/>.
        /// </summary>
        public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register("SlideTransform",
            typeof(TranslateTransform), typeof(Snackbar), new PropertyMetadata(new TranslateTransform()));

        /// <summary>
        /// Property for <see cref="ButtonCloseCommand"/>.
        /// </summary>
        public static readonly DependencyProperty ButtonCloseCommandProperty =
            DependencyProperty.Register("ButtonCloseCommand",
                typeof(Common.IRelayCommand), typeof(Snackbar), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets information whether the snackbar should be displayed.
        /// </summary>
        public bool Show
        {
            get => (bool)GetValue(ShowProperty);
            set => SetValue(ShowProperty, value);
        }

        /// <summary>
        /// Time for which the snackbar is to be displayed.
        /// </summary>
        public int Timeout
        {
            get => (int)GetValue(TimeoutProperty);
            set => SetValue(TimeoutProperty, value);
        }

        /// <inheritdoc />
        public Common.Icon Icon
        {
            get => (Common.Icon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <inheritdoc />
        public bool IconFilled
        {
            get => (bool)GetValue(IconFilledProperty);
            set => SetValue(IconFilledProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the top of the snackbar.
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the bottom of the snackbar.
        /// </summary>
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the bottom of the snackbar.
        /// </summary>
        public TranslateTransform SlideTransform
        {
            get => (TranslateTransform)GetValue(SlideTransformProperty);
            set => SetValue(SlideTransformProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="Common.RelayCommand"/> triggered after clicking close button.
        /// </summary>
        public Common.IRelayCommand ButtonCloseCommand => (Common.IRelayCommand)GetValue(ButtonCloseCommandProperty);

        /// <summary>
        /// Creates new instance and sets default <see cref="ButtonCloseCommand"/>.
        /// </summary>
        public Snackbar()
        {
            SetValue(ButtonCloseCommandProperty, new Common.RelayCommand(o => Hide()));
        }

        /// <summary>
        /// Shows the snackbar for the amount of time specified in <see cref="Timeout"/>.
        /// </summary>
        public void Expand()
        {
            ShowComponent();
        }

        /// <summary>
        /// Sets <see cref="Title"/> and <see cref="Message"/>, then shows the snackbar for the amount of time specified in <see cref="Timeout"/>.
        /// </summary>
        public async void Expand(string title, string message)
        {
            if (Show)
            {
                HideComponent();

                await Task.Delay(300);

                if (Application.Current == null)
                    return;

                Title = title;
                Message = message;

                ShowComponent();

                return;
            }

            Title = title;
            Message = message;

            ShowComponent();
        }

        /// <summary>
        /// Hides <see cref="Snackbar"/>.
        /// </summary>
        public void Hide()
        {
            HideComponent(0);
        }

        private void ShowComponent()
        {
            if (Show) return;

            Show = true;

            if (Timeout > 0)
                HideComponent(Timeout);
        }

        private async void HideComponent(int timeout = 0)
        {
            if (!Show) return;

            if (timeout < 1)
                Show = false;

            uint currentEvent = _identifier.GetNext();

            await Task.Delay(timeout);

            if (Application.Current == null)
                return;

            if (_identifier.IsEqual(currentEvent))
                Show = false;
        }
    }
}