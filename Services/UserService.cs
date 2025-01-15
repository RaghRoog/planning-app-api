
public class UserService : IUserService {
	private readonly AppDbContext _context;

	public UserService(AppDbContext context) {
		_context = context;
	}


	public void RegisterUser(User newUser) {
		if(_context.Users.Any(u => u.Email == newUser.Email)) {
			throw new Exception("User with this email already exists");
		}

		_context.Users.Add(newUser);
		_context.SaveChanges();
	}

	public User AuthenticateUser(string email, string password) {
		var user = _context.Users.FirstOrDefault(u => u.Email == email);
		if(user == null || user.Password != password) {
			return null;
		}

		return user;
	}
}
