   M S B A M L   `   `   `  ���� B  >Katalog, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:KatalogKatalog  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem     Katalog.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Katalog     9system&clr-namespace:System;assembly=mscorlib     -     �Name$  window�� � Title$ Katalog��$	��600��$	��800��$��15(�.��� -    �Name$ ContentRoot��$
��Auto��$
��Auto��.+�� -   $ AddObjectPanel��$
��Auto��$
��Auto��   Katalog.GridExtention   CompactRowDefinitions$ * auto��   CompactColumnDefinitions$ * auto��y��� *AddPanelVisibility+.+�� $��0��$��0��.+��� .���� $��30 30 30 30q� � DataContext �� *	NewObject+.+�� $ 	auto auto��.+��� $��0��$��Objektnummer��.���� -    ��Name$ ObjectNumberBox��$��1������ *ObjektNummer+�� $��1��$��
Objektname��.���� -   $ ObjectNameBox��$��2������ *
ObjektName+� -   $ 	Selectors��$	 * *��.+��� $��Herkunft��$��0��.���� -    ��Name$ VillageSelector��$��3�� w� TextPath$
 Name������ *	Dörfer+	 �� SelectedItem	 �� *Dorf+���� .R�� $	 * *��$	 * *��.+��� $��23(�
 �� TextAlignment$
 Center=�$��Center=�$��Center=�$��0��$��2��$��0������ *Name+�� $��Sprachgruppe��$��0��$��1����.   �� *Sprachgruppe+.���� $
 Center=�$��Center=�$��Center=����� *Name+�� $��Departement��$��20,0,0,0q�$��1��$��1����.   �� *Departement+.���� $
 Center=�$��Center=�$��Center=����� *Name+�� $��	Kategorie��$��1��.���� -   $ CategorySelector��$��4������ *
Kategorien+	 �� *	Kategorie+���� .R�� .+��� ��.  ���� *Crumb+.���� $��23(����� *Name+�� .+��� $��Beschreibung��.���� $	��600��.+��� $��Material���� -	   $ MaterialdDescription�� �� TextWrapping$
 Wrap=�$
��Auto��$	��100��" �� VerticalScrollBarVisibility$
 Auto=�$��5������ *BeschreibungMaterial+�� $��Herstellung���� -
   $ ProcessingDescription��$
 Wrap=�$
��Auto��$	��100��$
 Auto=�$��6������ *BeschreibungHerstellung+�� $��Zustand���� -   $ 	Condition��$��7������ *	Zustand+�� $��Masse���� -   $ Measure��$��8������ *Masse+�� $��Erworben / Bei���� -   $ Origin��$��9������ *ErworbenBei+�� $��	Datierung���� -   $
 Date��$��10������ *	Datierung+�� $��Versicherungswert���� -   $ Value��$��11������ *Versicherungswert+�� $��Bemerkungen���� -   $ Notes��$��12��$
 Wrap=�$
��Auto��$	��100��$
 Auto=����� *
Herkunft+�� -    ��Name$ FotoDropTarget��
���OO/�$	��100�� J� 	AllowDrop . $��0��$��1��
���OO/�._���  %� IsVirtualizing .  %� VirtualizationMode$ 	Recycling=����� *Fotos+���� .R��  � Orientation$ Vertical=����� .R��  � ContextMenu �� .@�m� -   $��	Entfernen��.+��� $	��200��$	��200������  � ToolTip _� .���� ���� � $��1��$��0��$��2��$��50��$	 * *��.+��� -   $��0��$��	Speichern��$��13���� -   $��1��$��	Abbrechen��$��14��� -   $ 	MainPanel��$
��Auto��$
��Auto��$ auto * auto auto��y��� *MainPanelVisibility+.+�� $��0��$( "auto auto auto auto auto auto auto��.+��� -   $��0��$��5q�$��0 0 10 0q�.��� $ * auto��.+��� $��0��$��Assets/Icons/add-icon.png��$��75��$��75��$��Uniform=��� $��1��.��Neu Erfassend� -   $��1��$��5q�$��0 0 10 0q�.��� $ * auto��.+��� $��0��$"��Assets/Icons/filter-icon.png��$��75��$��75��$��Uniform=��� $��1��.��Filter�� -   $��2��$��5q�$��0 0 10 0q�.��� $ * auto��.+��� $��0��$"��Assets/Icons/backup-icon.png��$��75��$��75��$��Uniform=��� $��1��.��Backup Speichern�� -   $��3��$��5q�$��0 0 10 0q�.��� $ * auto��.+��� $��0��$ ��Assets/Icons/meta-icon.png��$��75��$��75��$��Uniform=��� $��1��.��	Metadaten�� -   $��4��$��5q�$��0 0 10 0q�.��� $ * auto��.+��� $��0��$!��Assets/Icons/debug-icon.png��$��75��$��75��$��Uniform=��� $��1��.��	Restore� $��2��$��Center=� � 	Resources     Katalog.DivideBy2Converter   DivideBy2Converter��%   &	           � ColumnDefinitions �� $
��Auto ��� $��* �$	��800���� $
��Auto �.+��� $��50��$��0������ *
Background+ �� ElementName$ ContentRoot������ *
Background+$ ContentRoot������ *OnRightPageNavClick+{��� *IsLeftEnabled+.���� -    ��Name$ LeftArrowImage��$!��Assets/Icons/arrow-icon.png��z���  �� Angle$	 180Q� �� CenterX �� *ActualWidth+$ LeftArrowImage�� �� 	Converter# [   �� CenterY �� *ActualHeight+$ LeftArrowImage��# [  �� $��1��.+��� $��100 10 100 10q�$	��400�� � Minimum$ 1Q� �� Interval$ 1�� �� IsMoveToPointEnabled .  �� IsSnapToTickEnabled .   � Maximum  �� *
TotalPages+! � Value! �� *CurrentPage+" �� Ticks" �� *Ticks+�� $��35��# �� Orientation$# 
Horizontal=�$��Center=�.+��� $��25��$��50��$ �� TextAlignment$$ Center=����� *CurrentPageString+�� $��25��$��	10 0 10 0q�$��Center=��� $	� / �� �� ��� *
TotalPages+% �� Mode$% OneWay=��� $��50��$��25���� $��50��$��2������ *
Background+$ ContentRoot������ *
Background+$ ContentRoot������ *OnLeftPageNavClick+{��� *IsRightEnabled+.���� $!��Assets/Icons/arrow-icon.png���� -   & ��Name$& ObjectSelector�� . $ 	Recycling=�$��1������ *ObjekteFilteredPaged+���� .R�� $ 
Horizontal=����� *ActualWidth+$ window��$% OneWay=����� .R��� -   $��60��$��60��
������ 
�������.��   Katalog.ObjectControl   ��  �� .@�m� -   $��	Entfernen��' �� 
GroupStyle' (  "System.Windows.Controls.GroupStyle  (   ContainerStyle( �� ) �� 
TargetType") ��.��� * �� Property* � ��+ �� Value+ �� .R�9� , 9� 
IsExpanded, . ���� *Name+.���� � -   $ FilterPanel��$��3��$y�	Collapsed=�$	��300��$ auto auto auto auto *��$ auto *��.+��� $��0��$��0��.��Departement�� -    $& DepartementFilter��$��1��$��0������ *Departemnte+���� .R��� -!   .���� ���� *Name+�� $��0��$��1��.��Sprachgruppen�� -"   $& SprachgruppeFilter��$��1��$��1������ *Sprachgruppen+���� .R��� -#   .���� ���� *Name+�� $��0��$��2��.��	Dörfer�� -$   $& 
DorfFilter��$��1��$��2������ *DorfFilters+���� .R��� -%   .���� ���� *Name+�� $��0��$��3��.��
Kategorien�� -&   $& KategogrieFilter��$��1��$��3������ *
Kategorien+���� .R��� -'   .���� ���� *Name+�� $��0��$��4��.��Suchen� $��1��$��4��.+��� -(   $ SearchTextBox������ *SearchString+� -)   $ 	ViewPanel��$
��Auto��$
��Auto��y��� *ViewPanelVisibility+.+�� $ * auto�� �� *SelectedObject+.+�� $��1��$��50��$	 * *��.+��� -*   $��0��.��
Bearbeiten�� -+   $��1��.��	Zurück� $��0��$
 2* *��.+��� $��1�� . $ 	Recycling=����� *BilderSources+���� .R�� $ Vertical=����� .R��� $	��250��$	��250��- ^� InitialShowDelay$
- 3000������  _� .���� ���� � $��0��.+��� .���� $��30 30 30 30q�.+��� $��Objektnummer��.���� . �� Padding$. 10 0 0 5q�$��20(����� *ObjektNummer+�� $��
Objektname��.���� $. 10 0 0 5q�$��20(����� *
ObjektName+�� $��Herkunft>Dorf��.���� $. 10 0 0 5q�$��20(����� *	Dorf.Name+�� $��Herkunft>Departement��.���� $. 10 0 0 5q�$��20(����� *Dorf.Departement.Name+�� $��Herkunft>Sprachgruppe��.���� $. 10 0 0 5q�$��20(����� *Dorf.Sprachgruppe.Name+�� $��	Kategorie��.���� $. 10 0 0 5q�$��20(����� *Kategorie.Crumb+�� $��Beschreibung Material��.���� $. 10 0 0 5q�$��20(����� *BeschreibungMaterial+�� $��Beschreibung Herstellung��.���� $. 10 0 0 5q�$��20(����� *BeschreibungHerstellung+�� $��Zustand��.���� $. 10 0 0 5q�$��20(����� *	Zustand+�� $��Masse��.���� $. 10 0 0 5q�$��20(����� *Masse+�� $��Erworben / Bei��.���� $. 10 0 0 5q�$��20(����� *ErworbenBei+�� $��	Datierung��.���� $. 10 0 0 5q�$��20(����� *	Datierung+�� $��Versicherungswert��.���� $. 10 0 0 5q�$��20(����� *Versicherungswert+�� $��Bemerkungen��.���� $. 10 0 0 5q�$��20(����� *
Herkunft+� -,   $ MetadataPanel��$
��Auto��$
��Auto��$ * * * *��$ * auto��y��� *MetadataPanelVisibility+.+��� $	��0.5q�
�������$��0��$��0��._�� $ * auto auto auto auto��.+��� --   $& DepartementeMeta��$��0��/ �� SelectionMode$/ Single=����� *Departemnte+���� .R��� ���� *Name+�� $��1��$��Departemente���� 0 �� CornerRadius$0 30��
���   �$	��0.5q�$��2��._��� $��10 10 10 10q�.+��� $
��Name��.���� -.   $ NewDepartementTextbox��� $��3��$��50��$	 * *��{��� *DepartementEditEnabeled+.+��� -/   $��	Speichern��$��0���� -0   $��	Abbrechen��$��1���� -1   $��4��$��50��.��Hinzufügen�� $	��0.5q�
�������$��1��$��0��._�� $ * auto auto auto auto��.+��� -2   $& SprachgruppeMeta��$��0��$/ Single=����� *Sprachgruppen+���� .R��� ���� *Name+�� $��1��$��Sprachgruppen���� $0 30��
���   �$	��0.5q�$��2��._��� $��10 10 10 10q�.+��� $
��Name��.���� -3   $ NewSprachgruppeTextbox��� $��3��$��50��$	 * *��{��� *SprachgruppeEditEnabeled+.+��� -4   $��	Speichern��$��0���� -5   $��	Abbrechen��$��1���� -6   $��4��$��50��.��Hinzufügen�� $	��0.5q�
�������$��2��$��0��._�� $ * auto auto auto auto��.+��� -7   $& DorfMeta��$��0��$/ Single=����� *	Dörfer+���� .R��� ���� *Name+�� $��1��$��Dörfer���� $0 30��
���   �$	��0.5q�$��2��._��� $��10 10 10 10q�.+��� $
��Name��.���� -8   $ NewDorfTextbox���� $��Departement��.���� -9   $ NewDorfDepartement������ *Departemnte+���� .R��� ���� *Name+�� $��Sprachgruppe��.���� -:   $ NewDorfSprachgrupe������ *Sprachgruppen+���� .R��� ���� *Name+� $��3��$��50��$	 * *��{��� *DorfEditEnabeled+.+��� -;   $��	Speichern��$��0���� -<   $��	Abbrechen��$��1���� -=   $��4��$��50��.��Hinzufügen�� $	��0.5q�
�������$��3��$��0��._�� $ * auto auto auto auto��.+��� ->   $& KategorieMeta��$��0��$/ Single=����� *
Kategorien+���� .R��� ���� *Name+�� $��1��$��
Kategorien���� $0 30��
���   �$	��0.5q�$��2��._��� $��10 10 10 10q�.+��� $
��Name��.���� -?   $ NewKategorieTextbox���� $��Oberkategorie��.���� -@   $ NewKategorieOberkategorie������ *
Kategorien+���� .R��� ���� *Name+� $��3��$��50��$	 * *��{��� *KategorieEditEnabeled+.+��� -A   $��	Speichern��$��0���� -B   $��	Abbrechen��$��1���� -C   $��4��$��50��.��Hinzufügen�� -D   $��1��$��4��$��0��$��50��.��	Zurück