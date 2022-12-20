﻿using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EBM2x.UI.BackofficeEntry
{
    //[Xamarin.Forms.RenderWith(typeof(Xamarin.Forms.Platform._PickerRenderer))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AmountMaskedViewPanel : ContentView
    {
        public string BorderColor
        {
            get { return base.GetValue(BorderColorProperty).ToString(); }
            set { base.SetValue(BorderColorProperty, value); }
        }
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
                                                         propertyName: "BorderColor",
                                                         returnType: typeof(string),
                                                         declaringType: typeof(AmountMaskedViewPanel),
                                                         defaultValue: "000000",
                                                         defaultBindingMode: BindingMode.TwoWay);
        public string BackColor
        {
            get { return base.GetValue(BackColorProperty).ToString(); }
            set { base.SetValue(BackColorProperty, value); }
        }
        public static readonly BindableProperty BackColorProperty = BindableProperty.Create(
                                                         propertyName: "BackColor",
                                                         returnType: typeof(string),
                                                         declaringType: typeof(AmountMaskedViewPanel),
                                                         defaultValue: "000000",
                                                         defaultBindingMode: BindingMode.TwoWay);

        public AmountMaskedViewPanel()
        {
            InitializeComponent();
            entryFixedGrid.InitializeComponent();
        }

        public Entry GetEntry()
        {
            return entryEntry;
        }
        public double GetEntryValue()
        {
            if (string.IsNullOrEmpty(entryEntry.Text)) return 0;
            else
            {
                try
                {
                    return Convert.ToDouble(entryEntry.Text.Replace(",", ""));
                }
                catch
                {
                    return 0;
                }
            }
        }
        public void SetEntryValue(double stringValue)
        {
            string buffer = stringValue.ToString("#,##0.##");

            if (buffer.IndexOf(".") >= 0)
            {
                try
                {
                    entryEntry.Text = stringValue.ToString("#,##0.##");
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    entryEntry.Text = stringValue.ToString("#,##0");
                }
                catch
                {
                }
            }
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
