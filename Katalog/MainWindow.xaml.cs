using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;

namespace Katalog
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += LoadingDone;
            
        }
        
        private void LoadingDone(object sender, RoutedEventArgs routedEventArgs)
        {
            DataContext = new MainViewModel();

            ObjectSelector.Items.SortDescriptions.Clear();
            ObjectSelector.Items.SortDescriptions.Add(new SortDescription("ObjektNummer", ListSortDirection.Ascending));

            DepartementeMeta.Items.SortDescriptions.Clear();
            DepartementeMeta.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            SprachgruppeMeta.Items.SortDescriptions.Clear();
            SprachgruppeMeta.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            DorfMeta.Items.SortDescriptions.Clear();
            DorfMeta.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            KategorieMeta.Items.SortDescriptions.Clear();
            KategorieMeta.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            ObjectSelector.Items.GroupDescriptions.Clear();
            ObjectSelector.Items.GroupDescriptions.Add(new PropertyGroupDescription("GroupTag"));

        }

        private void OnDropPicture(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
            files.Foreach(MainViewModel.Instance.Fotos.Add);
        }

        private void OnSave(object s, RoutedEventArgs e)
        {
            try
            {
                var dataInvalid =
                    new[]
                    {
                        ObjectNumberBox.Text, ObjectNameBox.Text, MaterialdDescription.Text, ProcessingDescription.Text,
                        Condition.Text, Measure.Text, Date.Text, Value.Text, Origin.Text
                    }.Any(x => string.IsNullOrWhiteSpace(x?.ToString()));

                if (dataInvalid || CategorySelector.SelectedIndex == -1 || CategorySelector.SelectedIndex == -1) 
                {
                    var resuslt = MessageBox.Show(Application.Current.MainWindow, "Wirklich speichern?",
                        "Einige Felder sind noch leer", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                    if (resuslt == MessageBoxResult.No) return;
                }

                var d = (Dorf) VillageSelector.SelectionBoxItem??MainViewModel.Instance.Dörfer[0];
                var k = (Kategorie) CategorySelector.SelectionBoxItem??MainViewModel.Instance.Kategorien[0];

                try
                {
                    MessageBox.Show($"Neuer Eintrag:\r\n\r\n" +
                                    $"Objektnummer: {ObjectNumberBox.Text}\r\n" +
                                    $"Objektname: {ObjectNameBox.Text}\r\n" +
                                    $"Kategorie: {k.Crumb}\r\n" +
                                    $"Dorf: {d.Name}\r\n\r\n" +
                                    $"Departement: {d.Departement.Name}\r\n" +
                                    $"Sprachgruppe: {d.Sprachgruppe.Name}\r\n" +
                                    $"Beschreibung:\r\n" +
                                    $"\tMaterial: {MaterialdDescription.Text}\r\n" +
                                    $"\tHerstellung: {ProcessingDescription.Text}\r\n" +
                                    $"Zustand: {Condition.Text}\r\n" +
                                    $"Masse: {Measure.Text}\r\n" +
                                    $"Datierung: {Date.Text}\r\n" +
                                    $"Versicherungswert: {Value.Text}\r\n" +
                                    $"Bemerkungen: {Notes.Text}\r\n" +
                                    $"Fotos:\r\n {string.Join("\r\n",MainViewModel.Instance.Fotos)}\r\n");
                    MainViewModel.Instance.Save();
                    Sort();
                }
                catch (Exception ex)
                {
                    var foo = MainViewModel.Instance.NewObject;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void OnNewObject(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SetPanel(MainViewModel.Panels.Add);
            ObjectNumberBox.Focus();
        }

        private void OnCancle(object sender, RoutedEventArgs e)
        {
            var unsavedChanges = false;
            unsavedChanges |= !string.IsNullOrWhiteSpace(ObjectNumberBox.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(ObjectNameBox.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(MaterialdDescription.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(ProcessingDescription.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(Condition.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(Measure.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(Date.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(Value.Text);
            unsavedChanges |= !string.IsNullOrWhiteSpace(Notes.Text);
            if (!unsavedChanges)
            {
                MainViewModel.Instance.SetPanel(MainViewModel.Panels.Main);
                return;
            }

            var res = MessageBox.Show("Nicht gespeichert, wirklich abbrechen?", "Warnung", MessageBoxButton.YesNo,
                MessageBoxImage.Warning, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
                MainViewModel.Instance.ResetNewItem();
        }

        private void OnElementSelected(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SelectedObject = (Objekt) ((Button) sender).DataContext;
            MainViewModel.Instance.SetPanel(MainViewModel.Panels.View);
        }

        private void OnEdit(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.NewObject = MainViewModel.Instance.SelectedObject;
            MainViewModel.Instance.SelectedObject.Bilder.Split(';').Foreach(MainViewModel.Instance.Fotos.Add);
            MainViewModel.Instance.SetPanel(MainViewModel.Panels.Add);
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SetPanel(MainViewModel.Panels.Main);
        }

        private void OnFilter(object sender, RoutedEventArgs e)
        {
            FilterPanel.Visibility = FilterPanel.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void OnRemovePicture(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Sicher?", "WARNUNG", MessageBoxButton.YesNo);
            if(res == MessageBoxResult.Yes)
                MainViewModel.Instance.Fotos.Remove(((MenuItem)sender).DataContext as string);
        }

        private void DeleteObject(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Sicher?", "WARNUNG", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                MainViewModel.Instance.RemoveObject(MainViewModel.Instance.Objekte.FirstOrDefault(x => x.Id == ((Objekt)((MenuItem)sender).DataContext).Id));
                MainViewModel.Instance.SetPanel(MainViewModel.Panels.Main);
            }
            Sort();
        }

        public void OnBackupObjects(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.Backup();
        }
        private void Sort()
        {
            ObjectSelector.Items.SortDescriptions.Clear();
            ObjectSelector.Items.SortDescriptions.Add(new SortDescription("ObjektNummer", ListSortDirection.Ascending));
        }


        private void OnDorfFilterSelectionChanged(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox) sender;
            if (cb.IsChecked.HasValue && cb.IsChecked.Value)
            {
                MainViewModel.Instance.AddFilterDorf(((TextBlock)cb.Content).Text);
            }
            else
            {
                MainViewModel.Instance.RemoveFilterDorf(((TextBlock)cb.Content).Text);
            }
            Sort();
        }

        private void OnDepartementFilterSelectionChanged(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox) sender;
            if (cb.IsChecked.HasValue && cb.IsChecked.Value)
            {
                MainViewModel.Instance.AddFilterDepartement(((TextBlock)cb.Content).Text);
            }
            else
            {
                MainViewModel.Instance.RemoveFilterDepartement(((TextBlock)cb.Content).Text);
            }
            Sort();
        }

        private void OnSprachgruppeFilterSelectionChanged(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox) sender;
            if (cb.IsChecked.HasValue && cb.IsChecked.Value)
            {
                MainViewModel.Instance.AddFilterSprachgruppe(((TextBlock)cb.Content).Text);
            }
            else
            {
                MainViewModel.Instance.RemoveFilterSprachgruppe(((TextBlock)cb.Content).Text);
            }

            Sort();
        }

        private void OnKategorieFilterSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            var cb = (CheckBox) sender;
            if (cb.IsChecked.HasValue && cb.IsChecked.Value)
            {
                MainViewModel.Instance.AddFilterKategorie(((TextBlock)cb.Content).Text);
            }
            else
            {
                MainViewModel.Instance.RemoveFilterKategorie(((TextBlock)cb.Content).Text);
            }

            Sort();
        }

        private void OnMetadata(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SetPanel(MainViewModel.Panels.Metadata);
        }

        private void OnShowMain(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.SetPanel(MainViewModel.Panels.Main);
            Sort();
        }

        private void OnInfo(object sender, RoutedEventArgs e)
        {

        }

        private void OnNewDepartement(object sender, RoutedEventArgs e)
        {
            if (NewDepartementTextbox.Text == "") return;
            MainViewModel.Instance.AddNewDepartement(NewDepartementTextbox.Text);
            NewDepartementTextbox.Text = "";
        }

        private void OnNewSprachgruppe(object sender, RoutedEventArgs e)
        {
            if (NewSprachgruppeTextbox.Text == "") return;
            MainViewModel.Instance.AddNewSprachgruppe(NewSprachgruppeTextbox.Text);
            NewSprachgruppeTextbox.Text = "";
        }

        private void OnNewDorf(object sender, RoutedEventArgs e)
        {
            if (NewDorfTextbox.Text == "" || NewDorfDepartement.SelectedIndex == -1 || NewDorfSprachgrupe.SelectedIndex == -1) return;
            MainViewModel.Instance.AddNewDorf(NewDorfTextbox.Text, (Departement)NewDorfDepartement.SelectedItem, (Sprachgruppe)NewDorfSprachgrupe.SelectedItem);
            NewDorfTextbox.Text = "";
        }
        private void OnNewKategorie(object sender, RoutedEventArgs e)
        {
            if (NewKategorieTextbox.Text == "" || NewKategorieOberkategorie.SelectedIndex == -1) return;
            MainViewModel.Instance.AddNewKategorie(NewKategorieTextbox.Text, (Kategorie)NewKategorieOberkategorie.SelectedItem);
            NewDepartementTextbox.Text = "";
        }
        
        private void OnEditDepartement(object sender, RoutedEventArgs e)
        {
            if (NewDepartementTextbox.Text == "") return;
            MainViewModel.Instance.AddEditDepartement(NewDepartementTextbox.Text);
            NewDepartementTextbox.Text = "";
        }

        private void OnEditSprachgruppe(object sender, RoutedEventArgs e)
        {
            
            if (NewSprachgruppeTextbox.Text == "") return;
            MainViewModel.Instance.AddEditSprachgruppe(NewSprachgruppeTextbox.Text);
            NewSprachgruppeTextbox.Text = "";
        }

        private void OnEditDorf(object sender, RoutedEventArgs e)
        {
            if (NewDorfTextbox.Text == "" || NewDorfDepartement.SelectedIndex == -1 || NewDorfSprachgrupe.SelectedIndex == -1) return;
            MainViewModel.Instance.AddEditDorf(NewDorfTextbox.Text, (Departement)NewDorfDepartement.SelectedItem, (Sprachgruppe)NewDorfSprachgrupe.SelectedItem);
            NewDorfTextbox.Text = "";
            NewDorfDepartement.SelectedIndex = -1;
            NewDorfSprachgrupe.SelectedIndex = -1;
        }

        private void OnEditKategorie(object sender, RoutedEventArgs e)
        {
            if (NewKategorieTextbox.Text == "" || NewKategorieOberkategorie.SelectedIndex==-1) return;
            MainViewModel.Instance.AddEditKategorie(NewKategorieTextbox.Text, (Kategorie)NewKategorieOberkategorie.SelectedItem);
            NewDepartementTextbox.Text = "";
            NewKategorieOberkategorie.SelectedIndex = -1;
        }

        private void OnMetadataDepartementSelected(object sender, RoutedEventArgs e)
        {
            var departement = (Departement)DepartementeMeta.SelectedItem;
            MainViewModel.Instance.Load(departement);
            NewDepartementTextbox.Text = departement.Name;
        }

        private void OnMetadataSprachgruppeSelected(object sender, RoutedEventArgs e)
        {
            var sprachgruppe = (Sprachgruppe)SprachgruppeMeta.SelectedItem;
            MainViewModel.Instance.Load(sprachgruppe);
            NewSprachgruppeTextbox.Text = sprachgruppe.Name;
        }

        private void OnMetadataDorfSelected(object sender, RoutedEventArgs e)
        {
            var dorf = (Dorf)DorfMeta.SelectedItem;
            MainViewModel.Instance.Load(dorf);
            NewDorfTextbox.Text = dorf.Name;
            NewDorfDepartement.SelectedItem = dorf.Departement;
            NewDorfSprachgrupe.SelectedItem = dorf.Sprachgruppe;
        }

        private void OnMetadataKategorieSelected(object sender, RoutedEventArgs e)
        {
            var kategorie = (Kategorie)KategorieMeta.SelectedItem;
            MainViewModel.Instance.Load(kategorie);
            NewKategorieTextbox.Text = kategorie.Name;

            NewKategorieOberkategorie.SelectedItem = kategorie.Oberkategorie;
        }

        private void OnUnloadDepartement(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.UnloadDepartement();
            NewDepartementTextbox.Text = "";
        }

        private void OnUnloadSprachgruppe(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.UnloadSprachgruppe();
            NewSprachgruppeTextbox.Text = "";
        }

        private void OnUnloadDorf(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.UnloadDorf();
            NewDorfTextbox.Text = "";
            NewDorfDepartement.SelectedIndex = -1;
            NewDorfSprachgrupe.SelectedIndex = -1;
        }

        private void OnUnloadKategorie(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.UnloadKategorie();
            NewKategorieTextbox.Text = "";
            NewKategorieOberkategorie.SelectedIndex = -1;
        }


    }
}
