using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TemplateProject.Models;

namespace TemplateProject.ViewModels;

public class NotesViewModel : BaseViewModel, IQueryAttributable
{
    public ObservableCollection<TabViewModel> Tabs { get; set; }
    public TabViewModel SelectedTab { get; set; }
    public ICommand SelectTabCommand { get; }
    public ObservableCollection<ViewModels.NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public NotesViewModel()
    {
        // Initialize tabs
        Tabs = new ObservableCollection<TabViewModel>
        {
            new TabViewModel { Title = "All Notes", Notes = new ObservableCollection<NoteViewModel> { } },
            new TabViewModel{ Title = "Favorites", Notes = new ObservableCollection<NoteViewModel> { /* add notes here*/}},
        };

        // Set default selected tab
        SelectedTab = Tabs[0];

        // Command to handle tab selection
        SelectTabCommand = new Command<TabViewModel>(tab =>
        {
            SelectedTab = tab;
            OnPropertyChanged(nameof(SelectedTab));
        });

        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
    }

    


    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(ViewModels.NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModel matchedNote = AllNotes.Where(x => x.Identifier == noteId).FirstOrDefault();

            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            NoteViewModel matchedNote = AllNotes.Where(x => x.Identifier == noteId).FirstOrDefault();

            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            else
                AllNotes.Insert(0, new NoteViewModel(Note.Load(noteId)));
        }
    }
}