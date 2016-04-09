namespace Katalog
{
    public partial class Kategorie
    {
        public string Crumb => string.IsNullOrWhiteSpace(Oberkategorie.Name)||Oberkategorie.Name == Name||Oberkategorie.Name=="Root"?Name: Oberkategorie.Crumb+" > "+Name;
    }
}