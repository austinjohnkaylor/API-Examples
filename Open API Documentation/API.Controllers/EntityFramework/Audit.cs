namespace API.Controllers.EntityFramework;

/// <summary>
/// Provides audit information for a record
/// </summary>
public class Audit
{
    /// <summary>
    /// When the record was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Who or what the record was created by
    /// </summary>
    public string? CreatedBy { get; set; }
    /// <summary>
    /// When the record was updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Who or what the record was updated by
    /// </summary>
    public string? UpdatedBy { get; set; }
    /// <summary>
    /// When the record was deleted
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    /// <summary>
    /// Who or what the record was deleted by
    /// </summary>
    public string? DeletedBy { get; set; }
    /// <summary>
    /// Whether or not the record is active
    /// </summary>
    public bool IsActive { get; set; }
}