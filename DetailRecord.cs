
public class DetailRecord
{
    public long ID { get; set; }

    public long BatchID { get; set; }

    public string EID { get; set; }

    public string GLIN { get; set; }

    public string Application_ID { get; set; }

    public DateTime? RowInsertDate { get; set; }

    public DateTime? RowUpdateDate { get; set; }

    public string Comments { get; set; }

    public int Status { get; set; }
    public string StatusText { get; set; }
    public string? ErrorMessage { get; set; }
}
