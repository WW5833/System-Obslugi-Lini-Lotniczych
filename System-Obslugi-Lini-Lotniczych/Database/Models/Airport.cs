using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LotSystem.Database.Models;

[Table("airports")]
public sealed class Airport
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }

    public override string ToString()
    {
        return $"{Name} ({ShortName})";
    }
}