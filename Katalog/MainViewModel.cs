using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace Katalog
{
    public class MainViewModel : ViewModel
    {
        public static MainViewModel Instance;
        private readonly KatalogDataModelContainer dataModel;
        private Visibility addPanelVisibility = Visibility.Collapsed;
        private Visibility viewPanelVisibility = Visibility.Collapsed;
        private Visibility mainPanelVisibility = Visibility.Collapsed;
        private Visibility metadataPanelVisibility = Visibility.Collapsed;
        private Objekt newObject;
        private Objekt selectedObject;

        private Departement tempDepartement = null;
        private Sprachgruppe tempSprachgruppe = null;
        private Dorf tempDorf = null;
        private Kategorie tempKategorie = null;

        private int backupcounter = 0;
        private int backupinterval = 10;

        public List<string> filterDepatrement = new List<string>(); 
        public List<string> filterSprachgruppe = new List<string>(); 
        public List<string> filterDorf = new List<string>(); 
        public List<string> filterKategorie = new List<string>();
        private string searchString = ""; //"Bitte Filter setzen";
        private int currentPage = 1;
        private int pageSize = 25;
        private string _currentPageString = "1";

        public ObservableCollection<Dorf> Dörfer => dataModel.DorfSet.Local;
        public ObservableCollection<Kategorie> Kategorien => dataModel.KategorieSet.Local;
        public ObservableCollection<Objekt> Objekte => dataModel.ObjektSet.Local;
        public ObservableCollection<Departement> Departemnte => dataModel.DepartementSet.Local;
        public ObservableCollection<Sprachgruppe> Sprachgruppen => dataModel.SprachgruppeSet.Local;
        public ObservableCollection<string> Fotos { get; } = new ObservableCollection<string>();

        public IEnumerable<Dorf> DorfFilters => Dörfer.Where(FilterDörfer);

        public IOrderedEnumerable<Objekt> ObjekteFiltered => Objekte.Where(FilterObjects).OrderBy(x=>x.ObjektNummer);
        public IEnumerable<Objekt> ObjekteFilteredPaged => ObjekteFiltered.Skip((CurrentPage-1)*PageSize).Take(PageSize);

        public int ObjectCount => ObjekteFiltered.Count();

        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (value == pageSize) return;
                pageSize = value;
                OnPropertyChanged();
            }
        }

        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (value == currentPage) return;
                currentPage = value;
                OnPropertyChanged();
                UpdateGui();
            }
        }

        public bool IsLeftEnabled => CurrentPage > 0;
        public bool IsRightEnabled => CurrentPage < TotalPages;

        public string CurrentPageString
        {
            get { return _currentPageString; }
            set
            {
                if (value == _currentPageString) return;
                _currentPageString = value;
                OnPropertyChanged();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        CurrentPage = int.Parse(value);
                    }
                    catch
                    {
                        
                    }
                }
                UpdateGui();
            }
        }


        public int TotalPages => (int)Math.Ceiling(((double)ObjectCount) / ((double)PageSize));
        
        public CommandRelay OnLeftPageNavClick { get; set; }
        public CommandRelay OnRightPageNavClick { get; set; }

        private void OnLeftClick()
        {
            if (CurrentPage > 1)
                CurrentPage--;
            UpdateGui();
        }

        private void OnRightClick()
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
            UpdateGui();
        }
        
        public bool DepartementEditEnabeled => tempDepartement != null;
        public bool SprachgruppeEditEnabeled => tempSprachgruppe != null;
        public bool DorfEditEnabeled => tempDorf != null;
        public bool KategorieEditEnabeled => tempKategorie != null;

        public Objekt SelectedObject
        {
            get { return selectedObject; }
            set
            {
                if (Equals(value, selectedObject)) return;
                selectedObject = value;
                OnPropertyChanged();
            }
        }

        public Objekt NewObject
        {
            get { return newObject; }
            set
            {
                if (Equals(value, newObject)) return;
                newObject = value;
                OnPropertyChanged();
            }
        }

        public string SearchString
        {
            get { return searchString; }
            set
            {
                if (value == searchString) return;
                searchString = value;
                OnPropertyChanged();
                UpdateGui();
            }
        }

        public string WhatIsGoingOn => $"NewObject:\r\n\t{NewObject?.Serialize().Replace("|","\r\n\t")??""}\r\nSelectedObject:\r\n\t{SelectedObject?.Serialize().Replace("|","\r\n\t") ?? ""}\r\nFilters:\r\nDepartemente: {string.Join(", ", filterDepatrement)}\r\nSprachgruppen: {string.Join(", ", filterSprachgruppe)}\r\nDorf: {string.Join(", ", filterDorf)}\r\nKategorien: {string.Join(", ", filterKategorie)}";

        #region Panels

        public Visibility AddPanelVisibility
        {
            get { return addPanelVisibility; }
            private set
            {
                if (value == addPanelVisibility) return;
                addPanelVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility MainPanelVisibility
        {
            get { return mainPanelVisibility; }
            private set
            {
                if (value == mainPanelVisibility) return;
                mainPanelVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ViewPanelVisibility
        {
            get { return viewPanelVisibility; }
            private set
            {
                if (value == viewPanelVisibility) return;
                viewPanelVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility MetadataPanelVisibility
        {
            get { return metadataPanelVisibility; }
            private set
            {
                if (value == metadataPanelVisibility) return;
                metadataPanelVisibility = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<int> Ticks => Enumerable.Range(1, TotalPages);

        public void SetPanel(Panels panel)
        {
            switch (panel)
            {
                case Panels.Main:
                    AddPanelVisibility = Visibility.Collapsed;
                    MetadataPanelVisibility = Visibility.Collapsed;
                    ViewPanelVisibility = Visibility.Collapsed;
                    MainPanelVisibility = Visibility.Visible;
                    break;
                case Panels.Add:
                    MainPanelVisibility = Visibility.Collapsed;
                    MetadataPanelVisibility = Visibility.Collapsed;
                    ViewPanelVisibility = Visibility.Collapsed;
                    AddPanelVisibility = Visibility.Visible;
                    break;
                case Panels.View:
                    MainPanelVisibility = Visibility.Collapsed;
                    AddPanelVisibility = Visibility.Collapsed;
                    MetadataPanelVisibility = Visibility.Collapsed;
                    ViewPanelVisibility = Visibility.Visible;
                    break;
                case Panels.Metadata:
                    MainPanelVisibility = Visibility.Collapsed;
                    AddPanelVisibility = Visibility.Collapsed;
                    ViewPanelVisibility = Visibility.Collapsed;
                    MetadataPanelVisibility = Visibility.Visible;
                    break;
            }
        }

        public enum Panels
        {
            Main,
            Add,
            View,
            Metadata
        }
        #endregion Panels

        public MainViewModel()
        {
            Instance = this;
            this.dataModel = new KatalogDataModelContainer();
            dataModel.DepartementSet.Load();
            dataModel.SprachgruppeSet.Load();
            dataModel.DorfSet.Load();
            dataModel.KategorieSet.Load();
            dataModel.ObjektSet.Load();
            newObject = dataModel.ObjektSet.Create();
            newObject.PropertyChanged += (s, e) => OnPropertyChanged(nameof(NewObject));
            SetPanel(Panels.Main);
            OnRightPageNavClick = new CommandRelay(OnLeftClick);
            OnLeftPageNavClick = new CommandRelay(OnRightClick);
        }

        public void Save()
        {
            NewObject.Bilder = string.Join(";", Fotos.Where(x=>!string.IsNullOrWhiteSpace(x)));

            if (NewObject.ObjektNummer.Length == 0)
            {
                MessageBox.Show("Keine Objektnummer festgelegt", "WARNUNG", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }
            
            if (NewObject.Bilder.Length == 0)
            {
                MessageBox.Show("Keine Bilder hinzugefügt", "WARNUNG", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                return;
            }

            if (NewObject.ObjektName == null) NewObject.ObjektName = "";
            if (NewObject.Herkunft == null) NewObject.Herkunft = "";
            if (NewObject.BeschreibungMaterial == null) NewObject.BeschreibungMaterial = "";
            if (NewObject.BeschreibungHerstellung == null) NewObject.BeschreibungHerstellung = "";
            if (NewObject.Zustand == null) NewObject.Zustand = "";
            if (NewObject.Masse == null) NewObject.Masse = "";
            if (NewObject.ErworbenBei == null) NewObject.ErworbenBei = "";
            if (NewObject.Datierung == null) NewObject.Datierung = "";
            if (NewObject.Versicherungswert == null) NewObject.Versicherungswert = "";
            if (NewObject.Dorf == null) NewObject.Dorf = Dörfer[0];
            if (NewObject.Kategorie == null) newObject.Kategorie = Kategorien[1];

            dataModel.ObjektSet.AddOrUpdate(NewObject);
            dataModel.SaveChanges();
            ResetNewItem();
            backupcounter++;
            if (backupcounter >= backupinterval)
                Backup();
        }

        public void ResetNewItem()
        {
            NewObject = dataModel.ObjektSet.Create();
            SelectedObject = NewObject;
            Fotos.Clear();
            UpdateGui();
            SetPanel(Panels.Main);
        }

        public void RemoveObject(Objekt objekt)
        {
            dataModel.ObjektSet.Remove(objekt);
            dataModel.SaveChanges();
            UpdateGui();
        }

        private bool FilterDörfer(Dorf arg)
        {
            if (filterDepatrement.Count>0)
                if (filterSprachgruppe.Count>0)
                    return filterSprachgruppe.Contains(arg.Sprachgruppe.Name) && filterDepatrement.Contains(arg.Departement.Name);
                else
                    return filterDepatrement.Contains(arg.Departement.Name);
            else
            {
                if (filterSprachgruppe.Count > 0)
                    return filterSprachgruppe.Contains(arg.Sprachgruppe.Name);
                return true;
            }
        }

        private bool FilterObjects(Objekt arg)
        {
            var filteredForDorf = filterDorf.Count > 0;
            var filteredForSprachgruppe = filterSprachgruppe.Count > 0;
            var filteredForDepartement = filterDepatrement.Count > 0;
            var filteredForKategorie = filterKategorie.Count > 0;
            var filteredForText = SearchString.Length > 0;

            if (filteredForDorf)
            {
                if (filteredForKategorie)
                {
                    if (filteredForText)
                    {
                        return Search(arg, SearchString) && filterDorf.Contains(arg.Dorf.Name) &&
                               (filterKategorie.Contains(arg.Kategorie.Name) ||
                                filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                    }
                    else
                    {
                        return filterDorf.Contains(arg.Dorf.Name) &&
                               (filterKategorie.Contains(arg.Kategorie.Name) ||
                                filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                    }
                }
                else
                {
                    if (filteredForText)
                    {
                        return Search(arg, SearchString) && filterDorf.Contains(arg.Dorf.Name);
                    }
                    else
                    {
                        return filterDorf.Contains(arg.Dorf.Name);
                    }
                }
            }
            else
            {
                if (filteredForDepartement)
                {
                    if (filteredForSprachgruppe)
                    {
                        if (filteredForKategorie)
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) &&
                                       filterDepatrement.Contains(arg.Dorf.Departement.Name) &&
                                       filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name) &&
                                       (filterKategorie.Contains(arg.Kategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                            }
                            else
                            {
                                return filterDepatrement.Contains(arg.Dorf.Departement.Name) &&
                                       filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name) &&
                                       (filterKategorie.Contains(arg.Kategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                            }
                        }
                        else
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) &&
                                       filterDepatrement.Contains(arg.Dorf.Departement.Name) &&
                                       filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name);
                            }
                            else
                            {
                                return filterDepatrement.Contains(arg.Dorf.Departement.Name) &&
                                       filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name);
                            }
                        }
                    }
                    else
                    {
                        if (filteredForKategorie)
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) &&
                                       filterDepatrement.Contains(arg.Dorf.Departement.Name) &&
                                       (filterKategorie.Contains(arg.Kategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                            }
                            else
                            {
                                return filterDepatrement.Contains(arg.Dorf.Departement.Name) &&
                                       (filterKategorie.Contains(arg.Kategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                            }
                        }
                        else
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) &&
                                       filterDepatrement.Contains(arg.Dorf.Departement.Name);
                            }
                            else
                            {
                                return filterDepatrement.Contains(arg.Dorf.Departement.Name);
                            }
                        }
                    }
                }
                else
                {
                    if (filteredForSprachgruppe)
                    {
                        if (filteredForKategorie)
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) &&
                                       filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name) &&
                                       (filterKategorie.Contains(arg.Kategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                            }
                            else
                            {
                                return filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name) &&
                                       (filterKategorie.Contains(arg.Kategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                        filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name));
                            }
                        }
                        else
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) &&
                                       filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name);
                            }
                            else
                            {
                                return filterSprachgruppe.Contains(arg.Dorf.Sprachgruppe.Name);
                            }
                        }
                    }
                    else
                    {
                        if (filteredForKategorie)
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString) && filterKategorie.Contains(arg.Kategorie.Name) ||
                                       filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                       filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name);
                            }
                            else
                            {
                                return filterKategorie.Contains(arg.Kategorie.Name) ||
                                       filterKategorie.Contains(arg.Kategorie.Oberkategorie.Name) ||
                                       filterKategorie.Contains(arg.Kategorie.Oberkategorie.Oberkategorie.Name);
                            }
                        }
                        else
                        {
                            if (filteredForText)
                            {
                                return Search(arg, SearchString);
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        private bool Search(Objekt o, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern)) pattern = ".*";
            return o.ObjektNummer.ToLower().Matches(pattern.ToLower())
                || o.ObjektName.ToLower().Matches(pattern.ToLower())
                || o.Dorf.Name.ToLower().Matches(pattern.ToLower())
                || o.Dorf.Departement.Name.ToLower().Matches(pattern.ToLower())
                || o.Dorf.Sprachgruppe.Name.ToLower().Matches(pattern.ToLower())
                || o.Kategorie.Name.ToLower().Matches(pattern.ToLower())
                || o.Kategorie.Oberkategorie.Name.ToLower().Matches(pattern.ToLower())
                || o.Kategorie.Oberkategorie.Oberkategorie.Name.ToLower().Matches(pattern.ToLower())
                || o.Datierung.ToLower().Matches(pattern.ToLower())
                || o.ErworbenBei.ToLower().Matches(pattern.ToLower())
                || o.Masse.ToLower().Matches(pattern.ToLower())
                || o.Zustand.ToLower().Matches(pattern.ToLower())
                || o.Bilder.ToLower().Matches(pattern.ToLower())
                || o.BeschreibungHerstellung.ToLower().Matches(pattern.ToLower())
                || o.BeschreibungMaterial.ToLower().Matches(pattern.ToLower())
                || o.Versicherungswert.ToLower().Matches(pattern.ToLower());
        }

        public void UpdateSearchResults()
        {
            UpdateGui();
        }

        public void AddFilterDepartement(string name)
        {
            if(!filterDepatrement.Contains(name))
                filterDepatrement.Add(name);
            filterDorf.Clear();
            OnPropertyChanged(nameof(DorfFilters));
            UpdateGui();
        }

        public void AddFilterSprachgruppe(string name)
        {
            if(!filterSprachgruppe.Contains(name))
                filterSprachgruppe.Add(name);
            filterDorf.Clear();
            OnPropertyChanged(nameof(DorfFilters));
            UpdateGui();
        }

        public void AddFilterDorf(string name)
        {

            if(!filterDorf.Contains(name))
                filterDorf.Add(name);
            UpdateGui();
        }

        public void AddFilterKategorie(string name)
        {
            if(!filterKategorie.Contains(name))
                filterKategorie.Add(name);
            UpdateGui();
        }

        public void RemoveFilterDepartement(string name)
        {
            if(filterDepatrement.Contains(name))
                filterDepatrement.Remove(name);
            filterDorf.Clear();
            OnPropertyChanged(nameof(DorfFilters));
            UpdateGui();
        }

        public void RemoveFilterSprachgruppe(string name)
        {
            if(filterSprachgruppe.Contains(name))
                filterSprachgruppe.Remove(name);
            filterDorf.Clear();
            OnPropertyChanged(nameof(DorfFilters));
            UpdateGui();
        }

        public void RemoveFilterDorf(string name)
        {
            if(filterDorf.Contains(name))
                filterDorf.Remove(name);
            UpdateGui();
        }

        public void RemoveFilterKategorie(string name)
        {
            if(filterKategorie.Contains(name))
                filterKategorie.Remove(name);
            UpdateGui();
        }

        public void AddNewDepartement(string name)
        {
            var dep = dataModel.DepartementSet.Create();
            dep.Name = name;
            dataModel.DepartementSet.AddOrUpdate(dep);
            dataModel.SaveChanges();
            OnPropertyChanged(nameof(Departemnte));
            MessageBox.Show($"Departement {name} hinzugefügt");
        }
        public void AddNewSprachgruppe(string name)
        {
            var dep = dataModel.SprachgruppeSet.Create();
            dep.Name = name;
            dataModel.SprachgruppeSet.AddOrUpdate(dep);
            dataModel.SaveChanges();
            OnPropertyChanged(nameof(Sprachgruppen));
            MessageBox.Show($"Sprachgruppe {name} hinzugefügt");
        }
        public void AddNewDorf(string name, Departement departement, Sprachgruppe sprachgruppe)
        {
            var dep = dataModel.DorfSet.Create();
            dep.Name = name;
            dep.Departement = departement;
            dep.Sprachgruppe = sprachgruppe;
            dataModel.DorfSet.AddOrUpdate(dep);
            dataModel.SaveChanges();
            OnPropertyChanged(nameof(Dörfer));
            OnPropertyChanged(nameof(DorfFilters));
            MessageBox.Show($"Dorf {name} hinzugefügt");
        }
        public void AddNewKategorie(string name, Kategorie oberkategorie)
        {
            var dep = dataModel.KategorieSet.Create();
            dep.Name = name;
            dep.Oberkategorie = oberkategorie;
            dataModel.KategorieSet.AddOrUpdate(dep);
            dataModel.SaveChanges();
            OnPropertyChanged(nameof(Kategorien));
            MessageBox.Show($"Kategorie {name} hinzugefügt");
        }

        public void AddEditDepartement(string name)
        {
            if (tempDepartement == null) return;
            tempDepartement.Name = name;
            dataModel.DepartementSet.AddOrUpdate(tempDepartement);
            dataModel.SaveChanges();

            MessageBox.Show($"Departement {name} bearbeitet");
            tempDepartement = null;
            OnPropertyChanged(nameof(Departemnte));
            OnPropertyChanged(nameof(DepartementEditEnabeled));
        }
        public void AddEditSprachgruppe(string name)
        {
            if(tempSprachgruppe == null) return;
            tempSprachgruppe.Name = name;
            dataModel.SprachgruppeSet.AddOrUpdate(tempSprachgruppe);
            dataModel.SaveChanges();

            MessageBox.Show($"Sprachgruppe {name} bearbeitet");
            tempSprachgruppe = null;
            OnPropertyChanged(nameof(Sprachgruppen));
            OnPropertyChanged(nameof(SprachgruppeEditEnabeled));
        }
        public void AddEditDorf(string name, Departement departement, Sprachgruppe sprachgruppe)
        {
            if (tempDorf == null) return;
            tempDorf.Name = name;
            tempDorf.Departement = departement;
            tempDorf.Sprachgruppe = sprachgruppe;
            dataModel.DorfSet.AddOrUpdate(tempDorf);
            dataModel.SaveChanges();

            MessageBox.Show($"Dorf {name} bearbeitet");
            tempDorf = null;
            OnPropertyChanged(nameof(Dörfer));
            OnPropertyChanged(nameof(DorfFilters));
            OnPropertyChanged(nameof(DorfEditEnabeled));
        }
        public void AddEditKategorie(string name, Kategorie oberkategorie)
        {
            if (tempKategorie == null) return;
            tempKategorie.Name = name;
            tempKategorie.Oberkategorie = oberkategorie;
            dataModel.KategorieSet.AddOrUpdate(tempKategorie);
            dataModel.SaveChanges();
            MessageBox.Show($"Kategorie {name} bearbeitet");
            tempKategorie = null;
            OnPropertyChanged(nameof(Kategorien));
            OnPropertyChanged(nameof(KategorieEditEnabeled));
        }

        public void Load(Departement departement)
        {
            tempDepartement = departement;
            OnPropertyChanged(nameof(DepartementEditEnabeled));
        }
        public void Load(Sprachgruppe sprachgruppe)
        {
            tempSprachgruppe = sprachgruppe;
            OnPropertyChanged(nameof(SprachgruppeEditEnabeled));
        }
        public void Load(Dorf dorf)
        {
            tempDorf = dorf;
            OnPropertyChanged(nameof(DorfEditEnabeled));
        }
        public void Load(Kategorie kategorie)
        {
            tempKategorie = kategorie;
            OnPropertyChanged(nameof(KategorieEditEnabeled));
        }

        public void UnloadDepartement()
        {
            tempDepartement = null;
            OnPropertyChanged(nameof(DepartementEditEnabeled));
        }
        public void UnloadSprachgruppe()
        {
            tempSprachgruppe = null;
            OnPropertyChanged(nameof(SprachgruppeEditEnabeled));
        }
        public void UnloadDorf()
        {
            tempDorf = null;
            OnPropertyChanged(nameof(DorfEditEnabeled));
        }
        public void UnloadKategorie()
        {
            tempKategorie = null;
            OnPropertyChanged(nameof(KategorieEditEnabeled));
        }

        public void Backup()
        {
            //DBHelper.Backup();

            
            var separator = $"\r\n{(char)0x00002605}\r\n";
            var path = "";
#if DEBUG
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.GetBackupFileName());
#else
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.GetBackupFileName());
#endif
            var items = string.Join("\r\n", Instance.Objekte.Select(x => x.Serialize()));
            var departemente = string.Join("\r\n", Instance.Departemnte.Select(x => $"{x.Id}|{x.Name}"));
            var sprachgruppen = string.Join("\r\n", Instance.Sprachgruppen.Select(x => $"{x.Id}|{x.Name}"));
            var dörfer = string.Join("\r\n", Instance.Dörfer.Select(x => $"{x.Id}|{x.Name}|{x.Departement.Id}|{x.Departement.Id}"));
            var kategorien = string.Join("\r\n", Instance.Kategorien.Select(x => $"{x.Id}|{x.Name}|{x.Oberkategorie.Id}"));
            var data = $"{items}" +
                       $"{separator}" +
                       $"{departemente}" +
                       $"{separator}" +
                       $"{sprachgruppen}" +
                       $"{separator}" +
                       $"{dörfer}" +
                       $"{separator}" +
                       $"{kategorien}";
            File.WriteAllText(path, data);
            MessageBox.Show($"Backup nach {path} gespeichert");
            backupcounter = 0;

            FileHelper.ZipBackups();
        }


        private void UpdateGui()
        {
            if (CurrentPage > TotalPages) CurrentPage = TotalPages;
            CurrentPageString = CurrentPage.ToString();
            OnPropertyChanged(nameof(ObjekteFiltered));
            OnPropertyChanged(nameof(ObjekteFilteredPaged));
            OnPropertyChanged(nameof(ObjectCount));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(IsLeftEnabled));
            OnPropertyChanged(nameof(IsRightEnabled));
        }



        //public void NewBackup()
        //{
        //    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.GetBackupFileName());

        //    dataModel.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
        //        $"BACKUP DATABASE KatalogDatabase TO DISK='{path}' WITH FORMAT, MEDIANAME='DbBackups', MEDIADESCRIPTION='Media set for KatalogDatabase database';");

        //    MessageBox.Show($"Backup nach {path} gespeichert");
        //    backupcounter = 0;

        //    FileHelper.ZipBackups();
        //}

        //        public void RestoreBackup(string path)
        //        {
        //            var c = dataModel.Database.Connection;

        //            //var state = $"{c.Database}";
        //            //MessageBox.Show(state);
        //            //Debugger.Break();
        //            //dataModel.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
        //            //    "ALTER DATABASE [KatalogDatabase] SET OFFLINE WITH ROLLBACK IMMEDIATE;" +
        //            //    "ALTER DATABASE [KatalogDatabase] SET ONLINE;" +
        //            //    $"RESTORE DATABASE \"\" FROM DISK='{path}' WITH REPLACE;");

        //            dataModel.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
        //                "ALTER DATABASE [KatalogDatabase] SET OFFLINE WITH ROLLBACK IMMEDIATE;");
        //            DBHelper.Restore(c.DataSource, c.Database, path);
        //            dataModel.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,
        //                "ALTER DATABASE [KatalogDatabase] SET ONLINE;");
        //MessageBox.Show($"Backup von {path} geladen");
        //        }
    }
}