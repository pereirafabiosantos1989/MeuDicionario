﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MeuDicionario.ViewModel
{
    public class BasePropertyChange : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
