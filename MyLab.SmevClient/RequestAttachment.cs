using System.Collections.Generic;

namespace MyLab.SmevClient;

/// <summary>
/// Вложение запроса
/// </summary>
public class RequestAttachment
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Содержание в формате BASE64
    /// </summary>
    public string ContentBase64 { get; set; }
    /// <summary>
    /// Тип содежимого
    /// </summary>
    public string MimeType { get; set; }
    /// <summary>
    /// ЭП PKCS7 в формате BASE64
    /// </summary>
    public string SignatureBase64 { get; set; }
}