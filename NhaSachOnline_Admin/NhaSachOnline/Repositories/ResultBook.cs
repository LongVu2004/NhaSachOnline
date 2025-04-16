namespace NhaSachOnline.Repositories;

public record ResultBookModel(string BookName, string AuthorName, int TongSoSach);
public record ResultBookViewModel(DateTime startDate, DateTime endDate, IEnumerable<ResultBookModel> ResulBookModels);
