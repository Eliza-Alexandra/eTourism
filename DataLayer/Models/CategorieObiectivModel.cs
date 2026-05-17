[System.ComponentModel.DataAnnotations.Schema.Table("categorii_obiective")]
public class CategorieObiectiv
{
    [System.ComponentModel.DataAnnotations.Key]
    [System.ComponentModel.DataAnnotations.Schema.Column("id_categorie")]
    public int IdCategorie { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("nume_categorie")]
    public string NumeCategorie { get; set; } = string.Empty;
}
