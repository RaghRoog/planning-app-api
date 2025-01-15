
using System.ComponentModel.DataAnnotations;

public class User {
	public long Id { get; set; }
	[Required]
	[StringLength(50)]
	public string Username { get; set; }
	[Required]
	[EmailAddress]
	public string Email { get; set; }
	[Required]
	[MinLength(5)]
	public string Password { get; set; }
}
