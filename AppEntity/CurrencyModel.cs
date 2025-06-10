namespace AppEntity
{
	// Döviz bilgileri için model
	public class CurrencyModel
	{
		public string Code { get; set; } = "Bilinmeyen";
		public string Alis { get; set; } = "0.00";
		public string Satis { get; set; } = "0.00";
		public string Fark { get; set; } = "0.00";
		public string Yon { get; set; } = "";
	}

	// Altın bilgileri için model
	public class GoldModel
	{
		public string Code { get; set; } = "Altın";
		public string Alis { get; set; } = "0.00";
		public string Satis { get; set; } = "0.00";
		public string Degisim { get; set; } = "%0.00";
	}
}