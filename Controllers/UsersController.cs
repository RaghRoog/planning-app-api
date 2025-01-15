using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
	private readonly IUserService _userService;

	public UsersController(IUserService userService) {
		_userService = userService;
	}

	[HttpPost("register")]
	public ActionResult Register([FromBody] User newUser) { 
		if(!ModelState.IsValid) {
			return BadRequest(ModelState);
		}

		try {
			_userService.RegisterUser(newUser);
			return Ok(new { Message = "Registered" });
		}catch(Exception ex) {
			return Conflict(new { Message = ex.Message });
		}
	}

	[HttpPost("login")]
	public ActionResult Login([FromBody] LoginRequest loginRequest) {
		if(!ModelState.IsValid) {
			return BadRequest(ModelState);
		}

		var user = _userService.AuthenticateUser(loginRequest.Email, loginRequest.Password);
		if(user == null) {
			return Unauthorized(new { Message = "Wrong email or password" });
		}

		return Ok(user.Id);
	}
}
