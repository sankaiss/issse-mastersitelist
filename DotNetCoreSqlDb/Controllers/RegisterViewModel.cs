using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ange ditt namn.")]
    [Display(Name = "Ditt Namn")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Ange en e-postadress.")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Ange ett lösenord.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "Lösenordet måste innehålla minst en liten bokstav, en stor bokstav, en siffra och ett specialtecken.")]
    [MinLength(8, ErrorMessage = "Lösenordet måste vara minst 8 tecken långt.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Bekräfta lösenordet.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Lösenorden matchar inte.")]
    public string ConfirmPassword { get; set; }

}
