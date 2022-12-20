﻿using EBM2x.UI.Draw;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EBM2x.UI.EntryPanel
{
    //[Xamarin.Forms.RenderWith(typeof(Xamarin.Forms.Platform._PickerRenderer))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemCodeMaskedEntryPanel : ContentView
    {
        public string Text
        {
            get { return base.GetValue(TextProperty).ToString(); }
            set { base.SetValue(TextProperty, value); }
        }
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                         propertyName: "Text",
                                                         returnType: typeof(string),
                                                         declaringType: typeof(ItemCodeMaskedEntryPanel),
                                                         defaultValue: string.Empty,
                                                         defaultBindingMode: BindingMode.TwoWay);
        public ItemCodeMaskedEntryPanel()
        {
            InitializeComponent();
            entryFixedGrid.InitializeComponent();
            entryTitle.InvalidateSurface(Text);
            entryEntry.TextChanged += OnTextChanged;
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Regex.IsMatch(e.NewTextValue, "^[a-zA-Z0-9-]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(e.NewTextValue, "[^a-zA-Z0-9-]", string.Empty);
        }

        public void TitleInvalidateSurface(string text)
        {
            if (string.IsNullOrEmpty(text)) Text = "";
            else Text = text;
            entryTitle.InvalidateSurface(text);
        }
        public void TitleInvalidateSurface()
        {
            entryTitle.InvalidateSurface(Text);
        }
        public void TitleInvalidateSurface(string text, string textAlign, string textColor, float fillRate)
        {
            if (string.IsNullOrEmpty(text)) Text = "";
            else Text = text;
            entryTitle.InvalidateSurface(text, textAlign, textColor, fillRate);
        }
        public DrawText GetEntryTitle()
        {
            return entryTitle;
        }
        public Entry GetEntry()
        {
            return entryEntry;
        }
        public string GetEntryValue()
        {
            // JINIT_201911
            string text = "";
            try
            {
                if (entryEntry.Text == null) text = "";
                text = entryEntry.Text.Replace("-", "");
            }
            catch
            {
            }
            return text;
        }
        public void SetEntryValue(string stringValue)
        {
            entryEntry.Text = stringValue;
        }
        public bool SetFocus()
        {
            return entryEntry.Focus();
        }
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(entryEntry.Text);
        }
        public void SetReadOnly(bool readOnly)
        {
            entryEntry.IsReadOnly = readOnly;
        }
    }

}
