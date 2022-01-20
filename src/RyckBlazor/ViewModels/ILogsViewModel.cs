namespace RyckBlazor.ViewModels
{
	public interface ILogsViewModel
	{
		List<LogEntity> Logs { get; }

		DateTime DateDebutLog { get; set; }

		void OnChangeLevel(ChangeEventArgs e);


		Task LoadLogs();
	}
}
