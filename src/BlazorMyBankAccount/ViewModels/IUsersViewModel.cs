using BlazorMyBankAccount.Data;

namespace BlazorMyBankAccount.ViewModels
{
    public interface IUsersViewModel
    {
		List<UserView> AllUsers { get; set; }
		NavigationManager Navigation { get; set; }
		UserManager<IdentityUser> UserManager { get; set; }
		bool ShowResetMdp { get; set; }

		bool IsLoaded { get; set; }

		Task OnChangeRole(ChangeEventArgs e, string idUser);

		void DeleteUser(string idUser);

		void OpenChangeMdp(string idUser);

		void CancelChangeMdp();

		Task SetNewPassword(string newPassword);

		Task LoadAllUsers();
	}
}
