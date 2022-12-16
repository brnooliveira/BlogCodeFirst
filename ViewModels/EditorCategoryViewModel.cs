using System.ComponentModel.DataAnnotations;

namespace Blogao.ViewModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "O nome e obrigatorio!")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O Slug e obrigatorio!")]
    public string Slug { get; set; }
}