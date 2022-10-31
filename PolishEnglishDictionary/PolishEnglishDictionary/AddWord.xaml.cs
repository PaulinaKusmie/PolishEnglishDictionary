﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PolishEnglishDictionary
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddWord : ContentPage
    {
        public AddWord()
        {
            BindingContext = new ViewModel.AddWordViewModel();
            InitializeComponent();
        }
    }
}