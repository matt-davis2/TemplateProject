using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateProject.ViewModels;

public class TabViewModel
{
    public string Title { get; set; }
    public ObservableCollection<NoteViewModel> Notes { get; set; }

    public TabViewModel()
    {
        Notes = new ObservableCollection<NoteViewModel>();
    }
}
