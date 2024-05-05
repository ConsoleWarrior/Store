namespace Store.web.Contractors
{
	public interface IWebContractorService
	{
		string Name { get; }
		Uri StartSession(IReadOnlyDictionary<string, string> parameters, Uri returnUri);
	}
}
