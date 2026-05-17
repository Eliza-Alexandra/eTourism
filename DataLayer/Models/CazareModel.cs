[System.ComponentModel.DataAnnotations.Schema.Table("cazari")]
public class Cazare
{
    [System.ComponentModel.DataAnnotations.Key]
    [System.ComponentModel.DataAnnotations.Schema.Column("id_cazare")]
    public int Id { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("nume")]
    public string Nume { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("adresa")]
    public string Adresa { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("descriere")]
    public string Descriere { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("imagine")]
    public string Imagine { get; set; } = string.Empty;
}
