using System.ComponentModel.DataAnnotations;

public record StudentDto(
    int Id,

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    string Name,

    [Required(ErrorMessage = "El documento es requerido")]
    [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
    string Document,

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    string Email
);
