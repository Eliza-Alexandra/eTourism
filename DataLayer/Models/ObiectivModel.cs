[System.ComponentModel.DataAnnotations.Schema.Table("obiective")]

public class Obiectiv
{
    [System.ComponentModel.DataAnnotations.Key]
    [System.ComponentModel.DataAnnotations.Schema.Column("id_obiectiv")]
    public int Id { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("denumire")]
    public string Denumire { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("adresa")]
    public string Adresa { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("detalii")]
    public string Detalii { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("imagine")]
    public string Imagine { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("id_categorie")]
    public int IdCategorie { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.ForeignKey(nameof(IdCategorie))]
    public CategorieObiectiv? Categorie { get; set; }
}
