using System;
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
    public string Id { get; }
    /// <summary>
    /// Содержание в формате BASE64
    /// </summary>
    public string ContentBase64 { get;}
    /// <summary>
    /// Тип содежимого
    /// </summary>
    public string MimeType { get; }

    public RequestAttachment(string id, string contentBase64, string mimeType)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        ContentBase64 = contentBase64 ?? throw new ArgumentNullException(nameof(contentBase64));
        MimeType = mimeType ?? throw new ArgumentNullException(nameof(mimeType));
    }
}