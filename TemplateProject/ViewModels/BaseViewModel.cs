using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TemplateProject.ViewModels;

/// <summary>
/// Notifies subscribers when a property value changes. Uses CallerMemberName to automatically capture the name of the
/// calling property.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// An event that is triggered when a property value changes, allowing subscribers to be notified of updates. It is
    /// commonly used in data binding scenarios.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
