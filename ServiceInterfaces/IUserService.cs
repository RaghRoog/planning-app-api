
public interface IUserService {
	void RegisterUser(User newUser);
	User AuthenticateUser(string email, string password);
}
